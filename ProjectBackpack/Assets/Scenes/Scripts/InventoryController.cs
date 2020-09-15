using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryController : MonoBehaviour
{
    public Text slotPrefab;
    private PlayerController playerController;

    public void DisplayItems(List<GameObject> items)
    {
        foreach (GameObject item in items)
        {
            var guiObj = Instantiate<Text>(slotPrefab, gameObject.transform.position, gameObject.transform.rotation);
            guiObj.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            guiObj.transform.SetParent(gameObject.transform);

            guiObj.text = item.name;
        }

        gameObject.SetActive(true);
    }

    public void ResetDisplayItems()
    {
        gameObject.SetActive(false);
        foreach (Transform child in gameObject.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }

    void Start()
    {
        gameObject.SetActive(false);
        playerController = (PlayerController)Camera.main.GetComponent("PlayerController");
        playerController.inventoryController = this;
    }
}
