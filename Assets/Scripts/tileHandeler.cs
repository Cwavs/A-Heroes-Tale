using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tileHandeler : MonoBehaviour
{

    public GameObject hexTopper;
    public GameObject hexBase;

    public OffsetIndex hexCoords;

    public int buildingID;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void createBuilding(tileDatabase tiles, Mesh mesh, Vector2 gridSize, float scale, Vector3 org, float heightScale)
    {

        try
        {
            Destroy(hexTopper);
        }
        catch
        {
            print("no hex topper");
        }

        Vector3 hexPos = transform.position;

        float xCoord = org.x + hexPos.x / gridSize.x * scale;
        float yCoord = org.y + hexPos.z / gridSize.y * scale;

        float perlin = Mathf.PerlinNoise(xCoord, yCoord) * heightScale;

        if (buildingID <= tiles.tiles.Count - 1)
        {
            hexTopper = Instantiate(tiles.tiles[buildingID].topperHex, transform);
        }
        else
        {
            buildingID = 0;
            hexTopper = Instantiate(tiles.tiles[buildingID].topperHex, transform);
        }

        hexTopper.transform.Translate(0, perlin, 0);

        print(buildingID);

        MeshCollider col = hexTopper.AddComponent<MeshCollider>();
        col.sharedMesh = mesh;
        hexTopper.tag = "Topper";
    }
}
