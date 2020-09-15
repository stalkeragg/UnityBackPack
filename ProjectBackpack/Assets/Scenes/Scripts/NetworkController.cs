using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ItemPropertyDTO : ItemProperty
{
    private string ActionType;

    public ItemPropertyDTO(ItemProperty baseItemProperty, string actionType)
    {
        this.ActionType = actionType;

        this.Identifier = baseItemProperty.Identifier;
        this.Type = baseItemProperty.Type;
        this.Weight = baseItemProperty.Weight;
    }
}

public class NetworkController : MonoBehaviour
{
    public string ServerAddress;
    public string AuthCode;

    private List<GameObject> gameBackPacks;

    void Start()
    {
        gameBackPacks = new List<GameObject>();
        gameBackPacks.AddRange(GameObject.FindGameObjectsWithTag("Backpack"));

        foreach (GameObject obj in gameBackPacks)
        {
            var logic = obj.GetComponent<BackpackLogic>();
            if (logic != null)
            {
                logic.OnGameItemAttached += OnAttachItem;
                logic.OnGameItemDetached += OnDetachItem;
            }
        }
    }

    private IEnumerator SendActionRequest(GameObject obj, string actionType)
    {
        var itemPoperty = obj.GetComponent<ItemProperty>();
        if (itemPoperty != null)
        {
            // Create data
            var itemDTO = new ItemPropertyDTO(itemPoperty, actionType);
            var jsonSerializeData = itemDTO.GetSerializeData();

            // Send request

            UnityWebRequest www = UnityWebRequest.Post(ServerAddress, jsonSerializeData);
            www.SetRequestHeader("auth", AuthCode);

            yield return www.Send();

            if (www.isError)
            {
                Debug.Log(actionType + " Error Answer from Server: " + www.error);
            }
            else
            {
                Debug.Log(actionType + " Success Answer from Server: " + www.downloadHandler.text);
            }
        }
    }

    private void OnAttachItem(GameObject item)
    {
        StartCoroutine(SendActionRequest(item, "Attach"));
    }

    private void OnDetachItem(GameObject item)
    {
        StartCoroutine(SendActionRequest(item, "Detch"));
    }

}
