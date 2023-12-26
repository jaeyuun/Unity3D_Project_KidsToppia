using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateCharacterButton : MonoBehaviour
{
    [SerializeField] private PlayerCreate player;

    private void OnEnable()
    {
        if (player == null)
        {
            PlayerCreate[] players = FindObjectsOfType<PlayerCreate>();
            for (int i = 0; i < players.Length; i++)
            {
                if (players[i].isLocalPlayer)
                {
                    player = players[i];
                    break;
                }
            }
        }
        //player.gameObject.transform.position;
    }

    public void SelectMenuButton(int index)
    {
        player.selectMenu = (SelectMenu)index;
}

    public void SelectButton(int index)
    {
        player.select = (Select)index;
    }

    public void RidingSelectButton(int index)
    {
        player.ride = (Ride)index;
    }

    public void PlayerChange()
    {
        player.MenuSelect();
    }
}
