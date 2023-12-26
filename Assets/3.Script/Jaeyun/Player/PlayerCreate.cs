using Mirror;
using System.Collections;
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
    public Mesh[] hatMeshs; // Hat

    public GameObject[] hairStyles; // Hair
    public GameObject[] rides; // Riding

    [SyncVar]
    public bool isRiding = false; // 라이딩 착용한지 안한지 확인
    // Command를 통해 바꿔줄 변수
    [SyncVar]
    private bool isMaterial = false;
    [SyncVar]
    private bool isMeshAndMaterial = false;
    [SyncVar]
    private bool isGameObject = false;

    #region Unity CallBack Method
    private void OnEnable()
    {
        if (!isLocalPlayer) return;
        TryGetComponent(out ani_net);
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
            case SelectMenu.Hat:
                isMeshAndMaterial = true;
                break;
            case SelectMenu.Riding:
                isRiding = !isRiding;
                isGameObject = true;
                break;
            case SelectMenu.HairStyle:
                isGameObject = true;
                break;
        }
        StartCoroutine(perform());
    }
    private IEnumerator perform() {
        yield return null;
        if (isMaterial)
        {
            Material_Change();
            isMaterial = false;
        }
        if (isMeshAndMaterial)
        {
            MeshAndMaterial_Change();
            isMeshAndMaterial = false;
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
    }
    [ClientRpc]
    public void MeshAndMaterial_Change()
    { // Hat
      // MeshRenderer
        selectObject = changeObject[(int)selectMenu];
        Debug.Log(selectObject);
        MeshRenderer meshRenderer = selectObject.GetComponent<MeshRenderer>();
        Material[] newMaterial = meshRenderer.materials;
        newMaterial[0] = this.materials[(int)select];
        meshRenderer.materials = newMaterial;

        MeshFilter meshFilter = selectObject.GetComponent<MeshFilter>();
        meshFilter.mesh = this.hatMeshs[(int)select];

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
            if (isRiding)
            {
                rides[(int)ride].SetActive(true);
            }
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
    }
    #endregion
}
