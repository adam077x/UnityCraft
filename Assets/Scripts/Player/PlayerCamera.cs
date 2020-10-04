using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public float mouseSensitivity;

    public Transform playerBody;
    private float xRotation = 0;

    private float step = 0.1f;
    private float reach = 7.0f;

    public static byte currentBlock = 1;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        //if (PlayerInventory.instance.opened) return;

        Look();

        PlaceDestroyBlock();
    }

    /// <summary>
    /// Player look based on mouse position
    /// </summary>
    private void Look() 
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90, 90);

        transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
        playerBody.Rotate(Vector3.up * mouseX);
    }

    /// <summary>
    /// Places or Destroys blocks
    /// </summary>
    private void PlaceDestroyBlock() 
    {
        RaycastHit hit;
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Input.GetButtonDown("Fire1"))
        {
            if (Physics.Raycast(ray, out hit, reach))
            {
                Vector3 pos = hit.point + transform.forward * 0.1f;
                Vector3Int posInt = new Vector3Int(Mathf.FloorToInt(pos.x), Mathf.FloorToInt(pos.y), Mathf.FloorToInt(pos.z));

                World.instance.EditBlock(posInt, 0);
            }
        }
        else if (Input.GetButtonDown("Fire2"))
        {
            if (Physics.Raycast(ray, out hit, reach))
            {
                Vector3 pos = hit.point - transform.forward * 0.1f;
                Vector3Int posInt = new Vector3Int(Mathf.FloorToInt(pos.x), Mathf.FloorToInt(pos.y), Mathf.FloorToInt(pos.z));

                World.instance.EditBlock(posInt, currentBlock);
            }
        }
    }
}
