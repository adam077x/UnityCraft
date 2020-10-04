using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Structures
{
    /*
        Generates some basic structures like trees etc...
        Probably will be rewritten.    
    */

    public static void PlaceTree(int x, int z, float height) 
    {
        World world = World.instance;

        world.EditDeadChunks(new Vector3(x, height + 1, z), 6);
        world.EditDeadChunks(new Vector3(x, height + 2, z), 6);
        world.EditDeadChunks(new Vector3(x, height + 3, z), 6);
        world.EditDeadChunks(new Vector3(x, height + 4, z), 6);

        world.EditDeadChunks(new Vector3(x, height + 5, z), 7);

        world.EditDeadChunks(new Vector3(x + 1, height + 4, z), 7);
        world.EditDeadChunks(new Vector3(x - 1, height + 4, z), 7);

        world.EditDeadChunks(new Vector3(x + 1, height + 3, z), 7);
        world.EditDeadChunks(new Vector3(x -1, height + 3, z), 7);

        world.EditDeadChunks(new Vector3(x, height + 3, z + 1), 7);
        world.EditDeadChunks(new Vector3(x, height + 3, z - 1), 7);

        world.EditDeadChunks(new Vector3(x, height + 4, z - 1), 7);
        world.EditDeadChunks(new Vector3(x, height + 4, z + 1), 7);
    }
}