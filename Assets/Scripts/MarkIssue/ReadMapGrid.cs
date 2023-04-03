using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadMapGrid : MonoBehaviour
{
    public int maxGridX, maxGridZ;
    public GameObject[,] floorGrid, wallGrid;
    public GameObject floorParent, wallParent;

    private void Start()
    {
        floorGrid = new GameObject[maxGridX, maxGridZ];
        wallGrid = new GameObject[maxGridX, maxGridZ];
        for(int i = 0;i < floorParent.transform.childCount; i++)
        {
            GameObject floor = floorParent.transform.GetChild(i).gameObject;
            floorGrid[(int)floor.transform.position.x, (int)floor.transform.position.z] = floor;
        }
        for (int i = 0; i < wallParent.transform.childCount; i++)
        {
            GameObject wall = wallParent.transform.GetChild(i).gameObject;
            wallGrid[(int)wall.transform.position.x, (int)wall.transform.position.z] = wall;
        }
    }
}
