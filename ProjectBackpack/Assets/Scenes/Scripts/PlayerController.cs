using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Camera playerCamera;

    public InventoryController inventoryController;

    [HideInInspector]
    public bool BackPackIsPiked = false;

    private GameObject pickedPlayerItem = null;

    [HideInInspector]
    public event Action<GameObject, string> OnGameItemTryAttache;

    void Start()
    {
        playerCamera = Camera.main;
    }

    void Update()
    {
        // Detect backup using and send picked item info

        if (Input.GetMouseButtonDown(0))
        {
            Ray mouseVector = playerCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitObject;

            if (Physics.Raycast(mouseVector, out hitObject, 25, LayerMask.GetMask("Backpack")))
            {
                this.BackPackIsPiked = true;
            }
            else
            {
                this.BackPackIsPiked = false;
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            this.BackPackIsPiked = false;

            Ray mouseVector = playerCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitObject;

            if (Physics.Raycast(mouseVector, out hitObject, 25, LayerMask.GetMask("Backpack")))
            {
                if (pickedPlayerItem != null && OnGameItemTryAttache != null)
                {
                    OnGameItemTryAttache(pickedPlayerItem, hitObject.collider.name);
                    this.pickedPlayerItem = null;
                }
            }
            else
            {
                ResetPickedItem();
            }
        }
    }

    public void SetPickedItem(GameObject item)
    {
        this.pickedPlayerItem = item;
    }

    public void ResetPickedItem()
    {
        this.pickedPlayerItem = null;
    }
}
