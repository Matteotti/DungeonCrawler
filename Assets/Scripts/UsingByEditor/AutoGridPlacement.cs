using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class AutoGridPlacement : MonoBehaviour
{
    public int maxGridX, maxGridZ;
    public bool isCreated = false, isDestroyed = true;
    public EnumDefinition.MapGridType gridType;
    void Update()
    {
        if (!isCreated)
        {
            isCreated = true;
            CreateGrid();
        }
        if (!isDestroyed)
        {
            isDestroyed = true;
            DestroyGrid();
        }
    }

    private void CreateGrid()
    {
        for(int i = 0;i < maxGridX * maxGridZ;i++)
        {
            GameObject floor = GameObject.CreatePrimitive(PrimitiveType.Cube);
            floor.transform.position = new Vector3(i % maxGridX, (float)gridType, i / maxGridX);
            floor.transform.parent = transform;
        }
    }
    private void DestroyGrid()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            DestroyImmediate(transform.GetChild(i).gameObject);
        }
    }
}
