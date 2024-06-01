using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorAction : MonoBehaviour
{
    public GameObject door;
    public float doorSpeed;

    public void DoorOpen()
    {
        if (door.transform.position.x < 10.5f)
            door.transform.position += new Vector3(doorSpeed * Time.deltaTime, 0, 0);
    }
    public void DoorClose()
    {
        if (door.transform.position.x > 8.5f)
            door.transform.position -= new Vector3(doorSpeed * Time.deltaTime, 0, 0);
    }
}
