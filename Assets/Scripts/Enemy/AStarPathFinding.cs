using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarPathFinding : MonoBehaviour
{
    public class Distance
    {
        public int distance, distanceG, distanceN;
        public Vector2Int position;
    }
    public class Parent
    {
        public Vector2Int position;
        public Vector2Int parent;
    }
    public GameObject[,] wallGrid = null;

    public virtual void Start()
    {
        wallGrid = ReadMapGrid.instance.wallGrid;
    }
    public List<Vector2Int> FindPath(Vector2Int start, Vector2Int end)
    {
        List<Distance> openList = new List<Distance>();
        List<Vector2Int> closeList = new List<Vector2Int>();
        List<Parent> parentList = new List<Parent>();
        Insert(openList, start, ManhattonDistance(start, end), 0, ManhattonDistance(start, end));
        while(true)
        {
            if(openList.Count == 0)
            {
                return null;
            }
            else if(closeList.Contains(end))
            {
                List<Vector2Int> path = new List<Vector2Int>();
                path.Add(end);
                while(true)
                {
                    for(int i = 0; i < parentList.Count; i++)
                    {
                        if(parentList[i].position == path[path.Count - 1])
                        {
                            path.Add(parentList[i].parent);
                            break;
                        }
                    }
                    if(path[path.Count - 1] == start)
                    {
                        path.Reverse();
                        return path;
                    }
                }
            }
            Distance min = RemoveMinimum(openList);
            closeList.Add(min.position);
            for(int i = -1;i <= 1;i++)
            {
                for(int j = -1;j <= 1;j++)
                {
                    if(i == 0 && j == 0)
                    {
                        continue;
                    }
                    else if(i != 0 && j != 0)
                    {
                        continue;
                    }
                    else
                    {
                        Vector2Int position = new Vector2Int(min.position.x + i, min.position.y + j);
                        if(wallGrid[position.x, position.y] != null)
                        {
                            continue;
                        }
                        else if(closeList.Contains(position))
                        {
                            continue;
                        }
                        else
                        {
                            Distance temp = IsInList(openList, position);
                            if(temp == null)
                            {
                                Insert(openList, position, min.distanceG + 1 + ManhattonDistance(position, end), min.distanceG + 1, ManhattonDistance(position, end));
                                Parent parent = new Parent();
                                parent.position = position;
                                parent.parent = min.position;
                                parentList.Add(parent);
                            }
                            else
                            {
                                if(temp.distance > min.distanceG + 1 + ManhattonDistance(position, end))
                                {
                                    temp.distance = min.distanceG + 1 + ManhattonDistance(position, end);
                                    Parent parent = new Parent();
                                    parent.position = position;
                                    parent.parent = min.position;
                                    parentList.Add(parent);
                                }
                            }
                        }
                    }
                }
            }
        } 
    }
    int ManhattonDistance(Vector2Int start, Vector2Int end)
    {
        return Mathf.Abs(start.x - end.x) + Mathf.Abs(start.y - end.y);
    }
    Distance RemoveMinimum(List<Distance> list)
    {
        int min = list[0].distance;
        Vector2Int position = list[0].position;
        for(int i = 1; i < list.Count; i++)
        {
            if(list[i].distance < min)
            {
                min = list[i].distance;
                position = list[i].position;
            }
        }
        Distance temp = new Distance();
        temp = IsInList(list, position);
        list.Remove(temp);
        return temp;
    }
    void Insert(List<Distance> list, Vector2Int position, int distance, int distanceG, int distanceN)
    {
        Distance temp = new Distance();
        temp.distance = distance;
        temp.position = position;
        temp.distanceG = distanceG;
        temp.distanceN = distanceN;
        list.Add(temp);
    }
    Distance IsInList(List<Distance> list, Vector2Int position)
    {
        for(int i = 0; i < list.Count; i++)
        {
            if(list[i].position == position)
            {
                return list[i];
            }
        }
        return null;
    }
}
