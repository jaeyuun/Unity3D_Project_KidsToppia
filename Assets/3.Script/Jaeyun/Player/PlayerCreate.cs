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
    private NPC_chaseplayer npc_goppi;
    private PathFinding pathFinding = null;
    [SerializeField] private PlayerName playerName;
    private Transform startPos;

    public NetworkAnimator ani_net;
    public User_info info;
    public User_Character character;

    [SerializeField] private GameObject[] changeObject;
    private GameObject selectObject;

    [SyncVar]
    public SelectMenu selectMenu;
    [SyncVar]
    public Select select;
    [SyncVar]
    public Ride ride;
    [SyncVar]
    public string playerId = string.Empty;

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
    public override void OnStartClient()
    {
        base.OnStartClient();
        if (isLocalPlayer)
        {
            ChangeLayer(this.gameObject, 7);
        }

        StartCoroutine(SetPlayer());
    }

    private IEnumerator SetPlayer()
    {
        if (isLocalPlayer)
        {
            playerId = SQLManager.instance.info.User_Id;
            PlayerIDSet_CM(playerId);

            // PathFinding
            pathFinding = FindObjectOfType<PathFinding>();
            pathFinding.playerObject = gameObject;
            pathFinding.player = gameObject.transform;

            // player follow npc
            npc_goppi = FindObjectOfType<NPC_chaseplayer>();
            npc_goppi.player = gameObject;
        }
        else
        {
            yield return new WaitForSeconds(0.5f);
        }
        // SQLManager
        startPos = GameObject.FindGameObjectWithTag("StartPos").transform;
        this.transform.position = startPos.position;
        info = SQLManager.instance.PlayerInfo(playerId);
        character = SQLManager.instance.CharacterInfo(playerId);
        playerName.PlayerNameSet();
        
        // Eyes
        EyesChange(changeObject[0], character.User_Eyes);
        // Jumper, Runners, TShirts, Trunks, Skin
        OtherChange(changeObject[1], character.User_Jummper);
        OtherChange(changeObject[2], character.User_Runners);
        OtherChange(changeObject[3], character.User_TShirt);
        OtherChange(changeObject[4], character.User_Trunk);
        OtherChange(changeObject[5], character.User_Skin);
        // Hat
        for (int i = 0; i < hatObjects.Length; i++)
        {
            hatObjects[i].SetActive(false);
        }
        hatObjects[character.User_Hat].SetActive(true);
        // Ride
        for (int i = 0; i < rides.Length; i++)
        {
            rides[i].SetActive(false);
        }
        ani_net.animator.SetBool("isDriving", false);
        // HairStyles
        for (int i = 0; i < hairStyles.Length; i++)
        {
            hairStyles[i].SetActive(false);
        }
        hairStyles[character.User_HairStyle].SetActive(true);
    }

    private void EyesChange(GameObject obj, int index)
    {
        MeshRenderer meshRenderer = obj.GetComponent<MeshRenderer>();
        Material[] newMaterial = meshRenderer.materials;
        newMaterial[0] = this.materials[index];
        meshRenderer.materials = newMaterial;
    }

    private void OtherChange(GameObject obj, int index)
    {
        SkinnedMeshRenderer skinnedMeshRenderer = obj.GetComponent<SkinnedMeshRenderer>();
        Material[] newSkinnedMaterial = skinnedMeshRenderer.materials;
        newSkinnedMaterial[0] = this.materials[index];
        skinnedMeshRenderer.materials = newSkinnedMaterial;
    }

    private void ChangeLayer(GameObject obj, int layer)
    { // RawImage에 나올 플레이어에 레이어 적용하기
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

    [Command]
    private void PlayerIDSet_CM(string playerID)
    {
        playerId = playerID;
        PlayerIDSet_RPC(playerId);
    }

    #endregion
    #region ClientRPC
    [ClientRpc]
    private void PlayerIDSet_RPC(string playerID)
    {
        playerId = playerID;
        info = SQLManager.instance.PlayerInfo(playerId);
        character = SQLManager.instance.CharacterInfo(playerId);
        playerName.PlayerNameSet();
    }

    [ClientRpc]
    public void Material_Change()
    {
        selectObject = changeObject[(int)selectMenu];
        if (selectMenu == SelectMenu.Eyes)
        {
            EyesChange(selectObject, (int)select);
        }
        else
        {
            OtherChange(selectObject, (int)select);
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
