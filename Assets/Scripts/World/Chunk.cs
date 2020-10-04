using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    // WIDTH, HEIGHT of chunk
    public static int WIDTH = 16;
    public static int HEIGHT = 128;
    
    private MeshFilter meshFilter;
    private MeshCollider meshCollider;
    
    public byte[,,] chunkBlocks = new byte[WIDTH,HEIGHT,WIDTH];

    // Chunk data
    private int vertexIndex = 0;
    private List<Vector3> vertices = new List<Vector3>();
    private List<int> triangles = new List<int>();
    private List<Vector2> uvs = new List<Vector2>();

    [Header("Chunk Position")]
    public int x, z;

    [Header("Variables")]
    public bool active = false;
    public bool resetMesh;
    
    private void Start()
    {
        meshFilter = GetComponent<MeshFilter>();
        meshCollider = GetComponent<MeshCollider>();

        resetMesh = true;
    }
    
    private void Update()
    {
        if (resetMesh)
        {
            CreateMesh();
            resetMesh = false;
        }
    }

    /// <summary>
    /// Checks if block is on certain position
    /// </summary>
    private bool CheckBlock(Vector3 position)
    {
        int x = Mathf.FloorToInt(position.x);
        int y = Mathf.FloorToInt(position.y);
        int z = Mathf.FloorToInt(position.z);

        if (x < 0 || x > WIDTH - 1 || y < 0 || y > HEIGHT - 1 || z < 0 || z > WIDTH - 1)
            return false;

        if (chunkBlocks[x, y, z] == 0)
            return false;

        return World.instance.blockTypes[chunkBlocks[x, y, z]].solid;
    }

    /// <summary>
    /// Puts blocks into chunk based on noise map
    /// </summary>
    public void CreateBlocksInChunk()
    {
        for (int x = 0; x < WIDTH; x++)
        {
            for (int z = 0; z < WIDTH; z++)
            {
                float height = World.instance.noiseMapHeight.GetPixel(x + (this.x * WIDTH), z + (this.z * WIDTH)).r * 64 + 6;

                float treeMap = World.instance.noiseMapTrees.GetPixel(x + (this.x * WIDTH), z + (this.z * WIDTH)).g;
                for (int i = 0; i < height; i++)
                {
                    chunkBlocks[x, i, z] = 3;
                }
                
                chunkBlocks[x, 0, z] = 4;

                chunkBlocks[x, (int) height, z] = 5;
                chunkBlocks[x, (int) height-1, z] = 1;
                chunkBlocks[x, (int) height-2, z] = 1;
            }
        }
    }

    /// <summary>
    /// Creates mesh for chunk vertices, indices...
    /// </summary>
    public void CreateMeshData()
    {
        vertices.Clear();
        triangles.Clear();
        uvs.Clear();
        vertexIndex = 0;
        
        for (int y = 0; y < HEIGHT; y++)
        {
            for (int x = 0; x < WIDTH; x++)
            {
                for (int z = 0; z < WIDTH; z++)
                {
                    CreateBlockData(new Vector3(x, y, z));
                }
            }
        }
    }

    /// <summary>
    /// Creates vertices and triangles based on chunk blocks
    /// <para>Deletes sides that are not visible to player</para>
    /// </summary>
    public void CreateBlockData(Vector3 position)
    {
        for (int i = 0; i < 6; i++)
        {
            if (!CheckBlock(position + BlockData.faceChecks[i]))
            {
                byte blockID = chunkBlocks[(int) position.x, (int) position.y, (int) position.z];

                if (blockID == 0) return;

                vertices.Add(position + BlockData.blockVertices[BlockData.blockTriangles[i, 0]]);
                vertices.Add(position + BlockData.blockVertices[BlockData.blockTriangles[i, 1]]);
                vertices.Add(position + BlockData.blockVertices[BlockData.blockTriangles[i, 2]]);
                vertices.Add(position + BlockData.blockVertices[BlockData.blockTriangles[i, 3]]);

                AddTexture(World.instance.blockTypes[blockID].GetTextureID(i));

                triangles.Add(vertexIndex);
                triangles.Add(vertexIndex + 1);
                triangles.Add(vertexIndex + 2);
                triangles.Add(vertexIndex + 2);
                triangles.Add(vertexIndex + 1);
                triangles.Add(vertexIndex + 3);

                vertexIndex += 4;
            }
        }
    }

    /// <summary>
    /// Adds texture based on textureID to uvs list
    /// </summary>
    private void AddTexture(int textureID)
    {
        float y = textureID / BlockData.NumberOfTextures;
        float x = textureID - (y * BlockData.NumberOfTextures);

        x *= BlockData.GetBlockTextureSize();
        y *= BlockData.GetBlockTextureSize();

        y = 1f - y - BlockData.GetBlockTextureSize();
        
        uvs.Add(new Vector2(x, y));
        uvs.Add(new Vector2(x, y + BlockData.GetBlockTextureSize()));
        uvs.Add(new Vector2(x + BlockData.GetBlockTextureSize(), y));
        uvs.Add(new Vector2(x + BlockData.GetBlockTextureSize(), y + BlockData.GetBlockTextureSize()));
    }

    /// <summary>
    /// Sets chunk mesh to chunk data (vertices, triangles, uvs)
    /// <para>Updates mesh collider</para>
    /// <para>Recalculates Normals</para>
    /// </summary>
    public void CreateMesh()
    {
        Mesh mesh = new Mesh();

        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.uv = uvs.ToArray();
        
        mesh.RecalculateNormals();

        meshFilter.mesh = mesh;
        meshCollider.sharedMesh = meshFilter.mesh;
    }
}
