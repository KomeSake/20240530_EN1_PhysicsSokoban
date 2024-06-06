using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSlowly : MonoBehaviour
{
    private void OnTriggerExit(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            //将角度和速度变小，防止乱飞
            Rigidbody rig = other.GetComponent<Rigidbody>();
            rig.velocity = Vector3.down;
            rig.angularVelocity = Vector3.zero;
        }
    }
}
