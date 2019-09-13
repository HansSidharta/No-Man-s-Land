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
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour
{
    public int chunkSize = 8;

    public int worldWidth = 5;
    public int worldHeight = 5;
    public int worldDepth = 5;

    public float isolevel;

    public int seed;

    public GameObject chunkPrefab;

    public Dictionary<Vector3Int, Chunk> chunks;

    private Bounds worldBounds;

    public GenerateDensity densityGenerator;

    private void Awake()
    {
        densityGenerator = new GenerateDensity();
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(worldBounds.center, worldBounds.size);
    }
    
    private void Start()
    {
        worldBounds = new Bounds();
        UpdateBounds();

        chunks = new Dictionary<Vector3Int, Chunk>(worldWidth*worldHeight*worldDepth);;
        CreateChunks();
    }

    private void CreateChunks()
    {
        for (int x = 0; x < worldWidth; x++)
        {
            for (int y = 0; y < worldHeight; y++)
            {
                for (int z = 0; z < worldDepth; z++)
                {
                    CreateChunk(x * chunkSize, y * chunkSize, z * chunkSize);
                }
            }
        }
    }

    private Chunk GetChunk(Vector3Int pos)
    {
        return GetChunk(pos.x, pos.y, pos.z);
    }

    public Chunk GetChunk(int x, int y, int z)
    {
        int newX = Utils.FloorToNearestX(x, chunkSize);
        int newY = Utils.FloorToNearestX(y, chunkSize);
        int newZ = Utils.FloorToNearestX(z, chunkSize);
        
        return chunks[new Vector3Int(newX, newY, newZ)];
    }

    public float GetDensity(int x, int y, int z)
    {
        Point p = GetPoint(x, y, z);

        return p.density;
    }

    public float GetDensity(Vector3Int pos)
    {
        return GetDensity(pos.x, pos.y, pos.z);
    }

    public Point GetPoint(int x, int y, int z)
    {
        Chunk chunk = GetChunk(x, y, z);

        Point p = chunk.GetPoint(x.Mod(chunkSize),
                                 y.Mod(chunkSize),
                                 z.Mod(chunkSize));

        return p;
    }

    public void SetDensity(float density, int worldPosX, int worldPosY, int worldPosZ, bool setReadyForUpdate, Chunk[] initChunks)
    {
        Vector3Int dp = new Vector3Int(worldPosX, worldPosY, worldPosZ);

        Vector3Int lastChunkPos = dp.FloorToNearestX(chunkSize);

        for (int i = 0; i < 8; i++)
        {
            Vector3Int chunkPos = (dp - MarchingCubes.CubePoints[i]).FloorToNearestX(chunkSize);

            if (i != 0 && chunkPos == lastChunkPos)
            {
                continue;
            }

            Chunk chunk = GetChunk(chunkPos);
            
            lastChunkPos = chunk.position;

            Vector3Int localPos = (dp - chunk.position).Mod(chunkSize + 1);

            chunk.SetDensity(density, localPos);
            if (setReadyForUpdate) 
                chunk.readyforUpdate = true;
        }
    }

    public void SetDensity(float density, Vector3Int pos, bool setReadyForUpdate, Chunk[] initChunks)
    {
        SetDensity(density, pos.x, pos.y, pos.z, setReadyForUpdate, initChunks);
    }

    private void UpdateBounds()
    {
        float middleX = worldWidth * chunkSize / 2f;
        float middleY = worldHeight * chunkSize / 2f;
        float middleZ = worldDepth * chunkSize / 2f;
        
        Vector3 midPos = new Vector3(middleX, middleY, middleZ);

        Vector3Int size = new Vector3Int(
            worldWidth * chunkSize,
            worldHeight * chunkSize,
            worldDepth * chunkSize);

        worldBounds.center = midPos;
        worldBounds.size = size;
    }

    public bool IsPointInsideWorld(int x, int y, int z)
    {
        return IsPointInsideWorld(new Vector3Int(x, y, z));
    }

    public bool IsPointInsideWorld(Vector3Int point)
    {
        return worldBounds.Contains(point);
    }

    private void CreateChunk(int x, int y, int z)
    {
        Vector3Int position = new Vector3Int(x, y, z);

        Chunk chunk = Instantiate(chunkPrefab, position, Quaternion.identity).GetComponent<Chunk>();
        chunk.Initialize(this, chunkSize, position);
        chunks.Add(position, chunk);
    }
}
