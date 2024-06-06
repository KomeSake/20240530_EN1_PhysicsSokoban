using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerSplit : MonoBehaviour
{
    public static event Action<PlayerParticle.ParitcleName> OnComboSplit;
    public GameObject splitPrefab;
    private Rigidbody rig;
    private HashSet<Transform> splitObjs = new();
    private HashSet<Transform> objectToRemove = new();

    [Header("属性")]
    public float bornToSmall;
    public float comboToBig;
    public float smallMax;
    public float splitMoveToPlayerSpeed;

    private void Awake()
    {
        rig = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        //根据大小来确定质量
        rig.mass = transform.localScale.x;
    }
    public void BornSplit()
    {
        //分裂后自己要变小
        if (transform.localScale.x <= smallMax)
            return;
        transform.localScale -= new Vector3(bornToSmall, bornToSmall, bornToSmall);

        Vector3 bornPos = transform.position + new Vector3(0, transform.localScale.y + splitPrefab.transform.localScale.x, 0);
        GameObject obj = Instantiate(splitPrefab, bornPos, Quaternion.identity);
        SplitAction splitAction = obj.GetComponent<SplitAction>();
        splitAction.WakeUpFunc();
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
                    OnComboSplit?.Invoke(PlayerParticle.ParitcleName.comboSplit);

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

    public void ClearAllSplits()
    {
        foreach (var item in splitObjs)
        {
            Destroy(item.gameObject);
        }
        splitObjs.Clear();
        objectToRemove.Clear();
    }

    public void AddSplitInList(Transform transform)
    {
        splitObjs.Add(transform);
    }

    private void OnEnable()
    {
        SplitAction.OnSplitWakeUp += AddSplitInList;
        PlayerControl.OnRestartLevel += ClearAllSplits;
        ScenesManager.OnNextLevel += ClearAllSplits;
    }
    private void OnDisable()
    {
        SplitAction.OnSplitWakeUp -= AddSplitInList;
        PlayerControl.OnRestartLevel -= ClearAllSplits;
        ScenesManager.OnNextLevel -= ClearAllSplits;
    }
}
