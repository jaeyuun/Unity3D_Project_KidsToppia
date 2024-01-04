using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpButton : MonoBehaviour
{
    [SerializeField] private PlayerControll player;

    private void Awake()
    {
        if (Application.platform != RuntimePlatform.Android)
        {
            gameObject.SetActive(false);
        }
    }

    public void PlayerJump()
    {
        if (player == null)
        {
            player = FindObjectOfType<PlayerControll>();
        }
        player.PlayerJump_And();
    }
}
