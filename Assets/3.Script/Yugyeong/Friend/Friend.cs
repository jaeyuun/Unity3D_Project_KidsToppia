using UnityEngine;
using UnityEngine.UI;

public class Friend : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Friend_slot[] slots;
    private Friend_data data;

    //친구 정보 불러오기
    private void OnEnable()
    {
        data = SQLManager.instance.Friend();
    }
    //따라가기
    //온라인 오프라인 출력하기
    //친구 이름 띄우기
    //친구 삭제하기
}
