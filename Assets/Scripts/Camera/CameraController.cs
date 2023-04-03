using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float distanceUnit = 1f;
    public ReadMapGrid readMapGrid;
    public EnumDefinition.CameraFacingDirection direction = EnumDefinition.CameraFacingDirection.Forward;

    private void Start()
    {
        direction = EnumDefinition.CameraFacingDirection.Forward;
    }

    private void Update()
    {
        int a = Input.GetKeyDown(KeyCode.A) ? 1 : 0;
        int d = Input.GetKeyDown(KeyCode.D) ? 1 : 0;
        int w = Input.GetKeyDown(KeyCode.W) ? 1 : 0;
        int s = Input.GetKeyDown(KeyCode.S) ? 1 : 0;
        int horizontal = d - a;
        int vertical = w - s;
        switch(direction)
        {
            case EnumDefinition.CameraFacingDirection.Forward:
                if(readMapGrid.wallGrid[(int)(transform.position.x + horizontal), (int)(transform.position.z + vertical)] == null)
                {
                    transform.position += new Vector3(horizontal, 0, vertical) * distanceUnit;
                }
                break;
            case EnumDefinition.CameraFacingDirection.Right:
                if (readMapGrid.wallGrid[(int)(transform.position.x + vertical), (int)(transform.position.z - horizontal)] == null)
                {
                    transform.position += new Vector3(vertical, 0, -horizontal) * distanceUnit;
                }
                break;
            case EnumDefinition.CameraFacingDirection.Backward:
                if (readMapGrid.wallGrid[(int)(transform.position.x - horizontal), (int)(transform.position.z - vertical)] == null)
                {
                    transform.position += new Vector3(-horizontal, 0, -vertical) * distanceUnit;
                }
                break;
            case EnumDefinition.CameraFacingDirection.Left:
                if (readMapGrid.wallGrid[(int)(transform.position.x - vertical), (int)(transform.position.z + horizontal)] == null)
                {
                    transform.position += new Vector3(-vertical, 0, horizontal) * distanceUnit;
                }
                break;
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            transform.eulerAngles += new Vector3(0, -90, 0);
            direction += 1;
            if (direction > EnumDefinition.CameraFacingDirection.Right)
            {
                direction = EnumDefinition.CameraFacingDirection.Forward;
            }
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            transform.eulerAngles += new Vector3(0, 90, 0);
            direction -= 1;
            if (direction < EnumDefinition.CameraFacingDirection.Forward)
            {
                direction = EnumDefinition.CameraFacingDirection.Right;
            }
        }
    }
}
