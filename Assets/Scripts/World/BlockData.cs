using UnityEngine;

public static class BlockData
{
    public static readonly int NumberOfTextures = 4;

    public static float GetBlockTextureSize()
    {
        return 1f / NumberOfTextures;
    }

    public static readonly Vector3[] blockVertices = new Vector3[8]
    {
        new Vector3(0.0f, 0.0f, 0.0f),
        new Vector3(1.0f, 0.0f, 0.0f),
        new Vector3(1.0f, 1.0f, 0.0f),
        new Vector3(0.0f, 1.0f, 0.0f),
        new Vector3(0.0f, 0.0f, 1.0f),
        new Vector3(1.0f, 0.0f, 1.0f),
        new Vector3(1.0f, 1.0f, 1.0f),
        new Vector3(0.0f, 1.0f, 1.0f)
    };

    public static readonly Vector3[] faceChecks = new Vector3[6]
    {
        new Vector3(0.0f, 0.0f, -1.0f),
        new Vector3(0.0f, 0.0f, 1.0f),
        new Vector3(0.0f, 1.0f, 0.0f),
        new Vector3(0.0f, -1.0f, 0.0f),
        new Vector3(-1.0f, 0.0f, 0.0f),
        new Vector3(1.0f, 0.0f, 0.0f)
    };

    public static readonly int[,] blockTriangles = new int[6, 4]
    {
        { 0, 3, 1, 2 }, // BACK
        { 5, 6, 4, 7 }, // FRONT
        { 3, 7, 2, 6 }, // TOP
        { 1, 5, 0, 4 }, // BOTTOM
        { 4, 7, 0, 3 }, // LEFT
        { 1, 2, 5, 6 }  // RIGHT
    };

    public static readonly Vector2[] blockUvs = new Vector2[4]
    {
        new Vector2(0.0f, 0.0f),
        new Vector2(0.0f, 1.0f),
        new Vector2(1.0f, 0.0f),
        new Vector2(1.0f, 1.0f)
    };
}