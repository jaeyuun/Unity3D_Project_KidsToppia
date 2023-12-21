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
        player.PlayerJump();
    }
}
