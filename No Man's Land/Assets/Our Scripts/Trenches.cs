using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trenches : MonoBehaviour
{
    [SerializeField] private bool addTerrain = false;
    [SerializeField] private float force = 2f;
    [SerializeField] private float range = 2f;

    [SerializeField] private float maxReachDistance = 100f;

    [SerializeField] private AnimationCurve forceOverDistance = AnimationCurve.Constant(0, 1, 1);

    [SerializeField] private World world;
    [SerializeField] private Transform attachedMeshObject;
    Chunk[] _initChunks;
    Mesh mesh;
    private Vector3[] vertices;
    private Vector3 worldPoint;

    // Start is called before the first frame update
    void Start()
    {
        mesh = attachedMeshObject.GetComponent<MeshFilter>().mesh;
        _initChunks = new Chunk[8];
        vertices = mesh.vertices;
    }

    void Update()
    {
        StartCoroutine("UpdateFrame");
    }

    IEnumerator UpdateFrame()
    {
        foreach (Vector3 vert in vertices)
        {
            worldPoint = transform.TransformPoint(vert);
            EditTerrain(worldPoint, addTerrain, force, range);
        }

        yield return new WaitForSeconds(.1f);
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
