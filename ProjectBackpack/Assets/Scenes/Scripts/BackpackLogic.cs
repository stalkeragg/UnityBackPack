using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackpackLogic : MonoBehaviour
{
    private PlayerController playerController = null;

    private List<GameObject> backPackItems;

    private GameObject gunSpawnPoint;

    private GameObject bottleSpawnPoint;

    private GameObject kniweSpawnPoint;

    [HideInInspector]
    public event Action<GameObject> OnGameItemAttached;

    [HideInInspector]
    public event Action<GameObject> OnGameItemDetached;

    void Start()
    {
        playerController = (PlayerController)Camera.main.GetComponent("PlayerController");
        playerController.OnGameItemTryAttache += OnItemTryAttache;

        backPackItems = new List<GameObject>();

        gunSpawnPoint = GameObject.Find("GunSpawnPoint");
        bottleSpawnPoint = GameObject.Find("BottleSpawnPoint");
        kniweSpawnPoint = GameObject.Find("KnifeSpawnPoint");
    }

    private void OnItemTryAttache(GameObject item, string attachingObject)
    {
        if (item != null && attachingObject == gameObject.name)
        {
            AddItem(item);
        }
    }

    private void OnItemDetached(GameObject item)
    {
        backPackItems.Remove(item);

        if (OnGameItemDetached != null)
        {
            OnGameItemDetached(item);
        }
    }

    private void AddItem(GameObject obj)
    {
        var itemController = obj.GetComponent<ItemController>();
        itemController.OnItemDragDetach += OnItemDetached;
        backPackItems.Add(obj);

        var itemProperty = obj.GetComponent<ItemProperty>();

        if (itemProperty.Type == "gun")
        {
            itemController.AttachVisualItem(gunSpawnPoint.transform);
        }
        else if (itemProperty.Type == "bottle")
        {
            itemController.AttachVisualItem(bottleSpawnPoint.transform);
        }
        else if (itemProperty.Type == "knife")
        {
            itemController.AttachVisualItem(kniweSpawnPoint.transform);
        }
        else
        {
            itemController.AttachItemHiden();
        }

        if (OnGameItemAttached != null)
        {
            OnGameItemAttached(obj);
        }
    }

    private void OnMouseUp()
    {
        playerController.inventoryController.ResetDisplayItems();
    }

    private void OnMouseDown()
    {
        playerController.inventoryController.DisplayItems(backPackItems);
    }

    private void OnMouseExit()
    {
        playerController.inventoryController.ResetDisplayItems();
    }
}
