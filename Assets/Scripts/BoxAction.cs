using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxAction : MonoBehaviour
{
    public PhysicMaterial massEqual;
    public PhysicMaterial massSmall;

    private Rigidbody rig;
    private BoxCollider coll;

    private void Awake()
    {
        rig = GetComponent<Rigidbody>();
        coll = GetComponent<BoxCollider>();
    }

    private void Update()
    {
        //根据大小来确定质量
        rig.mass = transform.localScale.x;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.transform.CompareTag("Player") || other.transform.CompareTag("PlayerSplit"))
        {
            if (other.transform.localScale.x >= transform.localScale.x)
            {
                coll.material = massEqual;
            }
            else
            {
                coll.material = massSmall;
            }
        }
    }
}
