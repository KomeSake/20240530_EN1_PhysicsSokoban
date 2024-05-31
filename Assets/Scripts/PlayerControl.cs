using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerControl : MonoBehaviour
{
    public PlayerInputSystem inputActions;
    private PlayerSplit playerSplit;
    private Rigidbody rig;
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
        //BornSplit
        if (inputActions.Player.Actions_split.triggered)
        {
            if (this.CompareTag("Player"))
                playerSplit.BornSplit();
        }
        //MoveToPlayer 
        if (inputActions.Player.Actions_Combo.ReadValue<float>() > 0)
        {
            if (this.CompareTag("Player"))
                playerSplit.MoveToPlayer();
        }
    }

    private void FixedUpdate()
    {
        if (moveDir.sqrMagnitude > 0.1f && isGround)
        {
            rig.AddForce(moveSpeed * moveDir * Time.deltaTime);
        }
    }
    private void OnCollisionStay(Collision other)
    {
        if (other.transform.CompareTag("Floor"))
        {
            isGround = true;
        }
    }
    private void OnCollisionExit(Collision other)
    {

        if (other.transform.CompareTag("Floor"))
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
