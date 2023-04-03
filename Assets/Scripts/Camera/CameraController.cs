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
        if (Input.GetKeyDown(KeyCode.W) && readMapGrid.floorGrid[(int)transform.position.x, (int)transform.position.z + 1] == null)
        {
            transform.position += Vector3.forward * distanceUnit;
        }
        if (Input.GetKeyDown(KeyCode.S) && readMapGrid.floorGrid[(int)transform.position.x, (int)transform.position.z - 1] == null)
        {
            transform.position += Vector3.back * distanceUnit;
        }
        if (Input.GetKeyDown(KeyCode.A) && readMapGrid.floorGrid[(int)transform.position.x - 1, (int)transform.position.z] == null)
        {
            transform.position += Vector3.left * distanceUnit;
        }
        if (Input.GetKeyDown(KeyCode.D) && readMapGrid.floorGrid[(int)transform.position.x + 1, (int)transform.position.z] == null)
        {
            transform.position += Vector3.right * distanceUnit;
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
