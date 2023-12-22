using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public enum SelectMenu
{
    Jummper = 0,
    Runners,
    TShirts,
    Trunks,
    Skin,
    Hap,
    Paw,
    Riding
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
    Panda
}

public enum Ride
{
    BoxBus = 0,
    BoxCart,
    BoxPickUp,
    BoxScooter,
    BoxTruck
}

public class PlayerCreate : NetworkBehaviour, IState_Select
{
    [SerializeField] private GameObject[] changeObject;
    [SyncVar(hook = nameof(SetPlayer))]
    private GameObject selectObject;

    public SelectMenu selectMenu;
    public Select select;

    // Jumper, Runners, TShirts, Trunks, Skin
    public Material[] materials;

    // Hat
    public Mesh[] hatMesh;
    public Material[] hatMaterials;

    // Paw
    public Mesh[] pawMesh;
    public Material[] pawMaterials;

    // Riding
    public GameObject[] ride;

    #region Unity CallBack Method

    #endregion
    #region Client
    [Client]
    public void MenuSelect()
    { // Jumpper, Runners, TShirts, Trunks, Skin, Hat, Paw, Riding
        selectObject = changeObject[(int)selectMenu];
        ListSelect_CM();
    }
    #endregion
    #region Command
    [Command]
    private void ListSelect_CM()
    {
        switch (selectMenu)
        {
            case SelectMenu.Jummper:
                Material_Change();
                break;
        }
    }
    #endregion
    #region ClientRPC
    #endregion
    #region IState Select
    public void Material_Change()
    {
        SkinnedMeshRenderer skinnedMeshRenderer = selectObject.GetComponent<SkinnedMeshRenderer>();
        skinnedMeshRenderer.materials[0] = this.materials[(int)select];
    }

    public void MeshAndMaterial_Change()
    {

    }

    public void Riding_Change()
    {

    }
    #endregion
    private void SetPlayer(GameObject oldPlayer, GameObject newPlayer)
    {
        selectObject = newPlayer;
    }
}
