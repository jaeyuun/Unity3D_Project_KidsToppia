using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpButton : MonoBehaviour
{
    [SerializeField] private PlayerControll player;

    public void PlayerJump()
    {
        if (player == null)
        {
            player = FindObjectOfType<PlayerControll>();
        }

        if (player.isGround)
        {
            player.playerRigid.AddForce(new Vector3(0f, 2f, 0f) * player.jumpForce, ForceMode.Impulse);
        }
    }
}