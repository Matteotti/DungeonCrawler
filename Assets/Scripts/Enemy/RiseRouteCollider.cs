using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiseRouteCollider : MonoBehaviour
{
    public bool isUpdate = false;
    public float freshTime = 0.5f;
    private GameObject[,] floorGrid;
    private List<GameObject> markObjects;
    private PlayerMarkScript playerMarkScript;
    private PlayerMarkScript.PlayerMark initialMark;
    void Start()
    {
        InvokeRepeating("UpdateTrue", 0, freshTime);
        playerMarkScript = GameObject.Find("Player").GetComponent<PlayerMarkScript>();
        floorGrid = ReadMapGrid.instance.floorGrid;
    }
    void Update()
    {
        if(isUpdate)
        {
            initialMark = playerMarkScript.initialMark;
            markObjects = GetAllMarkObjects(initialMark, markObjects);
            UpdateColliderPosY(markObjects);
            isUpdate = false;
        }
    }
    List<GameObject> GetAllMarkObjects(PlayerMarkScript.PlayerMark mark, List<GameObject> markObjects)
    {
        if(mark == initialMark)
            markObjects = new List<GameObject>();
        if(mark.isSnow && mark.mark != null && !markObjects.Contains(mark.mark))
            markObjects.Add(mark.mark);
        if(mark.main != null)
            GetAllMarkObjects(mark.main, markObjects);
        if(mark.subMarks.Count > 0)
        {
            foreach(PlayerMarkScript.PlayerMark subMark in mark.subMarks)
            {
                GetAllMarkObjects(subMark, markObjects);
            }
        }
        return markObjects;
    }
    void UpdateColliderPosY(List<GameObject> markObjects)
    {
        for(int i = 0; i < floorGrid.GetLength(0); i++)
        {
            for(int j = 0; j < floorGrid.GetLength(1); j++)
            {
                if(markObjects.Contains(floorGrid[i, j]))
                {
                    floorGrid[i, j].GetComponent<BoxCollider>().center = new Vector3(0, 1f, 0);
                }
                else
                {
                    floorGrid[i, j].GetComponent<BoxCollider>().center = new Vector3(0, 0, 0);
                }
            }
        }
    }
    void UpdateTrue()
    {
        isUpdate = true;
    }
}
