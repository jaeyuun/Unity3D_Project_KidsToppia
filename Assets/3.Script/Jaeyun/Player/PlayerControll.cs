using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerControll : NetworkBehaviour
{
    [SerializeField] private FixedJoystick joystick;

    // Network Component
    public NetworkRigidbodyReliable playerRigid_net;
    public NetworkTransformReliable playerTrans_net;
    [SerializeField] private NetworkAnimator animator;

    public Rigidbody playerRigid;
    private Transform playerTrans;
    /*[SerializeField] private Animator playerAni;*/

    [SerializeField] private float moveSpeed = 3f;
    public float jumpForce = 3f;
    public bool isGround;

    private void OnEnable()
    {
        joystick = FindObjectOfType<FixedJoystick>();
        TryGetComponent(out playerRigid_net);
        TryGetComponent(out playerTrans_net);

        playerRigid = playerRigid_net.target.GetComponent<Rigidbody>();
        playerTrans = playerTrans_net.target.transform;
    }

    private void FixedUpdate()
    {
        PlayerMovement();
    }

    private void PlayerMovement()
    {
        playerRigid.velocity = new Vector3(joystick.Horizontal * moveSpeed, playerRigid.velocity.y, joystick.Vertical * moveSpeed);

        if (joystick.Horizontal != 0 || joystick.Vertical != 0 && isLocalPlayer)
        {
            playerTrans.rotation = Quaternion.LookRotation(new Vector3(playerRigid.velocity.x, 0f, playerRigid.velocity.z)); // jump 했을 때 앞으로 넘어지지 않게 만듦
            animator.animator.SetBool("isWalk", true);
            /*playerAni.SetBool("isWalk", true);*/
        }
        else
        {
            animator.animator.SetBool("isWalk", false);
        }
    }
}
