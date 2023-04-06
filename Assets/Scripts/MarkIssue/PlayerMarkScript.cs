using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMarkScript : MarkBase
{
    public Vector2Int currentPosition = Vector2Int.zero, lastPosition = Vector2Int.zero;
    public bool isWind = false;
    #region Debug
    public bool isDebug = false;
    public bool isLog = false;
    public Material common, main, usual;
    #endregion
    public override void Start()
    {
        base.Start();
        currentPosition = new Vector2Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.z));
        lastPosition = currentPosition;
        InitMark(currentPosition);
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            if(lastSplitMark != null)
            {
                transform.position = new Vector3(lastSplitMark.position.x, transform.position.y, lastSplitMark.position.y);
                currentPosition = lastSplitMark.position;
            }
        }
        currentPosition = new Vector2Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.z));
        if (currentPosition != lastPosition)
        {
            lastPosition = currentPosition;
            AddMark(currentPosition);
        }
        if(isWind)
        {
            isWind = false;
            ClearAllMark(currentPosition);
        }
        if(isDebug)
        {
            ClearMarkedCell();
            DrawMarkedCell(initialMark, true);
        }
        if(isLog)
        {
            isLog = false;
            if(lastSplitMark != null)
            {
                Debug.Log("lastSplitMark: " + lastSplitMark.position);
            }
            else
            {
                Debug.Log("lastSplitMark: null");
            }
        }
    }
    #region Debug
    void DrawMarkedCell(PlayerMark playerMark, bool isMain)
    {
        playerMark.mark.GetComponent<Renderer>().material = isMain ? main : common;
        if (playerMark.main != null)
        {
            DrawMarkedCell(playerMark.main, isMain);
        }
        for (int i = 0; i < playerMark.subMarks.Count; i++)
        {
            DrawMarkedCell(playerMark.subMarks[i], false);
        }
    }
    void ClearMarkedCell()
    {
        for (int i = 0; i < floorGrid.GetLength(0); i++)
        {
            for (int j = 0; j < floorGrid.GetLength(1); j++)
            {
                floorGrid[i, j].GetComponent<Renderer>().material = usual;
            }
        }
    }
    #endregion
}
