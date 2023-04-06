using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : AStarPathFinding
{
    //如果看到玩家，就A*寻路到玩家位置
    //如果没看到玩家，就随机巡逻
    //如果看到玩家的路径，顺着路径走，遍历整个路径
    public EnumDefinition.EnemyVisionState state = EnumDefinition.EnemyVisionState.SeeNothing;
    public List<GameObject> routeBeSeen;
    public EnemyVision enemyVision;
    public override void Start()
    {
        base.Start();
        enemyVision = GetComponent<EnemyVision>();
    }
    void Update()
    {
        state = enemyVision.state;
        routeBeSeen = enemyVision.routeBeSeen;
        if (state == EnumDefinition.EnemyVisionState.PlayerBeSeen)
        {
            Debug.Log("Player Be Seen");
        }
        else if (state == EnumDefinition.EnemyVisionState.PlayerRouteBeSeen)
        {
            Debug.Log("Player Route Be Seen");
        }
        else if (state == EnumDefinition.EnemyVisionState.SeeNothing)
        {
            Debug.Log("See Nothing");
        }
    }
}
    // 
    //public Vector2Int start = Vector2Int.zero, end = Vector2Int.zero;
    // public bool isFindPath = false;
    // GameObject[,] floorGrid = null;
    // List<Vector2Int> path = new List<Vector2Int>();
    // #region Debug
    // public bool isDebug = false;
    // public bool isLog = false;
    // public Material common, main, usual;
    // #endregion

    // public override void Start()
    // {
    //     base.Start();
    //     floorGrid = ReadMapGrid.instance.floorGrid;
    // }
    // void Update()
    // {
    //     if(isFindPath)
    //     {
    //         path = FindPath(start, end);
    //         isFindPath = false;
    //     }
    //     if(isDebug)
    //     {
    //         ClearMarkedCell();
    //         DrawMarkedCell(path);
    //     }
    // }

    // #region Debug
    // void DrawMarkedCell(List<Vector2Int> list)
    // {
    //     for (int i = 0; i < list.Count; i++)
    //     {
    //         floorGrid[list[i].x, list[i].y].GetComponent<Renderer>().material = main;
    //     }
    // }
    // void ClearMarkedCell()
    // {
    //     for (int i = 0; i < floorGrid.GetLength(0); i++)
    //     {
    //         for (int j = 0; j < floorGrid.GetLength(1); j++)
    //         {
    //             floorGrid[i, j].GetComponent<Renderer>().material = usual;
    //         }
    //     }
    // }
    // #endregion