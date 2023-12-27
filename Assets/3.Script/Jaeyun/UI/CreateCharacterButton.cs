using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateCharacterButton : MonoBehaviour
{
    [SerializeField] private PlayerCreate player = null;
    [SerializeField] private PlayerCreate_Scene player_scene = null;

    private void OnEnable()
    {
        if (player_scene == null && player == null)
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
    }

    public void SelectMenuButton(int index)
    {
        if (player != null)
        {
            player.selectMenu = (SelectMenu)index;
        }
        else if (player_scene != null)
        {
            player_scene.selectMenu = (SelectMenu)index;
        }
    }

    public void SelectButton(int index)
    {
        if (player != null) // todo... SceneName 바뀌었을 때 변경 필수!
        {
            player.select = (Select)index;
        }
        else if (player_scene != null)
        {
            player_scene.select = (Select)index;
        }
    }

    public void RidingSelectButton(int index)
    {
        player.ride = (Ride)index;
    }

    public void PlayerChange()
    {
        if (player != null) // todo... SceneName 바뀌었을 때 변경 필수!
        {
            player.MenuSelect();
        }
        else if (player_scene != null)
        {
            player_scene.MenuSelect();
        }
    }
}
