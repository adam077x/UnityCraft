using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    Generates chunks
    Is running in diffrent THREAD
 */
public class ChunkGeneratorManager
{
    public Chunk[,] chunks;
    public Chunk[,] deadChunks;
    public Vector3 playerPosition;

    public ChunkGeneratorManager(Chunk[,] chunks, Chunk[,] deadChunks, Vector3 playerPosition)
    {
        this.chunks = chunks;
        this.deadChunks = deadChunks;
        this.playerPosition = playerPosition;
    }

    public void Execute()
    {
        while (true)
        {
            for (int i = 0; i < World.MaxChunks; i++)
            {
                for (int j = 0; j < World.MaxChunks; j++)
                {
                    // If player is out of range of 9 chunks
                    if (Vector3.Distance(new Vector3(playerPosition.x, 0, playerPosition.z), new Vector3(i * Chunk.WIDTH, 0, j * Chunk.WIDTH)) >
                        Chunk.WIDTH * 9)
                    {
                        if (chunks[i, j] != null)
                        {
                            chunks[i, j].active = false;
                            deadChunks[i, j] = chunks[i, j];
                            chunks[i, j] = null;
                        }
                    }
                    else // If player is in range of 9 chunks
                    {
                        if (chunks[i, j] == null && deadChunks[i, j] != null)
                        {
                            chunks[i, j] = deadChunks[i, j];
                            chunks[i, j].x = i;
                            chunks[i, j].z = j;
                            chunks[i, j].CreateMeshData();
                            deadChunks[i, j].active = false;
                            chunks[i, j].active = true;
                            chunks[i, j].resetMesh = true;
                        }
                        else if (chunks[i, j] == null && deadChunks[i, j] == null)
                        {
                            chunks[i, j].x = i;
                            chunks[i, j].z = j;
                            chunks[i, j].CreateMeshData();
                            chunks[i, j].resetMesh = true;
                        }
                        else if (chunks[i, j] != null && deadChunks[i, j] == null)
                        {
                            chunks[i, j].x = i;
                            chunks[i, j].z = j;
                            chunks[i, j].CreateMeshData();
                            chunks[i, j].resetMesh = true;
                        }
                    }
                }
            }
        }
    }
}