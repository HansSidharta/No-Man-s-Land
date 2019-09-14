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
        mesh = attachedMeshObject.GetComponent<MeshFilter>().mesh;
        _initChunks = new Chunk[8];
        vertices = mesh.vertices;
        
        temp = GameObject.Find("World_Test");
        if (temp != null)
        {
            world = temp.GetComponent<World>();
        }

        UpdateFrame();
    }

    void UpdateFrame()
    {
        foreach (Vector3 vert in vertices)
        {
            worldPoint = transform.TransformPoint(vert);
            EditTerrain(worldPoint, addTerrain, force, range);
        }
    }

    private void EditTerrain(Vector3 point, bool addTerrain, float force, float range)
    {
        int buildModifier = addTerrain ? 1 : -1;

        int hitX = point.x.Round();
        int hitY = point.y.Round();
        int hitZ = point.z.Round();

        int intRange = range.Ceil();

        for (int x = -intRange; x <= intRange; x++)
        {
            for (int y = -intRange; y <= intRange; y++)
            {
                for (int z = -intRange; z <= intRange; z++)
                {
                    int offsetX = hitX - x;
                    int offsetY = hitY - y;
                    int offsetZ = hitZ - z;

                    if (!world.IsPointInsideWorld(offsetX, offsetY, offsetZ))
                        continue;

                    float distance = Utils.Distance(offsetX, offsetY, offsetZ, point);
                    if (!(distance <= range)) continue;

                    float modificationAmount = force / distance * forceOverDistance.Evaluate(1 - distance.Map(0, force, 0, 1)) * buildModifier;

                    float oldDensity = world.GetDensity(offsetX, offsetY, offsetZ);
                    float newDensity = oldDensity - modificationAmount;

                    newDensity = newDensity.Clamp01();

                    world.SetDensity(newDensity, offsetX, offsetY, offsetZ, true, _initChunks);
                }
            }
        }
    }
}
