using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerSplit : MonoBehaviour
{
    public GameObject splitPrefab;
    private HashSet<Transform> splitObjs = new();
    private HashSet<Transform> objectToRemove = new();

    [Header("属性")]
    public float bornToSmall;
    public float comboToBig;
    public float smallMax;
    public float splitMoveToPlayerSpeed;

    //private BoxCollider boxCollider;

    private void Start()
    {
        //boxCollider = GetComponent<BoxCollider>();
    }
    public void BornSplit()
    {
        //分裂后自己要变小
        if (transform.localScale.x <= smallMax)
            return;
        transform.localScale -= new Vector3(bornToSmall, bornToSmall, bornToSmall);

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
                //吸引分裂的球往主球移动
                Vector3 dir = transform.position - it.transform.position;
                Rigidbody rig = it.GetComponent<Rigidbody>();
                rig.AddForce(splitMoveToPlayerSpeed * Time.deltaTime * dir);

                //触碰到的分裂的球要删除，并且自己变大
                SplitAction splitAction = it.GetComponent<SplitAction>();
                if (splitAction != null && splitAction.isTouch)
                {
                    objectToRemove.Add(it);
                    Destroy(it.gameObject);
                    transform.localScale += new Vector3(comboToBig, comboToBig, comboToBig);
                }
            }

            //把不要的元素清空
            foreach (var item in objectToRemove)
            {
                splitObjs.Remove(item);
            }
            objectToRemove.Clear();
        }
    }
}
