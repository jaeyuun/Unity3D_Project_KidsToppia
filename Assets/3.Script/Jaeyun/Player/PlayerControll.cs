using Mirror;
using UnityEngine;

public class PlayerControll : NetworkBehaviour
{
    private FixedJoystick joystick;
    [SerializeField] private GameObject lenderCamera;
    public GameObject createPanel = null;

    // Network Component
    public NetworkRigidbodyReliable rigid_net;
    public NetworkTransformReliable trans_net;
    public NetworkAnimator ani_net;

    public Rigidbody playerRigid;
    private Transform playerTrans;

    [SerializeField] private float moveSpeed = 3f;
    public float jumpForce = 3f;
    public bool isGround = true;
    public bool isJoystick = false;
    public bool isLender = false;

    public override void OnStartClient()
    {
        base.OnStartClient();
        if (isLocalPlayer)
        {
            joystick = FindObjectOfType<FixedJoystick>();
            createPanel = FindObjectOfType<CreateCharacterButton>().gameObject;
            createPanel.SetActive(false);
            if (Application.platform != RuntimePlatform.Android)
            {
                joystick.gameObject.SetActive(false);
            }
        }

        TryGetComponent(out rigid_net);
        TryGetComponent(out trans_net);
        TryGetComponent(out ani_net);

        playerRigid = rigid_net.target.GetComponent<Rigidbody>();
        playerTrans = trans_net.target.transform;
    }

    private void FixedUpdate()
    {
        if (!isLocalPlayer) return;
        if (isClient && !lenderCamera.activeSelf)
        {
            lenderCamera.SetActive(true);
        }
        if (Application.platform == RuntimePlatform.Android)
        {
            PlayerMovement_And();
        }
        else
        { // window
            PlayerMovement_Win();
            PlayerJump_Win();
        }
    }

    private void PlayerMovement_And()
    {
        playerRigid.velocity = new Vector3(joystick.Horizontal * moveSpeed, playerRigid.velocity.y, joystick.Vertical * moveSpeed);

        if (joystick.Horizontal != 0 || joystick.Vertical != 0)
        {
            isJoystick = true;
            playerTrans.rotation = Quaternion.LookRotation(new Vector3(playerRigid.velocity.x, 0f, playerRigid.velocity.z)); // jump 했을 때 앞으로 넘어지지 않게 만듦
            ani_net.animator.SetBool("isWalking", true);
        }
        else
        {
            isJoystick = false;
            ani_net.animator.SetBool("isWalking", false);
        }
    }

    private void PlayerMovement_Win()
    {
        if (createPanel.activeSelf)
        {
            ani_net.animator.SetBool("isWalking", false);
            return;
        }

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        playerRigid.velocity = new Vector3(horizontal * moveSpeed, playerRigid.velocity.y, vertical * moveSpeed);

        if (horizontal != 0 || vertical != 0)
        {
            playerTrans.rotation = Quaternion.LookRotation(new Vector3(playerRigid.velocity.x, 0f, playerRigid.velocity.z)); // jump 했을 때 앞으로 넘어지지 않게 만듦
            ani_net.animator.SetBool("isWalking", true);
        }
        else
        {
            ani_net.animator.SetBool("isWalking", false);
        }
    }

    public void PlayerJump_And()
    {
        if (!isLocalPlayer) return;
        if (isGround)
        {
            playerRigid.AddForce(new Vector3(0f, 2f, 0f) * jumpForce, ForceMode.Impulse);
        }
    }

    public void PlayerJump_Win()
    {
        if (!isLocalPlayer) return;
        if (Input.GetKeyDown(KeyCode.Space) && isGround)
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
