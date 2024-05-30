using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplitAction : MonoBehaviour
{
    private SphereCollider coll;
    [Header("属性")]
    public bool isTouch;

    private void Start()
    {
        coll = GetComponent<SphereCollider>();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.transform.CompareTag("Player"))
        {
            isTouch = true;
        }
    }
}
