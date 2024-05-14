using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "New Map Piece", menuName ="Custom/Map Piece")]
public class BoardPieceData : ScriptableObject
{
    public string ID;
    public GameObject Prefab;

    private void OnValidate()
    {
        ID = name;
    }
}