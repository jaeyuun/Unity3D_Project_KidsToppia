using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public enum SelectMenu
{
    Eyes = 0,
    Jummper,
    Runners,
    TShirts,
    Trunks,
    Skin,
    Hat,
    Paw,
    Riding,
    HairStyle
}

public enum Select
{
    BlueBear = 0,
    Hippo,
    Shark,
    Lobster,
    Frog,
    Panther,
    Sloth,
    Panda,
    None
}

public enum Ride
{
    BoxBus = 0,
    BoxCart,
    BoxPickUp,
    BoxScooter,
    BoxTruck,
    None
}

public class PlayerCreate : NetworkBehaviour, IState_Select
{
    private PlayerControll player;
    [SerializeField] private GameObject[] changeObject;
    [SyncVar(hook = nameof(SetPlayer))]
    private GameObject selectObject;

    public SelectMenu selectMenu;
    public Select select;
    public Ride ride;

    public Material[] materials; // Eyes, Jumper, Runners, TShirts, Trunks, Skin
    public Mesh[] hatMeshs; // Hat
    public Mesh[] pawMeshs; // Paw

    public GameObject[] hairStyles; // Hair
    public GameObject[] rides; // Riding

    #region Unity CallBack Method
    private void Awake()
    {
        TryGetComponent(out player);
    }
    #endregion
    #region Client
    [Client]
    public void MenuSelect()
    { // Eyes, Jumpper, Runners, TShirts, Trunks, Skin, Hat, Paw
        if (!isLocalPlayer) return;
        selectObject = changeObject[(int)selectMenu];
        ListSelect_CM();
    }
    [Client]
    public void RidingSelect()
    {
        if (!isLocalPlayer) return;
        RidingSelect_CM();
    }
    #endregion
    #region Command
    [Command]
    private void ListSelect_CM()
    {
        switch (selectMenu)
        {
            case SelectMenu.Eyes:
            case SelectMenu.Jummper:
            case SelectMenu.Runners:
            case SelectMenu.TShirts:
            case SelectMenu.Trunks:
            case SelectMenu.Skin:
                Material_Change();
                break;
            case SelectMenu.Hat:
            case SelectMenu.Paw:
                MeshAndMaterial_Change();
                break;
            case SelectMenu.Riding:
            case SelectMenu.HairStyle:
                GameObject_Change();
                break;
        }
    }
    [Command]
    private void RidingSelect_CM()
    {
        GameObject_Change();
    }
    #endregion
    #region ClientRPC
    [ClientRpc]
    public void Material_Change()
    {
        SkinnedMeshRenderer skinnedMeshRenderer = selectObject.GetComponent<SkinnedMeshRenderer>();
        skinnedMeshRenderer.materials[0] = this.materials[(int)select];
    }
    [ClientRpc]
    public void MeshAndMaterial_Change()
    { // Shark Paw는 없음
        selectObject.SetActive(true);
        SkinnedMeshRenderer skinnedMeshRenderer = selectObject.GetComponent<SkinnedMeshRenderer>();
        MeshFilter meshFilter = selectObject.GetComponent<MeshFilter>();
        skinnedMeshRenderer.materials[0] = this.materials[(int)select];
        if (selectMenu == SelectMenu.Hat)
        {
            meshFilter.mesh = this.hatMeshs[(int)select];
        }
        else if (selectMenu == SelectMenu.Paw)
        {
            meshFilter.mesh = this.pawMeshs[(int)select];
        }
        // Hat, Paw 밖에 없음
        if (select == Select.None)
        {
            selectObject.SetActive(false);
        }
    }
    [ClientRpc]
    public void GameObject_Change()
    {
        if (selectMenu == SelectMenu.HairStyle)
        {
            for (int i = 0; i < hairStyles.Length; i++)
            {
                hairStyles[i].SetActive(false);
            }
            hairStyles[(int)select].SetActive(true);
        }
        else if (selectMenu == SelectMenu.Riding)
        {
            for (int i = 0; i < rides.Length; i++)
            {
                rides[i].SetActive(false);
            }
            if (ride == Ride.None)
            {
                player.ani_net.animator.SetBool("isDriving", false);
            }
            else
            {
                player.ani_net.animator.SetBool("isDriving", true);
                rides[(int)ride].SetActive(true);
            }
        }
    }
    #endregion
    private void SetPlayer(GameObject oldPlayer, GameObject newPlayer)
    {
        selectObject = newPlayer;
    }
}
