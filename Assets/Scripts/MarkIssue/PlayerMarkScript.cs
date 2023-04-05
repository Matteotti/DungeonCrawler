using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMarkScript : MarkBase
{
    public Vector2Int currentPosition = Vector2Int.zero, lastPosition = Vector2Int.zero;
    public bool isWind = false;
    #region Debug
    public bool isDebug = false;
    public Material common, main;
    #endregion
    void Start()
    {
        currentPosition = new Vector2Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.z));
        lastPosition = currentPosition;
        InitMark(currentPosition);
    }
    void Update()
    {
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
            DrawMarkedCell(initialMark, true);
        }
    }
    #region Debug
    void DrawMarkedCell(PlayerMark playerMark, bool isMain)
    {
        playerMark.mark.GetComponent<Renderer>().material = isMain ? main : common;
        if (playerMark.main != null)
        {
            DrawMarkedCell(playerMark.main, true);
        }
        for (int i = 0; i < playerMark.subMarks.Count; i++)
        {
            DrawMarkedCell(playerMark.subMarks[i], false);
        }
    }
    #endregion
}
