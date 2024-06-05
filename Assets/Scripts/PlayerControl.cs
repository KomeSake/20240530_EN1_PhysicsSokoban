using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerControl : MonoBehaviour
{
    public static event Action OnRestartLevel;
    public static event Action<PlayerParticle.ParitcleName> OnBornSplit;
    public PlayerInputSystem inputActions;
    private PlayerSplit playerSplit;
    private Rigidbody rig;
    private MeshRenderer meshRenderer;
    public Transform cameraTrans;
    public Vector3 moveDir;
    public float moveSpeed;
    public Material material_on;
    public Material material_off;
    public bool isGround;


    private void Awake()
    {
        inputActions = new PlayerInputSystem();
        playerSplit = GetComponent<PlayerSplit>();
        rig = GetComponent<Rigidbody>();
        meshRenderer = GetComponent<MeshRenderer>();
    }

    private void Update()
    {
        if (this.CompareTag("Player"))
        {
            if (inputActions.Player.Actions_onlyPlay.ReadValue<float>() <= 0)
            {
                //Move
                moveDir.x = inputActions.Player.Move.ReadValue<Vector2>().x;
                moveDir.z = inputActions.Player.Move.ReadValue<Vector2>().y;
                meshRenderer.material = material_on;
            }
            else
            {
                meshRenderer.material = material_off;
            }
        }
        else
        {
            //Move
            moveDir.x = inputActions.Player.Move.ReadValue<Vector2>().x;
            moveDir.z = inputActions.Player.Move.ReadValue<Vector2>().y;
        }
        if (this.CompareTag("Player"))
        {
            //BornSplit
            if (inputActions.Player.Actions_split.triggered)
            {
                if (transform.localScale.x >= playerSplit.smallMax)
                    OnBornSplit?.Invoke(PlayerParticle.ParitcleName.bornSplit);
                playerSplit.BornSplit();
            }
            //MoveToPlayer 
            if (inputActions.Player.Actions_Combo.ReadValue<float>() > 0)
                playerSplit.MoveToPlayer();
            //位置重生检查
            if (transform.position.y < 0 && transform.position.y > -0.1f)
            {
                //将角度和速度变小，防止乱飞
                rig.velocity = Vector3.down * 5;
                rig.angularVelocity = Vector3.zero;
            }
            if (transform.position.y <= -20)
            {
                //将大小和位置也固定，保证可以飞到固定位置
                transform.position = new Vector3(8.5f, 60, -4);
                transform.localScale = Vector3.one;
            }
            //关卡重置
            if (inputActions.Player.Restart.triggered)
            {
                isGround = false;
                OnRestartLevel?.Invoke();
            }
            //摄像机控制
            cameraTrans.position = new Vector3(cameraTrans.position.z, transform.position.y + 9.5f, cameraTrans.position.z);
            //TODO:摄像机的控制还不是很好，而且球的位置需要加一个线性
        }
    }

    private void RestartVelactiy(InputAction.CallbackContext context)
    {
        rig.velocity = Vector3.zero;
        moveDir = Vector3.zero;
    }

    private void FixedUpdate()
    {
        if (moveDir.sqrMagnitude > 0.1f && isGround)
        {
            if (transform.CompareTag("Player"))
            {
                rig.AddForce(moveSpeed * Time.deltaTime * moveDir);
            }
            else
            {
                rig.AddForce(moveSpeed * Time.deltaTime * moveDir);
            }
        }
    }
    private void OnCollisionStay(Collision other)
    {
        if (other.transform.CompareTag("Floor") || other.transform.CompareTag("Flag") || other.transform.CompareTag("Box") || other.transform.CompareTag("PlayerSplit"))
        {
            isGround = true;
        }
    }
    private void OnCollisionExit(Collision other)
    {

        if (other.transform.CompareTag("Floor") || other.transform.CompareTag("Flag") || other.transform.CompareTag("Box") || other.transform.CompareTag("PlayerSplit"))
        {
            isGround = false;
        }
    }

    private void OnEnable()
    {
        inputActions.Enable();
        inputActions.Player.Actions_onlyPlay.performed += RestartVelactiy;
    }

    private void OnDisable()
    {
        inputActions.Disable();
        inputActions.Player.Actions_onlyPlay.performed -= RestartVelactiy;
    }

}
