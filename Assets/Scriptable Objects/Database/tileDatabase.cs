using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using RotaryHeart.Lib.SerializableDictionary;

[System.Serializable]
public class tileDatabaseDict : SerializableDictionaryBase<int, baseTile> { }


[CreateAssetMenu(fileName = "tileDatabase", menuName = "Tiles/Database")]
public class tileDatabase : ScriptableObject
{
    public tileDatabaseDict tiles;
}