using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hexagonPerlin : MonoBehaviour
{
    public GameObject Player;

    public float xOrg;
    public float yOrg;
    public float scale = 50f;
    public float heightScale = 10f;

    public float buildingScale = 200f;
    public float buildingOffset;

    public tileDatabase tDB;

    public baseTile tileObject;

    float oldxOrg;
    float oldyOrg;
    float oldScale;

    public Gradient grad = new Gradient();
    public GradientColorKey[] colourKey;
    public GradientAlphaKey[] alphaKey;

    public Gradient buildingGrad = new Gradient();
    public GradientColorKey[] buildingColourKey;
    public GradientAlphaKey[] buildingAlphaKey;

    Vector2 gridSize;

    public Grid grid;

    List<GameObject> myTiles;

    public List<Material> hexMats;
    public Material mat;

    public Dictionary<string, Tile> tiles;

    public Mesh mesh;

    public Vector2 per;

    // Start is called before the first frame update
    void Start()
    {
        colourKey = new GradientColorKey[3];
        alphaKey = new GradientAlphaKey[1];

        alphaKey[0].alpha = 1.0f;
        alphaKey[0].time = 0.0f;
        colourKey[0].color = Color.red;
        colourKey[0].time = 0.2f;
        colourKey[1].color = Color.green;
        colourKey[1].time = 0.5f;
        colourKey[2].color = Color.blue;
        colourKey[2].time = 1.0f;

        grad.mode = GradientMode.Blend;

        grad.SetKeys(colourKey, alphaKey);

        buildingColourKey = new GradientColorKey[3];
        buildingAlphaKey = new GradientAlphaKey[1];

        buildingAlphaKey[0].alpha = 1.0f;
        buildingAlphaKey[0].time = 0.0f;
        buildingColourKey[0].color = Color.white;
        buildingColourKey[0].time = 0.3f;
        buildingColourKey[1].color = Color.black;
        buildingColourKey[1].time = 1.0f;
        buildingColourKey[2].color = Color.gray;
        buildingColourKey[2].time = 0.2f;

        buildingGrad.mode = GradientMode.Fixed;

        buildingGrad.SetKeys(buildingColourKey, buildingAlphaKey);


        grid = GetComponent<Grid>();

        gridSize = new Vector2(grid.mapWidth, grid.mapHeight);

        tiles = grid.Tiles;

        myTiles = new List<GameObject>();

        generateMap();
        generateBuilding();

        Player = GameObject.FindGameObjectWithTag("Player");
        Player.transform.parent = myTiles[0].transform;
        Player.transform.localPosition = new Vector3(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (buildingOffset - oldxOrg != 0 || buildingOffset - oldyOrg != 0 || buildingScale - oldScale != 0)
        {
            generateBuilding();
        }
        oldxOrg = buildingOffset;
        oldyOrg = buildingOffset;
        oldScale = buildingScale;
    }

    void generateMap()
    {

        int i = 0;

        foreach (var item in tiles)
        {
            Transform hexTransform = item.Value.transform;
            Vector3 hexPos = hexTransform.position;

            float xCoord = xOrg + hexPos.x / gridSize.x * scale;
            float yCoord = yOrg + hexPos.z / gridSize.y * scale;

            float perlin = Mathf.PerlinNoise(xCoord, yCoord) * heightScale;
            per = new Vector2(perlin, 0);

            hexMats.Add(new Material(mat));
            //hexMats[i].color = grad.Evaluate(perlin);

            myTiles.Add(Instantiate(new GameObject(), new Vector3(hexPos.x, 0, hexPos.z), new Quaternion()));

            myTiles[i].transform.parent = hexTransform;

            tileHandeler handel = myTiles[i].AddComponent<tileHandeler>();

            handel.hexCoords = Tile.CubeToEvenPointy(item.Value.index);

            handel.hexBase = Instantiate(tileObject.baseHex, myTiles[i].transform);

            handel.hexBase.transform.localScale = new Vector3(1, perlin, 1);

            handel.hexBase.GetComponent<Renderer>().material = hexMats[i];

            i++;
        }
    }

    void generateBuilding()
    {

        int i = 0;

        foreach (var item in tiles)
        {
            Transform hexTransform = item.Value.transform;
            Vector3 hexPos = hexTransform.position;

            float xCoord = xOrg + hexPos.x / gridSize.x * buildingScale + buildingOffset;
            float yCoord = yOrg + hexPos.z / gridSize.y * buildingScale + buildingOffset;

            float buildingPerlin = Mathf.PerlinNoise(xCoord, yCoord);

            xCoord = xOrg + hexPos.x / gridSize.x * scale;
            yCoord = yOrg + hexPos.z / gridSize.y * scale;

            float perlin = Mathf.PerlinNoise(xCoord, yCoord) * heightScale;

            hexMats[i] = new Material(mat);
            Color col = buildingGrad.Evaluate(buildingPerlin);

            tileHandeler handel = myTiles[i].GetComponent<tileHandeler>();

            if(col.grayscale == 0f)
            {
                handel.buildingID = 0;
            }
            else if(col.grayscale == 0.5f)
            { 
                handel.buildingID = 1;
            }else if(col.grayscale == 1)
            {
                handel.buildingID = 2;
            }
            


            i++;
        }

        //i = 0;
        //foreach (var item in tiles)
        //{
        //    tileHandeler handel = myTiles[i].GetComponent<tileHandeler>();

        //    if (handel.buildingID != 0)
        //    {
        //        List<Tile> tilesClose = grid.TilesInRange(item.Value, tDB.tiles[handel.buildingID].closeBuildingRange);
        //        List<Tile> tilesFar = grid.TilesInRange(item.Value, tDB.tiles[handel.buildingID].buildingRange);

        //        foreach (var x in tilesClose)
        //        {
        //            tileHandeler handel2 = x.GetComponentInChildren<tileHandeler>();
        //            handel2.buildingID = 0;
        //        }
        //        foreach (var x in tilesFar)
        //        {
        //            tileHandeler handel2 = x.GetComponentInChildren<tileHandeler>();
        //            handel2.buildingID++;
        //            if(handel2.buildingID > tDB.tiles.Count-1)
        //            {
        //                handel2.buildingID = 0;
        //            }
        //        }
        //    }
        //    i++;
        //}

        i = 0;
        foreach (var item in tiles)
        {
            tileHandeler handel = myTiles[i].GetComponent<tileHandeler>();
            handel.createBuilding(tDB, mesh, gridSize, scale, new Vector2(xOrg, yOrg), heightScale);
            i++;
        }
    }
}
