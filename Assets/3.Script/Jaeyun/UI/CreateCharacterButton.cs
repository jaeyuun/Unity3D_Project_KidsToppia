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
            player = FindObjectOfType<PlayerCreate>();
        }
    }

    public void SelectMenuButton(int index)
    {
        player.selectMenu = (SelectMenu)index;
        Debug.Log(player.selectMenu);
}

    public void SelectButton(int index)
    {
        player.select = (Select)index;
        Debug.Log(player.select);
    }

    public void RidingSelectButton(int index)
    {
        player.ride = (Ride)index;
        player.isRiding = !player.isRiding;
    }

    public void PlayerChange()
    {
        player.MenuSelect();
    }
}
