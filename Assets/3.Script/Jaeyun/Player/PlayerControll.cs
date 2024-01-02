using Mirror;
using UnityEngine;

public class PlayerControll : NetworkBehaviour
{
    private FixedJoystick joystick;
    private GameObject joystickPanel;
    [SerializeField] private GameObject lenderCamera;

    // Network Component
    public NetworkRigidbodyReliable rigid_net;
    public NetworkTransformReliable trans_net;
    public NetworkAnimator ani_net;

    public Rigidbody playerRigid;
    private Transform playerTrans;

    [SerializeField] private float moveSpeed = 3f;
    public float jumpForce = 3f;
    public bool isGround = true;

    private void OnEnable()
    {
        joystick = FindObjectOfType<FixedJoystick>();
        joystickPanel = GameObject.FindGameObjectWithTag("JoystickPanel");
        TryGetComponent(out rigid_net);
        TryGetComponent(out trans_net);
        TryGetComponent(out ani_net);

        playerRigid = rigid_net.target.GetComponent<Rigidbody>();
        playerTrans = trans_net.target.transform;
        joystickPanel.SetActive(false);
    }

    private void FixedUpdate()
    {
        if (!isLocalPlayer) return;
        if (isClient && !lenderCamera.activeSelf)
        {
            lenderCamera.SetActive(true);
        }
        PlayerMovement();
    }

    private void PlayerMovement()
    {
        playerRigid.velocity = new Vector3(joystick.Horizontal * moveSpeed, playerRigid.velocity.y, joystick.Vertical * moveSpeed);

        if (joystick.Horizontal != 0 || joystick.Vertical != 0)
        {
            joystickPanel.SetActive(true);
            playerTrans.rotation = Quaternion.LookRotation(new Vector3(playerRigid.velocity.x, 0f, playerRigid.velocity.z)); // jump 했을 때 앞으로 넘어지지 않게 만듦
            ani_net.animator.SetBool("isWalking", true);
        }
        else
        {
            joystickPanel.SetActive(false);
            ani_net.animator.SetBool("isWalking", false);
        }
    }

    public void PlayerJump()
    {
        if (!isLocalPlayer) return;
        if (isGround)
        {
            playerRigid.AddForce(new Vector3(0f, 2f, 0f) * jumpForce, ForceMode.Impulse);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        isGround = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        isGround = false;
    }
}
