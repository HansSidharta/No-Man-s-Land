using System.Collections;
using UnityEngine;

public class BrushDeform_TerrainEdit : SpotDeform_TerrainEdit
{
    Vector3 lastposition, currentPosition;

    public override void UpdateFrame()
    {
        Update();
    }

    void Update()
    {
        // Update run every frame
        // Set up co-routine to save perf (~25-40% saved)
        // Coroutine runs on main thread. Run non-critical code
        // Start Coroutine
        StartCoroutine(coroutineMethod);
    }

    IEnumerator FrameUpdate()
    {
        // Save performance by checking if object is in motion.
        // Only need to update world if the objects last known position differs from the current location 

        currentPosition = transform.TransformPoint(vertices[0]);

        if (currentPosition != lastposition)
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

        lastposition = currentPosition;
    }
}
