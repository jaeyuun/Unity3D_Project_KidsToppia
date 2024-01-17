using Mirror;
using UnityEngine;

public class PlayerControll : NetworkBehaviour
{
    private FixedJoystick joystick;
    [SerializeField] private Transform mainCamera;
    [SerializeField] private GameObject lenderCamera;
    public GameObject createPanel = null;

    // Network Component
    public NetworkRigidbodyReliable rigid_net;
    public NetworkTransformReliable trans_net;
    public NetworkAnimator ani_net;

    public Rigidbody playerRigid;
    private Transform playerTrans;
    [SerializeField] private Transform playerRotate;
    [SerializeField] private Transform ridingRotate;

    [SerializeField] private float moveSpeed = 3f;
    public float jumpForce = 3f;
    public bool isGround = true;
    public bool isJoystick = false;
    public bool isLender = false;

    [SerializeField] private LayerMask groundLayer;

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
        if (isClient)
        {
            if (!lenderCamera.activeSelf)
            {
                lenderCamera.SetActive(true);
            }
            GroundCheck();
        }
        if (Application.platform == RuntimePlatform.Android)
        {
            PlayerMovement_And();
        }
        else
        { // window
            if (TalkManager.instance.talk_pannel.activeSelf) return;
            PlayerMovement_Win();
            PlayerJump_Win();
        }
    }

    private void PlayerMovement_And()
    {
        if (joystick.Horizontal != 0 || joystick.Vertical != 0)
        {
            isJoystick = true;
            MoveVector(joystick.Horizontal, joystick.Vertical);
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

        if (horizontal != 0 || vertical != 0)
        {
            MoveVector(horizontal, vertical);
            ani_net.animator.SetBool("isWalking", true);
        }
        else
        {
            ani_net.animator.SetBool("isWalking", false);
        }
    }

    private void MoveVector(float horizontal, float vertical)
    {
        Vector3 heading = mainCamera.localRotation * Vector3.forward;
        heading.y = 0;
        heading = heading.normalized;

        Vector3 v3Direction = heading * Time.deltaTime * vertical * moveSpeed;
        v3Direction += Quaternion.Euler(0, 90, 0) * heading * Time.deltaTime * horizontal * moveSpeed;

        playerTrans.Translate(v3Direction);
        MoveRotate_CMD(v3Direction);
    }

    public void PlayerJump_And()
    {
        if (!isLocalPlayer) return;
        if (isGround)
        {
            AudioManager.instance.PlaySFX("Jump");
            playerRigid.AddForce(new Vector3(0f, 2f, 0f) * jumpForce, ForceMode.Impulse);
        }
    }

    public void PlayerJump_Win()
    {
        if (!isLocalPlayer) return;
        if (Input.GetKeyDown(KeyCode.Space) && isGround)
        {
            AudioManager.instance.PlaySFX("Jump");
            playerRigid.AddForce(new Vector3(0f, 2f, 0f) * jumpForce, ForceMode.Impulse);
        }
    }

    private void GroundCheck()
    {
        Vector3 checkPosition = new Vector3(transform.position.x, transform.position.y - 0.2f, transform.position.z);
        Vector3 boxSize = new Vector3(0.5f, 0.2f, 0.5f);
        isGround = Physics.CheckBox(checkPosition, boxSize, Quaternion.identity, groundLayer);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 checkPosition = new Vector3(transform.position.x, transform.position.y - 0.2f, transform.position.z);
        Vector3 boxSize = new Vector3(0.5f, 0.2f, 0.5f);
        Gizmos.DrawWireCube(checkPosition, boxSize);
    }

    #region Command
    [Command]
    private void MoveRotate_CMD(Vector3 v3Direction)
    {
        MoveRotate_RPC(v3Direction);
    }
    #endregion
    #region ClientRpc
    [ClientRpc]
    private void MoveRotate_RPC(Vector3 v3Direction)
    {
        playerRotate.rotation = Quaternion.LookRotation(v3Direction);
        ridingRotate.rotation = Quaternion.LookRotation(v3Direction);
    }
    #endregion
}
