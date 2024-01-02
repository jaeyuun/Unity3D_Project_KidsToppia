using UnityEngine;

public class PlayerCreate_Scene : MonoBehaviour, IState_Select
{ // 캐릭터 맨 처음에 생성할 때 사용하는 스크립트
    [SerializeField] private GameObject[] changeObject;
    private GameObject selectObject;

    public SelectMenu selectMenu;
    public Select select;

    public Material[] materials; // Eyes, Jumper, Runners, TShirts, Trunks, Skin
    public GameObject[] hatObjects; // Hat
    public GameObject[] hairStyles; // Hair

    public void MenuSelect()
    { // Eyes, Jumpper, Runners, TShirts, Trunks, Skin, Hat, Paw
        ListSelect_CM(selectMenu, select);
    }
    private void ListSelect_CM(SelectMenu _selectMenu, Select _select)
    {
        selectMenu = _selectMenu;
        select = _select;

        switch (_selectMenu)
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
            case SelectMenu.HairStyle:
                GameObject_Change();
                break;
        }
        // Database에 저장하는 Method 추가
    }
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
        }
        string updateMenu = selectMenu.ToString();
        int updateSelect = (int)select;
        SQLManager.instance.UpdateData(updateMenu, updateSelect);
    }
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
            case SelectMenu.HairStyle:
                SQLManager.instance.character_info.User_HairStyle = (int)select;
                break;
            default:
                break;
        }
        string updateMenu = selectMenu.ToString();
        int updateSelect = (int)select;
        SQLManager.instance.UpdateData(updateMenu, updateSelect);
    }
}
