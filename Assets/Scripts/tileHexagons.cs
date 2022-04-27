using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tileHexagons : MonoBehaviour
{

    public GameObject cube;

    public GameObject hex;

    public int mapSize = 6;

    public List<GameObject> hexes;

    // Start is called before the first frame update
    void Start()
    {
        hexes = new List<GameObject>();

        Mesh mesh = hex.GetComponent<MeshFilter>().sharedMesh;

        Bounds bounds = mesh.bounds;

        int x = 0;
        while (x < mapSize/2)
        {
            int y = 0;
            while (y < mapSize/2)
            {
                float w = Mathf.Sqrt(3) * bounds.size.z + x;
                float h = 2 * bounds.size.z + y;
                if(x%2 == 0)
                {
                    hexes.Add(Instantiate(hex, new Vector3(h + bounds.size.x, 0, w - bounds.size.z), new Quaternion(0, 0, 0, 0)));
                }
                else
                {
                    hexes.Add(Instantiate(hex, new Vector3(h + bounds.size.x, 0, w - bounds.size.z), new Quaternion(0, 0, 0, 0)));
                }
                y++;
            }
            x++;
        }

        //foreach (GameObject baseTiles in hexes)
        //{
        //    baseTiles.transform.Translate(new Vector3(baseTiles.transform.position.x + bounds.size.x, 0, baseTiles.transform.position.z + bounds.size.z));
        //}
    }

    // Update is called once per frame
    void Update()
    {

    }
}
