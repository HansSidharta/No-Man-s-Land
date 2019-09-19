using System;
using System.Collections.Generic;
using UnityEngine;

public class SpotDeform_TerrainEdit_ver2 : MonoBehaviour
{
    [SerializeField] protected bool addTerrain = true;
    [SerializeField] protected float range = 2f;
    protected float force = 2f;
    [SerializeField] protected AnimationCurve forceOverDistance = AnimationCurve.Constant(0, 1, 1);
    protected World world;
    GameObject temp;   
    [SerializeField] protected string ObjectLookupTag; 
    [SerializeField] protected Transform attachedMeshObject;
    protected Vector3[] vertices;
    Mesh mesh;
    protected Chunk[] _initChunks;
    protected Vector3 worldPoint;

    // Start is called before the first frame update
    void Start()
    {
        // Given Tag must be true and active. Ability to edit terrain is predicatd on finding the tag assosiated with 'World'
        // Get list of known tags from unity
        string[] check = UnityEditorInternal.InternalEditorUtility.tags;

        // Check if object tag is true and active
        if(ObjectLookupTag == null || !(Array.Exists(check, element => element == ObjectLookupTag)))
        {
            throw new System.NullReferenceException();
        }
        
        // Initialise Cube Chunk
        _initChunks = new Chunk[8];

        // Get mesh data from attached Object
        mesh = attachedMeshObject.GetComponent<MeshFilter>().mesh;
        
        // Get vertex data from Mesh
        vertices = mesh.vertices;
        
        // Find instance of Gameworld from scene using ObjectLookup
        temp = GameObject.FindGameObjectWithTag(ObjectLookupTag);

        // If game world found, assign to world
        if (temp != null)
        {            
            world = temp.GetComponent<World>();
        }

        // Update frame
        UpdateFrame();        
    }

    public virtual void UpdateFrame()
    {
        // loop through each vertex to get 3D point
        foreach (Vector3 vert in vertices)
        {
            // Convert 3D local point to world point
            worldPoint = transform.TransformPoint(vert);
            // Pass world point to terrain editor
            EditTerrain(worldPoint, addTerrain, force, range);
        }

        // Remove object from gameworld
        Destroy(gameObject);
    }

    protected void EditTerrain(Vector3 point, bool addTerrain, float force, float range)
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
