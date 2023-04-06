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
    public GameObject[,] floorGrid = null;
    private PlayerMark currentMark = new PlayerMark();
    public virtual void Start()
    {
        floorGrid = ReadMapGrid.instance.floorGrid;
    }
    public void InitMark(Vector2Int position)
    {
        currentMark = new PlayerMark();
        currentMark.position = position;
        currentMark.mark = floorGrid[position.x, position.y];
        currentMark.isSnow = currentMark.mark.CompareTag("FloorWithSnow");
        initialMark = currentMark;
        playerMarks.Add(currentMark);
    }
    public void AddMark(Vector2Int nextPosition)
    {
        if (currentMark.position == nextPosition)
        {
            return;
        }
        else if (currentMark.main != null && currentMark.main.position == nextPosition)
        {
            currentMark = currentMark.main;
            return;
        }
        else if (currentMark.lastMark != null && currentMark.lastMark.position == nextPosition)
        {
            currentMark = currentMark.lastMark;
            return;
        }
        else
        {
            for (int i = 0; i < currentMark.subMarks.Count; i++)
            {
                if (currentMark.subMarks[i].position == nextPosition)
                {
                    PlayerMark temp1 = currentMark.subMarks[i];
                    currentMark.subMarks[i] = currentMark.main;
                    currentMark.main = temp1;
                    currentMark = currentMark.main;
                    return;
                }
            }
            if (currentMark.main != null)
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
            currentMark.isSnow = currentMark.mark.CompareTag("FloorWithSnow");
        }
    }
    public void UpdateMarkRoute()
    {
        if (lastSplitMark == null)
        {
            return;
        }
        else
        {
            PlayerMark temp = lastSplitMark.lastMark;
            temp.position = Vector2Int.zero;
            while (temp != null)
            {
                if (temp.subMarks.Count > 0 && temp.isSnow)
                {
                    temp.subMarks.RemoveAll(x => x != null);
                }
                temp = temp.lastMark;
            }
            temp = lastSplitMark.main;
            while (temp != null)
            {
                if (temp.subMarks.Count > 0 && temp.isSnow)
                {
                    temp.subMarks.RemoveAll(x => x != null);
                }
                temp = temp.main;
            }
        }
    }
    public void ClearAllMark(Vector2Int position)
    {
        playerMarks = new List<PlayerMark>();
        InitMark(position);
    }
}
