/* 
   Spot Deform for Cube Marching Algorithm
   ---------------------------------------

   This class interacts with a cube marching algorithm to allow the user 
   to dynamically deform terrain during gameplay, in real time. 
   This script is attached to a mesh object, takes the vertex data from the mesh,
   and converts the 3D local vertex data to 3D world points.
   These points are passed to the cube marching algorthim.

   Version: 2.0    

 */

//
// System Dependencies
// -------------------
using System;
using System.Collections;

//
// External Dependencies
// -------------------
using UnityEngine;

public class SpotDeform_TerrainEdit : MonoBehaviour
{
    [SerializeField] protected bool addTerrain = false;
    [SerializeField] protected float range = 2f;
    protected float force = 2f;
    [SerializeField] protected AnimationCurve forceOverDistance = AnimationCurve.Constant(0, 1, 1);
    protected World world;
    protected GameObject searchedTagGameObject;
    [SerializeField] protected string objectTagLookup;
    [SerializeField] protected Transform attachedMeshObject;
    protected Vector3[] vertices;
    protected Chunk[] initializeChunk;
    protected Vector3 worldPoint;
    protected String coroutineMethod = "FrameUpdate";

    // 
    // Start is the entry point for Unity scripts
    // There are some key attributes that are needed for the script to run. 
    // Start method validates that the attributes can
    // be initialsied, otherwise it will throw an exception, and 
    // terminate if the attributes cannot be successfully initialised.
    void Start()
    { 
        searchedTagGameObject = GameObject.FindGameObjectWithTag(objectTagLookup);

        if (searchedTagGameObject != null)
            world = searchedTagGameObject.GetComponent<World>();

        else if (searchedTagGameObject == null)
            throw new NullReferenceException();

        Mesh mesh = attachedMeshObject.GetComponent<MeshFilter>().mesh;
        vertices = mesh.vertices;

        initializeChunk = new Chunk[8];

        UpdateFrame();
    }

    // 1 pass must be performed across Each Vector (X,Y,Z) held in vertices array, tranforming each vertex position.
    // Looping through each vertex happens on the main thread (Unity does not allow threaded transforms).
    // This locks the program for the duration of the loop. The more Vectors present, the more vertex data, 
    // the longer the time required before the main thread can continue to process other tasks.
    // To work around this issue, this method starts a coroutine running the given method. 
    // The coroutine will yeild to the main thread during the loop, allowing other tasks to be performed while 
    // the loop is running.
    public virtual void UpdateFrame()
    {
        StartCoroutine(coroutineMethod);
    }

    // This method runs as a coroutine, the results of each pass update the frame. Inherently, the task 
    // may not be completed in a single frame taking multiple frames to update fully. The vertex points 
    // for the attached mesh obj are stored as a local coordinate. This local coordinate must be transformed 
    // into a world coordinate that shares the same space as the "world" gameObject. 
    // This transformed point data can then be passed to the terrain editor.   
    IEnumerator FrameUpdate()
    {
        foreach (Vector3 vert in vertices)
        {
            worldPoint = transform.TransformPoint(vert);
            EditTerrain(worldPoint, addTerrain, force, range);

            yield return new WaitForSeconds(.1f);
        }

        Destroy(gameObject);
    }

    /* 
    This function takes the transformed vertex data and passes it through to the cube marching algorithm.
    Loping through the X, Y, Z, coordinates.

    The "marching cubes" exist within a 3D bounding box. The point data is first tested for presence within the 
    bounding box.
    If the point is within the bounding box, the coordinate will modify the nodes that make up the 
    'marching cube', then set the new world data.    
     */
    protected void EditTerrain(Vector3 point, bool addTerrain, float force, float range)
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
                    if (!(distance <= range)) 
                        continue;

                    float modificationAmount = force / distance * forceOverDistance.Evaluate(1 - distance.Map(0, force, 0, 1)) * buildModifier;

                    float oldDensity = world.GetDensity(offsetX, offsetY, offsetZ);
                    float newDensity = oldDensity - modificationAmount;

                    newDensity = newDensity.Clamp01();

                    world.SetDensity(newDensity, offsetX, offsetY, offsetZ, true, initializeChunk);
                }
            }
        }
    }

    // Construct is used for testing purpose to fulfill Testing Driven Design
    // Since class methods and parameters are defined in the editor, it
    // is difficult to set these values in a remote test.
    // 'Construct' gives the test some way to instantiate the script with testable variables.
    // Construct functions similar to the Constructors in OOP.
    public void Construct(String tag, GameObject searchedTagGameObject)
    {

        this.objectTagLookup = tag;
        this.searchedTagGameObject = searchedTagGameObject;

    }
}
