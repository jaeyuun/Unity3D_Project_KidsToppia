using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreateCharacterButton : MonoBehaviour
{
    [SerializeField] private PlayerCreate player;
    [SerializeField] private PlayerCreate_Scene player_scene;

    private void OnEnable()
    {
        if (player == null && SceneManager.GetActiveScene().name.Equals("MainGame_J"))
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
        if (SceneManager.GetActiveScene().name.Equals("MainGame_J")) // todo... SceneName 바뀌었을 때 변경 필수!
        {
            player.selectMenu = (SelectMenu)index;
        }
        else if (SceneManager.GetActiveScene().name.Equals("CreateScene"))
        {
            player_scene.selectMenu = (SelectMenu)index;
        }
    }

    public void SelectButton(int index)
    {
        if (SceneManager.GetActiveScene().name.Equals("MainGame_J")) // todo... SceneName 바뀌었을 때 변경 필수!
        {
            player.select = (Select)index;
        }
        else if (SceneManager.GetActiveScene().name.Equals("CreateScene"))
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
        if (SceneManager.GetActiveScene().name.Equals("MainGame_J")) // todo... SceneName 바뀌었을 때 변경 필수!
        {
            player.MenuSelect();
        }
        else if (SceneManager.GetActiveScene().name.Equals("CreateScene"))
        {
            player_scene.MenuSelect();
        }
    }
}
