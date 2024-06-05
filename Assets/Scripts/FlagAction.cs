using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagAction : MonoBehaviour
{
    public enum FLAGTYPE
    {
        box,
        sphere,
    }
    [SerializeField]
    public FLAGTYPE type;
    public Material material_off;
    public Material material_on;
    public Material material_off_sphere;
    public Material material_on_sphere;
    private MeshRenderer meshRenderer;
    public GameObject pointLight;

    [Header("属性")]
    public bool isFlag;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }
    private void Start()
    {
        Light light = pointLight.GetComponent<Light>();
        switch (type)
        {
            case FLAGTYPE.box:
                meshRenderer.material = material_off;
                light.color = new Color(1, 0.9105f, 0, 1);
                break;
            case FLAGTYPE.sphere:
                meshRenderer.material = material_off_sphere;
                light.color = new Color(0, 1, 0.9229f, 1);
                break;
            default:
                break;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (type == FLAGTYPE.box)
        {
            if (other.transform.CompareTag("Box"))
            {
                isFlag = true;
                meshRenderer.material = material_on;
                pointLight.SetActive(true);
            }
        }
        else if (type == FLAGTYPE.sphere)
        {
            if (other.transform.CompareTag("Player") || other.transform.CompareTag("PlayerSplit"))
            {
                isFlag = true;
                meshRenderer.material = material_on_sphere;
                pointLight.SetActive(true);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (type == FLAGTYPE.box)
        {
            if (other.transform.CompareTag("Box"))
            {
                isFlag = false;
                meshRenderer.material = material_off;
                pointLight.SetActive(false);
            }
        }
        else if (type == FLAGTYPE.sphere)
        {
            if (other.transform.CompareTag("Player") || other.transform.CompareTag("PlayerSplit"))
            {
                isFlag = false;
                meshRenderer.material = material_off_sphere;
                pointLight.SetActive(false);
            }
        }
    }
}
