using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorAction : MonoBehaviour
{
    public GameObject door;
    public float doorSpeed;

    public void DoorOpen()
    {
        transform.position += new Vector3(-doorSpeed * Time.deltaTime, 0, 0);
    }
}
