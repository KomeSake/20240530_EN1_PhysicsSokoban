using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorAction : MonoBehaviour
{
    public GameObject door;
    public float doorSpeed;

    private void DoorOpen()
    {
        if (door.transform.position.x < 10.5f)
            door.transform.position += new Vector3(doorSpeed * Time.deltaTime, 0, 0);
    }
    private void DoorClose()
    {
        if (door.transform.position.x > 8.5f)
            door.transform.position -= new Vector3(doorSpeed * Time.deltaTime, 0, 0);
    }

    private void OnEnable()
    {
        ScenesManager.OnClearLevel += DoorOpen;
        ScenesManager.OnNoClearLevel += DoorClose;
    }
    private void OnDisable()
    {
        ScenesManager.OnClearLevel -= DoorOpen;
        ScenesManager.OnNoClearLevel -= DoorClose;

    }
}
