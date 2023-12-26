using Mirror;
using UnityEngine;

public enum SelectMenu
{
    Eyes = 0,
    Jummper,
    Runners,
    TShirts,
    Trunks,
    Skin,
    Hat,
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
    private PlayerControll player;

    [SerializeField] private GameObject[] changeObject;
    //[SyncVar(hook = nameof(SetPlayer))]
    [SerializeField] private GameObject selectObject = null;

    //[SyncVar(hook = nameof(SetSelectMenu))]
    public SelectMenu selectMenu;
    //[SyncVar(hook = nameof(SetSelect))]
    public Select select;
    //[SyncVar(hook = nameof(SetRide))]
    public Ride ride;

    public Material[] materials; // Eyes, Jumper, Runners, TShirts, Trunks, Skin
    public Mesh[] hatMeshs; // Hat

    public GameObject[] hairStyles; // Hair
    public GameObject[] rides; // Riding

    public bool isRiding = false;

    #region Unity CallBack Method
    private void Awake()
    {
        TryGetComponent(out player);
    }
    #endregion
    #region Client
    //[Client]
    public void MenuSelect()
    { // Eyes, Jumpper, Runners, TShirts, Trunks, Skin, Hat, Paw
        //if (!isLocalPlayer) return;
        ListSelect_CM();
    }
    #endregion
    #region Command
    //[Command]
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
                selectObject = changeObject[(int)selectMenu];
                Material_Change();
                break;
            case SelectMenu.Hat:
                selectObject = changeObject[(int)selectMenu];
                MeshAndMaterial_Change();
                break;
            case SelectMenu.Riding:
            case SelectMenu.HairStyle:
                GameObject_Change();
                break;
        }
    }
    #endregion
    #region ClientRPC
    //[ClientRpc]
    public void Material_Change()
    {
        if (selectMenu == SelectMenu.Eyes)
        {
            MeshRenderer meshRenderer = selectObject.GetComponent<MeshRenderer>();
            Material[] newMaterial = meshRenderer.materials;
            newMaterial[0] = this.materials[(int)select];
            meshRenderer.materials = newMaterial;
        }
        else
        {
            SkinnedMeshRenderer meshRenderer = selectObject.GetComponent<SkinnedMeshRenderer>();
            Material[] newMaterial = meshRenderer.materials;
            newMaterial[0] = this.materials[(int)select];
            meshRenderer.materials = newMaterial;
        }
    }
    //[ClientRpc]
    public void MeshAndMaterial_Change()
    { // Hat
      // MeshRenderer 
        MeshRenderer meshRenderer = selectObject.GetComponent<MeshRenderer>();
        Material[] newMaterial = meshRenderer.materials;
        newMaterial[0] = this.materials[(int)select];
        meshRenderer.materials = newMaterial;

        MeshFilter meshFilter = selectObject.GetComponent<MeshFilter>();
        meshFilter.mesh = this.hatMeshs[(int)select];

    }
    //[ClientRpc]
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

            if (!isRiding)
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
    #region Hook Method
    /*private void SetPlayer(GameObject oldPlayer, GameObject newPlayer)
    {
        selectObject = newPlayer;
    }

    private void SetSelectMenu(SelectMenu oldSelect, SelectMenu newSelect)
    {
        selectMenu = newSelect;
    }

    private void SetSelect(Select oldSelect, Select newSelect)
    {
        select = newSelect;
    }

    private void SetRide(Ride oldRide, Ride newRide)
    {
        ride = newRide;
    }*/
    #endregion
}
