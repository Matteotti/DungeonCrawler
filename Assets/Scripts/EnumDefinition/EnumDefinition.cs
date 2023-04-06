using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnumDefinition : MonoBehaviour
{
    public enum CameraFacingDirection
    {
        Forward,
        Left,
        Backward,
        Right,
    }
    public enum MapGridType
    {
        Floor,
        Wall,
        Ceiling,
    }
    public enum EnemyVisionState
    {
        PlayerBeSeen,
        PlayerRouteBeSeen,
        SeeNothing,
    }
}
