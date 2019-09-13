/* MIT License

Copyright (c) 2019 Eldemarkki

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE. */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    [HideInInspector] public bool readyforUpdate;
    [HideInInspector] public Point[,,] points;
    [HideInInspector] public int chunkSize;
    [HideInInspector] public Vector3Int position;

    private float _isoLevel;
    private int _seed;
    
    private MarchingCubes _marchingCubes;
    private MeshFilter _meshFilter;
    private MeshCollider _meshCollider;
    private GenerateDensity _generateDensity;

    // Start is called before the first frame update
    void Start()
    {
        Generate();
    }

    private void Awake(){
        _meshFilter = GetComponent<MeshFilter>();
        _meshCollider = GetComponent<MeshCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        if(readyforUpdate)
        {
            Generate();
            readyforUpdate = false;
        }
    }

    public void Initialize(World world, int chunkSize, Vector3Int position)
    {
        this.chunkSize = chunkSize;
        this.position = position;
        _isoLevel = world.isolevel;

        _generateDensity = world.densityGenerator;
        
        int worldPosX = position.x;
        int worldPosY = position.y;
        int worldPosZ = position.z;

        points = new Point[chunkSize + 1, chunkSize + 1, chunkSize + 1];

        _seed = world.seed;
        _marchingCubes = new MarchingCubes(points, _isoLevel, _seed);

        for (int x = 0; x < points.GetLength(0); x++)
        {
            for (int y = 0; y < points.GetLength(1); y++)
            {
                for (int z = 0; z < points.GetLength(2); z++)
                {
                    points[x, y, z] = new Point(
                        new Vector3Int(x, y, z),
                        _generateDensity.CalculateDensity(x + worldPosX, y + worldPosY, z + worldPosZ)
                    );
                }
            }
        }
    }

    public void Generate()
    {
        Mesh mesh = _marchingCubes.CreateMeshData(points);
        
        _meshFilter.sharedMesh = mesh;
        _meshCollider.sharedMesh = mesh;
    }

    public Point GetPoint(int x, int y, int z)
    {
        return points[x, y, z];
    }

    public void SetDensity(float density, int x, int y, int z)
    {
        points[x, y, z].density = density;
    }

    public void SetDensity(float density, Vector3Int pos)
    {
        SetDensity(density, pos.x, pos.y, pos.z);
    }
}
