using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpotDeform_TerrainEdit_ver2 : MonoBehaviour
{
    [SerializeField] private bool addTerrain = true;
    [SerializeField] private float force = 2f;
    [SerializeField] private float range = 2f;

    [SerializeField] private float maxReachDistance = 1f;

    [SerializeField] private AnimationCurve forceOverDistance = AnimationCurve.Constant(0, 1, 1);
    private World world;
    private GameObject temp;    
    [SerializeField] private Transform attachedMeshObject;
    private Vector3[] vertices;
    Mesh mesh;
    Chunk[] _initChunks;
    private Vector3 worldPoint;

    // Start is called before the first frame update
    void Start()
    {
        // Initialise Cube Chunk
        _initChunks = new Chunk[8];

        // Get mesh data from attached Object
        mesh = attachedMeshObject.GetComponent<MeshFilter>().mesh;
        
        // Get vertex data from Mesh
        vertices = mesh.vertices;
        
        // Find instance of Gameworld from scene
        temp = GameObject.Find("World_Test");

        // If game world found, assign to world
        if (temp != null)
        {
            world = temp.GetComponent<World>();
        }

        // Update frame
        UpdateFrame();
    }

    void UpdateFrame()
    {
        // loop through each vertex to get 3D point
        foreach (Vector3 vert in vertices)
        {
            // Convert 3D local point to world point
            worldPoint = transform.TransformPoint(vert);
            // Pass point to terrain editor
            EditTerrain(worldPoint, addTerrain, force, range);
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
