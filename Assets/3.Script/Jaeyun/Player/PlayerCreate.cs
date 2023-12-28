using System.Collections;
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
    public NetworkAnimator ani_net;

    [SerializeField] private GameObject[] changeObject;
    private GameObject selectObject;

    [SyncVar]
    public SelectMenu selectMenu;
    [SyncVar]
    public Select select;
    [SyncVar]
    public Ride ride;

    public Material[] materials; // Eyes, Jumper, Runners, TShirts, Trunks, Skin
    public GameObject[] hatObjects; // Hat
    public GameObject[] hairStyles; // Hair
    public GameObject[] rides; // Riding

    [SyncVar]
    public bool isRiding = false; // 라이딩 착용한지 안한지 확인
    [SyncVar]
    private bool isMaterial = false;
    [SyncVar]
    private bool isGameObject = false;

    private char updateRiding = 'F'; // Database에 들어있는 라이딩 탔는지 안탔는지 유무

    #region Unity CallBack Method
    private void Start()
    {
        SetPlayer();
        ChangeLayer(this.gameObject, 7);
    }

    private void SetPlayer()
    {
        // eyes
        MeshRenderer meshRenderer = changeObject[0].GetComponent<MeshRenderer>();
        Material[] newMaterial = meshRenderer.materials;
        newMaterial[0] = this.materials[(int)select];
        meshRenderer.materials = newMaterial;
        // Jumper, Runners, TShirts, Trunks, Skin
        SkinnedMeshRenderer skinnedMeshRenderer = changeObject[0].GetComponent<SkinnedMeshRenderer>();
        Material[] newSkinnedMaterial = meshRenderer.materials;
        newMaterial[0] = this.materials[(int)select];
        meshRenderer.materials = newMaterial;
    }

    private void ChangeLayer(GameObject obj, int layer)
    {
        if (!isLocalPlayer) return;
        obj.layer = layer;
        foreach (Transform child in obj.transform)
        {
            ChangeLayer(child.gameObject, layer);
        }
    }
    #endregion

    #region Client
    public void MenuSelect()
    { // Eyes, Jumpper, Runners, TShirts, Trunks, Skin, Hat, Paw
        if (!isLocalPlayer) return;
        ListSelect_CM(selectMenu, select, ride);
    }
    #endregion
    #region Command
    [Command(requiresAuthority = false)]
    private void ListSelect_CM(SelectMenu _selectMenu, Select _select, Ride _ride)
    {
        selectMenu = _selectMenu;
        select = _select;
        ride = _ride;

        switch (_selectMenu)
        {
            case SelectMenu.Eyes:
            case SelectMenu.Jummper:
            case SelectMenu.Runners:
            case SelectMenu.TShirts:
            case SelectMenu.Trunks:
            case SelectMenu.Skin:
                isMaterial = true;
                break;
            case SelectMenu.Riding:
                isRiding = !isRiding;
                isGameObject = true;
                break;
            case SelectMenu.Hat:
            case SelectMenu.HairStyle:
                isGameObject = true;
                break;
        }
        StartCoroutine(Perform_Co());
    }
    private IEnumerator Perform_Co()
    {
        yield return null;
        if (isMaterial)
        {
            Material_Change();
            isMaterial = false;
        }
        if (isGameObject)
        {
            GameObject_Change();
            // RidingAnimaition();
            isGameObject = false;
        }
    }
    #endregion
    #region ClientRPC
    [ClientRpc]
    public void Material_Change()
    {
        selectObject = changeObject[(int)selectMenu];
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
        switch (selectMenu)
        {
            case SelectMenu.Eyes:
                SQLManager.instance.character_info.User_Eyes = (int)select;
                break;
            case SelectMenu.Jummper:
                SQLManager.instance.character_info.User_Jummper = (int)select;
                break;
            case SelectMenu.Runners:
                SQLManager.instance.character_info.User_Runners = (int)select;
                break;
            case SelectMenu.TShirts:
                SQLManager.instance.character_info.User_TShirt = (int)select;
                break;
            case SelectMenu.Trunks:
                SQLManager.instance.character_info.User_Trunk = (int)select;
                break;
            case SelectMenu.Skin:
                SQLManager.instance.character_info.User_Skin = (int)select;
                break;
            case SelectMenu.Hat:
                SQLManager.instance.character_info.User_Hat = (int)select;
                break;
        }
        if (isLocalPlayer)
        {
            string updateMenu = selectMenu.ToString();
            int updateSelect = (int)select;
            SQLManager.instance.UpdateData(updateMenu, updateSelect);
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
            if (isRiding)
            {
                rides[(int)ride].SetActive(true);
            }
            else
            {
                for (int i = 0; i < rides.Length; i++)
                {
                    rides[i].SetActive(false);
                }
            }
            // Animation
            if (isLocalPlayer)
            {
                if (!isRiding)
                {
                    ani_net.animator.SetBool("isDriving", false);
                }
                else
                {
                    ani_net.animator.SetBool("isDriving", true);
                }
            }
        }
        else if (selectMenu == SelectMenu.Hat)
        {
            for (int i = 0; i < hatObjects.Length; i++)
            {
                hatObjects[i].SetActive(false);
            }
            hatObjects[(int)select].SetActive(true);
        }
        switch (selectMenu)
        {
            case SelectMenu.Hat:
                SQLManager.instance.character_info.User_Hat = (int)select;
                break;
            case SelectMenu.Riding:
                SQLManager.instance.character_info.User_Ride = (int)select;
                break;
            case SelectMenu.HairStyle:
                SQLManager.instance.character_info.User_HairStyle = (int)select;
                break;
        }
        if (isLocalPlayer)
        {
            string updateMenu = selectMenu.ToString();
            int updateSelect = (int)select;
            if (isRiding)
            {
                updateRiding = 'T';
            }
            else
            {
                updateRiding = 'F';
            }
            switch (selectMenu)
            {
                case SelectMenu.Riding:
                    SQLManager.instance.UpdateRiding(updateRiding, updateSelect);
                    break;
                case SelectMenu.Hat:
                case SelectMenu.HairStyle:
                    SQLManager.instance.UpdateData(updateMenu, updateSelect);
                    break;
            }
        }
    }
    #endregion
}
