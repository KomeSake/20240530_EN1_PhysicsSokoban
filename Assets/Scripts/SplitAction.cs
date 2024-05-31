using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplitAction : MonoBehaviour
{
    public static event System.Action<Transform> OnSplitWakeUp;
    private SphereCollider coll;
    public Material materialSleep;
    public Material materialWakeUp;
    private MeshRenderer meshRenderer;
    private PlayerControl playerControl;
    [Header("属性")]
    public bool isTouch;
    public bool isWakeUp;

    private void Awake()
    {
        coll = GetComponent<SphereCollider>();
        meshRenderer = GetComponent<MeshRenderer>();
        playerControl = GetComponent<PlayerControl>();
    }
    private void Start()
    {
        // playerControl.enabled = false;
        // meshRenderer.material = materialSleep;
    }
    private void Update()
    {
    }
    private void OnCollisionEnter(Collision other)
    {
        if (!isWakeUp)//没唤醒状态
        {
            if (other.transform.CompareTag("Player") || other.transform.CompareTag("PlayerSplit"))
            {
                WakeUpFunc();
                OnSplitWakeUp?.Invoke(transform);
            }
        }
        else           //唤醒状态
        {
            if (other.transform.CompareTag("Player"))
            {
                isTouch = true;
            }
        }
    }
    private void OnCollisionExit(Collision other)
    {
        if (isWakeUp)
        {
            if (other.transform.CompareTag("Player"))
            {
                isTouch = false;
            }
        }
    }

    public void WakeUpFunc()
    {
        meshRenderer.material = materialWakeUp;
        playerControl.enabled = true;
        isWakeUp = true;
    }
}
