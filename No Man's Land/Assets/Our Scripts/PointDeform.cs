using System.Collections;
using UnityEngine;

public class PointDeform : SpotDeform_TerrainEdit
{
    public virtual void UpdateFrame()
    {
        foreach (Vector3 vert in vertices)
        {
            worldPoint = transform.TransformPoint(vert);
            EditTerrain(worldPoint, addTerrain, force, range);
        }

        Destroy(gameObject);
    }
}
