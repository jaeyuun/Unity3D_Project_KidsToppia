using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateCharacterButton : MonoBehaviour
{
    private PlayerCreate player;

    private void OnEnable()
    {
        if (player == null)
        {
            player = FindObjectOfType<PlayerCreate>();
        }
    }

    public void MenuSelectButton(int index)
    {
        player.selectMenu = (SelectMenu)index;
}

    public void SelectButton(int index)
    {
        player.select = (Select)index;
    }

    public void PlayerChange()
    {
        player.MenuSelect();
    }
}
