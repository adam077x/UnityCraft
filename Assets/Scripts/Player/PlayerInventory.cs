using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventory : MonoBehaviour
{
    public Text currentBlockText;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            currentBlockText.text = "Dirt";
            PlayerCamera.currentBlock = 1;  
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            currentBlockText.text = "Cobblestone";
            PlayerCamera.currentBlock = 2;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            currentBlockText.text = "Stone";
            PlayerCamera.currentBlock = 3;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            currentBlockText.text = "Bedrock";
            PlayerCamera.currentBlock = 4;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            currentBlockText.text = "Grass";
            PlayerCamera.currentBlock = 5;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            currentBlockText.text = "Wood";
            PlayerCamera.currentBlock = 6;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            currentBlockText.text = "Leaves";
            PlayerCamera.currentBlock = 7;
        }
    }
}
