using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagAction : MonoBehaviour
{
    public Material material_off;
    public Material material_on;
    private MeshRenderer meshRenderer;

    [Header("属性")]
    public bool isFlag;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    private void OnCollisionStay(Collision other)
    {
        if (other.transform.CompareTag("Player") || other.transform.CompareTag("Box") || other.transform.CompareTag("PlayerSplit"))
        {
            isFlag = true;
            meshRenderer.material = material_on;
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.transform.CompareTag("Player") || other.transform.CompareTag("Box") || other.transform.CompareTag("PlayerSplit"))
        {
            isFlag = false;
            meshRenderer.material = material_off;
        }
    }
}
