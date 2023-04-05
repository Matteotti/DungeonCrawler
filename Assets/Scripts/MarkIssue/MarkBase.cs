using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
public class MarkBase : MonoBehaviour
{
    public class PlayerMark
    {
        public bool isSnow = true;
        public Vector2Int position = Vector2Int.zero;
        public GameObject mark = null;
        public PlayerMark main = null;
        public PlayerMark lastMark = null;
        public List<PlayerMark> subMarks = new List<PlayerMark>();
    }
    public List<PlayerMark> playerMarks = new List<PlayerMark>();
    public PlayerMark lastSplitMark = null;
    public PlayerMark initialMark = null;
    private GameObject[,] floorGrid = ReadMapGrid.instance.floorGrid;
    private PlayerMark currentMark = new PlayerMark();
    public void InitMark(Vector2Int position)
    {
        currentMark.position = position;
        currentMark.mark = floorGrid[position.x, position.y];
        initialMark = currentMark;
        playerMarks.Add(currentMark);
    }
    public void AddMark(Vector2Int nextPosition)
    {
        if(currentMark.position == nextPosition)
        {
            return;
        }
        else if(currentMark.main.position == nextPosition)
        {
            currentMark = currentMark.main;
            return;
        }
        else if(currentMark.lastMark.position == nextPosition)
        {
            currentMark = currentMark.lastMark;
            return;
        }
        else
        {
            for(int i = 0;i < currentMark.subMarks.Count;i++)
            {
                if(currentMark.subMarks[i].position == nextPosition)
                {
                    currentMark = currentMark.subMarks[i];
                    PlayerMark temp1 = currentMark.subMarks[i];
                    currentMark.subMarks[i] = currentMark.main;
                    currentMark.main = temp1;
                    return;
                }
            }
            if(currentMark.main != null)
            {
                currentMark.subMarks.Add(currentMark.main);
                lastSplitMark = currentMark;
                UpdateMarkRoute();
            }
            PlayerMark temp2 = new PlayerMark();
            temp2.position = nextPosition;
            temp2.mark = floorGrid[nextPosition.x, nextPosition.y];
            temp2.lastMark = currentMark;
            currentMark.main = temp2;
            currentMark = temp2;
        }
    }
    /// <summary>
    /// Called After Player Arrive At LastSplitMark
    /// </summary>
    public void UpdateMarkRoute()
    {
        if(lastSplitMark == null)
        {
            return;
        }
        else
        {
            PlayerMark temp = lastSplitMark;
            while(temp != null)
            {
                if(temp.subMarks.Count > 0)
                {
                    temp.subMarks.RemoveAll(x => x == null);
                }
                temp = temp.lastMark;
            }
        }
    }
    public void ClearAllMark(Vector2Int position)
    {
        playerMarks = new List<PlayerMark>();
        InitMark(position);
    }
}
