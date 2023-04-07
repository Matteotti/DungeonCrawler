using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : AStarPathFinding
{
    private GameObject[,] floorGrid = null;
    private int currentPathIndex;
    public Vector2Int start = Vector2Int.zero, end = Vector2Int.zero, currentDestination, lastDestination;
    public EnumDefinition.EnemyVisionState state = EnumDefinition.EnemyVisionState.SeeNothing;
    public EnumDefinition.EnemyMoveState moveState = EnumDefinition.EnemyMoveState.MoveRandomly;
    public List<GameObject> routeBeSeen;
    public List<Vector2Int> playerPath;
    public GameObject player;
    public EnemyVision enemyVision;
    public PlayerMarkScript.PlayerMark targetPlayerMark;
    public bool routeForward;
    public Dictionary<PlayerMarkScript.PlayerMark, int> playerMarkSubMarksIndex = new Dictionary<PlayerMarkScript.PlayerMark, int>();
    public float moveTimeGap = 1;
    public override void Start()
    {
        base.Start();
        floorGrid = ReadMapGrid.instance.floorGrid;
        enemyVision = GetComponent<EnemyVision>();
        InvokeRepeating(nameof(Move), 2, moveTimeGap);
    }
    void Update()
    {
        state = enemyVision.state;
        routeBeSeen = enemyVision.routeBeSeen;
        DetermineMoveState();
    }
    void DetermineMoveState()
    {
        if (state == EnumDefinition.EnemyVisionState.PlayerBeSeen)
        {
            moveState = EnumDefinition.EnemyMoveState.MoveToPlayer;
            currentDestination = new Vector2Int(Mathf.RoundToInt(player.transform.position.x), Mathf.RoundToInt(player.transform.position.z));
        }
        else if (moveState == EnumDefinition.EnemyMoveState.MoveToPlayer)
        {
            if (currentDestination == new Vector2Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.z)))
            {
                moveState = EnumDefinition.EnemyMoveState.MoveRandomly;
            }
        }
        else if (moveState == EnumDefinition.EnemyMoveState.MoveRandomly)
        {
            if (state == EnumDefinition.EnemyVisionState.PlayerRouteBeSeen)
            {
                moveState = EnumDefinition.EnemyMoveState.MoveToPlayerRoute;
                int index = Random.Range(0, routeBeSeen.Count);
                currentDestination = new Vector2Int(Mathf.RoundToInt(routeBeSeen[index].transform.position.x), Mathf.RoundToInt(routeBeSeen[index].transform.position.z));
            }
        }
        else if (moveState == EnumDefinition.EnemyMoveState.MoveToPlayerRoute)
        {
            if (currentDestination == new Vector2Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.z)))
            {
                moveState = EnumDefinition.EnemyMoveState.MoveAlongPlayerRoutePath;
                GetWalkingRoute();
            }
        }
        else if (moveState != EnumDefinition.EnemyMoveState.MoveAlongPlayerRoutePath)
        {
            if (state == EnumDefinition.EnemyVisionState.SeeNothing)
            {
                moveState = EnumDefinition.EnemyMoveState.MoveRandomly;
            }
        }
    }
    void GetDestinationPath()
    {
        if (currentDestination != lastDestination)
        {
            start = new Vector2Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.z));
            end = currentDestination;
            lastDestination = currentDestination;
            currentPathIndex = 0;
            playerPath = FindPath(start, end);
        }
    }
    void MoveToDestination()
    {
        if (currentPathIndex < playerPath.Count)
        {
            transform.position = new Vector3(playerPath[currentPathIndex].x, transform.position.y, playerPath[currentPathIndex].y);
            currentPathIndex++;
        }
    }
    void GetWalkingRoute()
    {
        PlayerMarkScript playerMarkScript = player.GetComponent<PlayerMarkScript>();
        PlayerMarkScript.PlayerMark latestMark = playerMarkScript.currentMark;
        while (latestMark.position != currentDestination)
        {
            latestMark = latestMark.lastMark;
        }
        targetPlayerMark = latestMark;
    }
    void WalkingAlongRoute()
    {
        if (targetPlayerMark == null)
            return;
        if (routeForward && targetPlayerMark.main != null)
        {
            if (targetPlayerMark.subMarks.Count > 0)
            {
                if (playerMarkSubMarksIndex.ContainsKey(targetPlayerMark) && playerMarkSubMarksIndex[targetPlayerMark] < targetPlayerMark.subMarks.Count)
                {
                    targetPlayerMark = targetPlayerMark.subMarks[playerMarkSubMarksIndex[targetPlayerMark]];
                    playerMarkSubMarksIndex[targetPlayerMark.lastMark] += 1;
                }
                else if (!playerMarkSubMarksIndex.ContainsKey(targetPlayerMark))
                {
                    targetPlayerMark = targetPlayerMark.main;
                    playerMarkSubMarksIndex.Add(targetPlayerMark.lastMark, 0);
                }
            }
            else
                targetPlayerMark = targetPlayerMark.main;
            transform.position = new Vector3(targetPlayerMark.position.x, transform.position.y, targetPlayerMark.position.y);
        }
        else if (routeForward && targetPlayerMark.main == null)
        {
            routeForward = false;
            targetPlayerMark = targetPlayerMark.lastMark;
            transform.position = new Vector3(targetPlayerMark.position.x, transform.position.y, targetPlayerMark.position.y);
        }
        else if (!routeForward && targetPlayerMark.lastMark != null)
        {
            if (targetPlayerMark.subMarks.Count > 0)
            {
                if (playerMarkSubMarksIndex.ContainsKey(targetPlayerMark) && playerMarkSubMarksIndex[targetPlayerMark] < targetPlayerMark.subMarks.Count)
                {
                    routeForward = true;
                    targetPlayerMark = targetPlayerMark.subMarks[playerMarkSubMarksIndex[targetPlayerMark]];
                    playerMarkSubMarksIndex[targetPlayerMark.lastMark] += 1;
                }
                else if (!playerMarkSubMarksIndex.ContainsKey(targetPlayerMark))
                {
                    state = EnumDefinition.EnemyVisionState.SeeNothing;
                    moveState = EnumDefinition.EnemyMoveState.MoveRandomly;
                }
            }
            else
                targetPlayerMark = targetPlayerMark.lastMark;
            targetPlayerMark = targetPlayerMark.lastMark;
            transform.position = new Vector3(targetPlayerMark.position.x, transform.position.y, targetPlayerMark.position.y);
        }
        else if (!routeForward && targetPlayerMark.lastMark == null)
        {
            routeForward = true;
            targetPlayerMark = targetPlayerMark.main;
            transform.position = new Vector3(targetPlayerMark.position.x, transform.position.y, targetPlayerMark.position.y);
        }
    }
    void RandomMove()
    {
        List<GameObject> walkableGridCount = new List<GameObject>();
        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                if ((i != 0 || j != 0) && i * j == 0 && wallGrid[(int)transform.position.x + i, (int)transform.position.z + j] == null)
                {
                    walkableGridCount.Add(floorGrid[(int)transform.position.x + i, (int)transform.position.z + j]);
                }
            }
        }
        int index = Random.Range(0, walkableGridCount.Count);
        transform.position = new Vector3(walkableGridCount[index].transform.position.x, transform.position.y, walkableGridCount[index].transform.position.z);
    }
    void Move()
    {
        switch (moveState)
        {
            case EnumDefinition.EnemyMoveState.MoveToPlayer:
                GetDestinationPath();
                MoveToDestination();
                break;
            case EnumDefinition.EnemyMoveState.MoveToPlayerRoute:
                GetDestinationPath();
                MoveToDestination();
                break;
            case EnumDefinition.EnemyMoveState.MoveAlongPlayerRoutePath:
                WalkingAlongRoute();
                break;
            case EnumDefinition.EnemyMoveState.MoveRandomly:
                RandomMove();
                break;
        }
    }
}