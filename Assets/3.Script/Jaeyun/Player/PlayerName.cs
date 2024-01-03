using Mirror;
using TMPro;
using UnityEngine;

public class PlayerName : NetworkBehaviour
{
    [SerializeField] private PlayerCreate player;
    [SerializeField] private TMP_Text nameText;

    private void FixedUpdate()
    {
        PlayerNameUpdate();
    }

    public void PlayerNameSet()
    {
        nameText.text = player.info.User_NickName;
    }

    private void PlayerNameUpdate()
    {
        nameText.gameObject.transform.position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, 2f, 0));
    }
}
