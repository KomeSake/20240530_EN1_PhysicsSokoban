using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerSplit : MonoBehaviour
{
    public GameObject splitPrefab;
    private HashSet<Transform> splitObjs;

    public float splitMoveToPlayerSpeed;

    //private BoxCollider boxCollider;

    private void Start()
    {
        //boxCollider = GetComponent<BoxCollider>();
        splitObjs = new HashSet<Transform>();
    }
    public void BornSplit()
    {
        Vector3 bornPos = transform.position + new Vector3(0, 3, 0);
        GameObject obj = Instantiate(splitPrefab, bornPos, Quaternion.identity);
        splitObjs.Add(obj.transform);
    }

    public void MoveToPlayer()
    {
        if (splitObjs.Count != 0)
        {
            foreach (var it in splitObjs)
            {
                Vector3 dir = transform.position - it.transform.position;
                Rigidbody rig = it.GetComponent<Rigidbody>();
                rig.AddForce(dir * splitMoveToPlayerSpeed * Time.deltaTime);
            }
        }
    }
}
