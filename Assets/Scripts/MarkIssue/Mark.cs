using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mark : MonoBehaviour
{
    public class PlayerMark
    {
        bool isSplited;
        int splitCount;
        PlayerMark main;
        List<PlayerMark> subMarks;
    }
}
