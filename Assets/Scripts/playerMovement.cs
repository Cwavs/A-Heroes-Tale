using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovement : MonoBehaviour
{
    public tileDatabase tDB;
    public Camera cam;
    List<Tile> tiles;

    // Start is called before the first frame update
    void Start()
    {
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Grid grid = GameObject.FindGameObjectWithTag("GameController").GetComponent<hexagonPerlin>().grid;

            List<Tile> oldtiles = tiles;
            tiles = grid.TilesInRange(transform.GetComponentInParent<Tile>(), 3);

            //foreach (var item in oldtiles)
            //{
            //    try
            //    {
            //        item.GetComponentInChildren<tileHandeler>().hexTopper.GetComponent<MeshRenderer>().materials[1].color = new Color(140f / 255, 140f / 255, 140f / 255);
            //    }
            //    catch
            //    {
            //        item.GetComponentInChildren<tileHandeler>().hexTopper.GetComponentInChildren<MeshRenderer>().materials[1].color = new Color(140f / 255, 140f / 255, 140f / 255);
            //    }
            //}

            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;

            

            if (Physics.Raycast(ray, out hit) && hit.transform.tag == "Topper" && tiles.Contains(hit.transform.GetComponentInParent<Tile>()))
            {
                transform.parent = hit.transform;
                transform.localPosition = tDB.tiles[hit.transform.GetComponentInParent<tileHandeler>().buildingID].playerCoords;
            }

            //foreach (var item in tiles)
            //{
            //    try
            //    {
            //        item.GetComponentInChildren<tileHandeler>().hexTopper.GetComponent<MeshRenderer>().materials[1].color = new Color(161f / 255, 124f / 255, 45f / 255);
            //    }
            //    catch
            //    {
            //        item.GetComponentInChildren<tileHandeler>().hexTopper.GetComponentInChildren<MeshRenderer>().materials[1].color = new Color(161f / 255, 124f / 255, 45f / 255);
            //    }
            //}
        }
    }
}
