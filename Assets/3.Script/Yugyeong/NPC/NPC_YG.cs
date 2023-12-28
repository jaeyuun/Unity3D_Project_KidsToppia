using System.Collections;
using UnityEngine;

public abstract class NPC_YG : MonoBehaviour
{
    [Header("NPC")]
    public Transform trans;
    public Animator ani;
    public Transform goal;
    public bool can_move;

    virtual public void Awake()
    {
        //컴포넌트 가져오기
        TryGetComponent(out ani);
        TryGetComponent(out trans);

        //델리게이트 등록하기
        TalkManager.event_talkend += Turn_canmove;
    }

    private void Start()
    {
        StartCoroutine(Find_posttion());
        StartCoroutine(Set_position());
    }

    private void Turn_canmove()
    {
        can_move = !can_move;
    }

    virtual public IEnumerator Find_posttion()
    {
        yield return null;
    }

    virtual public IEnumerator Set_position()
    {
        yield return null;
    }

   

}
