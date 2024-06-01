using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerControl : MonoBehaviour
{
    public PlayerInputSystem inputActions;
    private PlayerSplit playerSplit;
    private Rigidbody rig;
    public Transform cameraTrans;
    public Vector3 moveDir;
    public float moveSpeed;
    public bool isGround;


    private void Awake()
    {
        inputActions = new PlayerInputSystem();
        playerSplit = GetComponent<PlayerSplit>();
        rig = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        //根据大小来确定推动力的速度

        //Move
        moveDir.x = inputActions.Player.Move.ReadValue<Vector2>().x;
        moveDir.z = inputActions.Player.Move.ReadValue<Vector2>().y;
        if (this.CompareTag("Player"))
        {
            //BornSplit
            if (inputActions.Player.Actions_split.triggered)
                playerSplit.BornSplit();
            //MoveToPlayer 
            if (inputActions.Player.Actions_Combo.ReadValue<float>() > 0)
                playerSplit.MoveToPlayer();
            //位置重生检查
            if (transform.position.y < 0 && transform.position.y > -0.1f)
            {
                rig.velocity = Vector3.down * 5;
                rig.angularVelocity = Vector3.zero;
            }
            if (transform.position.y <= -20)
            {
                transform.position = new Vector3(8.5f, 60, -4);
            }
            //摄像机控制
            cameraTrans.position = new Vector3(cameraTrans.position.z, transform.position.y + 9.5f, cameraTrans.position.z);
            //TODO:摄像机的控制还不是很好，而且球的位置需要加一个线性
        }
    }

    private void FixedUpdate()
    {
        if (moveDir.sqrMagnitude > 0.1f && isGround)
        {
            if (transform.CompareTag("Player"))
            {
                if (transform.localScale.x <= 1)
                {
                    rig.AddForce(transform.localScale.x * 700 * moveSpeed * moveDir * Time.deltaTime);
                }
                else
                {
                    rig.AddForce(transform.localScale.x * 1500 * moveSpeed * moveDir * Time.deltaTime);
                }
            }
            else
            {
                rig.AddForce(700 * moveDir * Time.deltaTime);
            }
        }
    }
    private void OnCollisionStay(Collision other)
    {
        if (other.transform.CompareTag("Floor") || other.transform.CompareTag("Flag"))
        {
            isGround = true;
        }
    }
    private void OnCollisionExit(Collision other)
    {

        if (other.transform.CompareTag("Floor") || other.transform.CompareTag("Flag"))
        {
            isGround = false;
        }
    }

    private void OnEnable()
    {
        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }

}
