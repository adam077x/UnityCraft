using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Random = UnityEngine.Random;

public class World : MonoBehaviour
{
    public static readonly int MaxChunks = 64;
    
    public static World instance;
    
    public Material material;
    public BlockType[] blockTypes;

    public Chunk[,] chunks;
    public Chunk[,] deadChunks;

    [SerializeField] private GameObject chunk;

    [Header("Noise Maps")]
    public Texture2D noiseMapHeight;
    public Texture2D noiseMapTrees;

    [Header("Seed")]
    public float seedX;
    public float seedY;

    [Space]
    public GameObject player;

    private ChunkGeneratorManager chunkGeneratorManager; // Chunk generator
    private Thread thread; // Thread for chunk generation

    void Start()
    {
        player = GameObject.Find("Player");

        chunks = new Chunk[MaxChunks,MaxChunks];
        deadChunks = new Chunk[MaxChunks,MaxChunks];

        //Generates seed
        seedX = Random.Range(0f, 99999f);
        seedY = Random.Range(0f, 99999f);

        GeneratePerlinNoiseMaps();

        // Instance of this class
        instance = GetComponent<World>();
        
        // Sets player position to the center of world
        player.transform.position = new Vector3((World.MaxChunks * Chunk.WIDTH) / 2, noiseMapHeight.GetPixel(noiseMapHeight.width / 2, noiseMapHeight.height / 2).r*64+20, (World.MaxChunks * Chunk.WIDTH) / 2);

        GenerateTerrain();

        StartThreadForChunkGeneration();
    }

    /// <summary>
    /// Starts THREAD for chunk generation
    /// </summary>
    private void StartThreadForChunkGeneration() 
    {
        chunkGeneratorManager = new ChunkGeneratorManager(chunks, deadChunks, player.transform.position);
        thread = new Thread(new ThreadStart(chunkGeneratorManager.Execute));
        thread.Start();
    }

    /// <summary>
    /// Generates PERLIN NOISE for map generation
    /// </summary>
    private void GeneratePerlinNoiseMaps() 
    {
        noiseMapHeight = PerlinNoise.GenerateTexture(MaxChunks * Chunk.WIDTH, MaxChunks * Chunk.WIDTH, seedX, seedY, 10);
        noiseMapTrees = PerlinNoise.GenerateTexture(MaxChunks * Chunk.WIDTH, MaxChunks * Chunk.WIDTH, seedX / 2, seedY / 2, 10);
    }

    private void Update()
    {
        // Passes player position for chunk generator in THREAD
        chunkGeneratorManager.playerPosition = player.transform.position;
        
        // Enables or Disables chunks based on active variable
        for (int i = 0; i < MaxChunks; i++)
        {
            for (int j = 0; j < MaxChunks; j++)
            {
                if (chunks[i, j] != null)
                {
                    chunks[i, j].gameObject.SetActive(chunks[i, j].active);
                }
                else if (deadChunks[i, j] != null)
                {
                    deadChunks[i, j].gameObject.SetActive(deadChunks[i, j].active);
                }
            }
        }
    }

    void GenerateTerrain()
    {
        for (int i = 0; i < MaxChunks; i++)
        {
            for (int j = 0; j < MaxChunks; j++)
            {
                deadChunks[i, j] = Instantiate(chunk, new Vector3(i * Chunk.WIDTH, 0, j * Chunk.WIDTH), Quaternion.identity)
                    .GetComponent<Chunk>();
                deadChunks[i, j].x = i;
                deadChunks[i, j].z = j;
                deadChunks[i, j].active = false;
                deadChunks[i, j].transform.parent = transform;
                deadChunks[i, j].name = "chunk_" + deadChunks[i, j].x + "_" + deadChunks[i, j].z;
                deadChunks[i, j].CreateBlocksInChunk();
            }
        }

        for (int x = 0; x < Chunk.WIDTH * MaxChunks; x++)
        {
            for (int z = 0; z < Chunk.WIDTH * MaxChunks; z++)
            {
                float height = World.instance.noiseMapHeight.GetPixel(x, z).r * 64 + 6;
                float treeMap = World.instance.noiseMapTrees.GetPixel(x, z).g;

                float r = UnityEngine.Random.Range(0, 255 * treeMap);
                if (r > 150)
                {
                    float random = Random.Range(0, 5);
                    if (random == 1)
                    {
                        Structures.PlaceTree(x, z, height);
                    }
                }
            }
        }
    }

    /// <summary>
    /// Places/Destroys block in game
    /// </summary>
    public void EditBlock(Vector3 position, byte id)
    {
        Vector3Int pos = new Vector3Int(Mathf.FloorToInt(position.x), Mathf.FloorToInt(position.y), Mathf.FloorToInt(position.z));

        Chunk c = chunks[(int)(pos.x/Chunk.WIDTH), (int)(pos.z/Chunk.WIDTH)];
        
        pos.x -= (int)c.gameObject.transform.position.x;
        pos.y -= (int)c.gameObject.transform.position.y;
        pos.z -= (int)c.gameObject.transform.position.z;
        
        c.chunkBlocks[pos.x, pos.y, pos.z] = id;
        c.CreateMeshData();
        c.resetMesh = true;
    }

    /// <summary>
    /// Places/Destroys block in chunk thats not rendering
    /// </summary>
    public void EditDeadChunks(Vector3 position, byte id) 
    {
        Vector3Int pos = new Vector3Int(Mathf.FloorToInt(position.x), Mathf.FloorToInt(position.y), Mathf.FloorToInt(position.z));

        int x = (int)(pos.x / Chunk.WIDTH);
        int z = (int)(pos.z / Chunk.WIDTH);

        if (x >= MaxChunks || x <= 0 || z >= MaxChunks || z <= 0) return;

        Chunk c = deadChunks[x, z];

        pos.x -= (int)c.gameObject.transform.position.x;
        pos.y -= (int)c.gameObject.transform.position.y;
        pos.z -= (int)c.gameObject.transform.position.z;

        c.chunkBlocks[pos.x, pos.y, pos.z] = id;
    }

    private void OnApplicationQuit()
    {
        thread.Abort();
    }

    private void OnDestroy()
    {
        thread.Abort();
    }
}