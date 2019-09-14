using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrushDeform_TerrainEdit : MonoBehaviour
{
    [SerializeField] private bool addTerrain = false;
    [SerializeField] private float force = 2f;
    [SerializeField] private float range = 2f;

    [SerializeField] private float maxReachDistance = 100f;

    [SerializeField] private AnimationCurve forceOverDistance = AnimationCurve.Constant(0, 1, 1);

    private World world;

    private GameObject temp;
    [SerializeField] private string ObjectLookup; 
    [SerializeField] private Transform attachedMeshObject;
    
    Chunk[] _initChunks;
    Mesh mesh;
    private Vector3[] vertices;
    private Vector3 worldPoint;

    // Start is called before the first frame update
    void Start()
    {
        // Script needs reference to world, world object name must be given
        if(ObjectLookup == null)
        {
            throw new System.NullReferenceException();
        }
        // Initialise Cube chunk
        _initChunks = new Chunk[8];

        // Get mesh data from attached object
        mesh = attachedMeshObject.GetComponent<MeshFilter>().mesh;   
        // Get vertex data from attached mesh    
        vertices = mesh.vertices;

        // Search entire scene to find Gameworld
        temp = GameObject.Find("World_Test");

        // If found set to world
        if (temp != null)
        {
            world = temp.GetComponent<World>();
        }
    }

    void Update()
    {
        // Update run every frame
        // Set up co-routine to save perf (~40-50% saved)
        // Coroutine runs on main thread. Run non-critical code
        // Start Coroutine
        StartCoroutine("UpdateFrame");
    }

    IEnumerator UpdateFrame()
    {
        // Loop through each vertex in verticies
        foreach (Vector3 vert in vertices)
        {
            // Convert vertex from local to world location
            worldPoint = transform.TransformPoint(vert);

            // Pass world point through to terrain editor
            EditTerrain(worldPoint, addTerrain, force, range);

            // Yeild to the main thread. Resume looping when feasible
            yield return new WaitForSeconds(.1f);
        }        
    }

    private void EditTerrain(Vector3 point, bool addTerrain, float force, float range)
    {
        // strength modifier
        int buildModifier = addTerrain ? 1 : -1;

        // round 3D floats to ints
        int hitX = point.x.Round();
        int hitY = point.y.Round();
        int hitZ = point.z.Round();

        // Round up float to int
        int intRange = range.Ceil();

        // loop through all points in world
        for (int x = -intRange; x <= intRange; x++)
        {
            for (int y = -intRange; y <= intRange; y++)
            {
                for (int z = -intRange; z <= intRange; z++)
                {
                    int offsetX = hitX - x;
                    int offsetY = hitY - y;
                    int offsetZ = hitZ - z;


                    // Check if given points are within world bounds
                    if (!world.IsPointInsideWorld(offsetX, offsetY, offsetZ))
                        continue;

                    float distance = Utils.Distance(offsetX, offsetY, offsetZ, point);
                    if (!(distance <= range)) continue;

                    float modificationAmount = force / distance * forceOverDistance.Evaluate(1 - distance.Map(0, force, 0, 1)) * buildModifier;
                    
                    // Generate new world data
                    float oldDensity = world.GetDensity(offsetX, offsetY, offsetZ);
                    float newDensity = oldDensity - modificationAmount;

                    newDensity = newDensity.Clamp01();

                    // Set new world data
                    world.SetDensity(newDensity, offsetX, offsetY, offsetZ, true, _initChunks);
                }
            }
        }
    }
}
