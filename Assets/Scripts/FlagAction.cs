using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagAction : MonoBehaviour
{
    public Material material_off;
    public Material material_on;
    private MeshRenderer meshRenderer;
    public GameObject pointLight;

    [Header("属性")]
    public bool isFlag;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    private void OnTriggerStay(Collider other)
    {

        if (other.transform.CompareTag("Player") || other.transform.CompareTag("Box") || other.transform.CompareTag("PlayerSplit"))
        {
            isFlag = true;
            meshRenderer.material = material_on;
            pointLight.SetActive(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.transform.CompareTag("Player") || other.transform.CompareTag("Box") || other.transform.CompareTag("PlayerSplit"))
        {
            isFlag = false;
            meshRenderer.material = material_off;
            pointLight.SetActive(false);
        }
    }
}
