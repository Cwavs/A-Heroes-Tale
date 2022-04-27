using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "baseTile", menuName = "Tiles/baseTile")]
public class baseTile : ScriptableObject
{
    public GameObject baseHex;
    public GameObject topperHex;

    public Vector3 playerCoords;

    public int buildingRange;
    public int closeBuildingRange;
}
