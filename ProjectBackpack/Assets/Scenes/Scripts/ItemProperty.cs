using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ItemProperty : MonoBehaviour
{
    [SerializeField]
    public string Identifier;

    [SerializeField]
    public string Type;

    [SerializeField]
    public float Weight;

    public string GetSerializeData()
    {
        return JsonUtility.ToJson(this);
    }
}
