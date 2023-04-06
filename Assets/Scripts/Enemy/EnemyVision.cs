using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVision : MonoBehaviour
{
    public float freshTime = 0.5f;
    public float range = 10f;
    public int rayCount = 10;
    public LayerMask layerMask = 1 << 3 | 1 << 6 | 1 << 7;//3:Player, 6:PlayerRoute, 7:Block
    public EnumDefinition.EnemyVisionState state = EnumDefinition.EnemyVisionState.SeeNothing;
    public List<GameObject> routeBeSeen;
    void Start()
    {
        StartCoroutine(Check());
    }
    // void Update()
    // {
    //     if (state == EnumDefinition.EnemyVisionState.PlayerBeSeen)
    //     {
    //         Debug.Log("Player Be Seen");
    //     }
    //     else if (state == EnumDefinition.EnemyVisionState.PlayerRouteBeSeen)
    //     {
    //         Debug.Log("Player Route Be Seen");
    //     }
    //     else if (state == EnumDefinition.EnemyVisionState.SeeNothing)
    //     {
    //         Debug.Log("See Nothing");
    //     }
    // }
    IEnumerator Check()
    {
        yield return new WaitForSeconds(freshTime);
        RayCheckRoute();
        StartCoroutine(Check());
    }
    void RayCheckRoute()
    {
        routeBeSeen = new List<GameObject>();
        Vector3 direction = Vector3.forward;
        float angle = 360f / rayCount;
        state = EnumDefinition.EnemyVisionState.SeeNothing;
        for (int i = 0; i < rayCount; i++)
        {
            Vector3 rayDirection = Quaternion.AngleAxis(angle * i, Vector3.up) * direction;
            Ray ray = new Ray(transform.position, rayDirection);
            RaycastHit[] hit;
            hit = Physics.RaycastAll(ray, range, layerMask);
            Array.Sort(hit, (x, y) => x.distance.CompareTo(y.distance));
            foreach (RaycastHit h in hit)
            {
                if (h.collider.gameObject.layer == 6)
                {
                    if (!routeBeSeen.Contains(h.collider.gameObject))
                    {
                        routeBeSeen.Add(h.collider.gameObject);
                    }
                    state = EnumDefinition.EnemyVisionState.PlayerRouteBeSeen;
                }
                else if (h.collider.gameObject.layer == 3)
                {
                    state = EnumDefinition.EnemyVisionState.PlayerBeSeen;
                    return;
                }
                else if (h.collider.gameObject.layer == 7)
                {
                    break;
                }
            }
            Debug.DrawRay(ray.origin, ray.direction * range, Color.red, 1);
        }
    }
}