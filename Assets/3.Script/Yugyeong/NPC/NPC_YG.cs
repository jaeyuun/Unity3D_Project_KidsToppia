using System.Collections;
using UnityEngine;

public enum NPC
{
    Squirrel=0, //맵 돌아다니는 NPC
    Cat //플레이어 따라다니는 NPC
}
public abstract class NPC_YG : MonoBehaviour
{
    [SerializeField] private Transform trans;
    [SerializeField] private Animator ani;

    [SerializeField] private Transform[] goals;
    [SerializeField] private Transform goal;
    [SerializeField] private int index;

    [SerializeField] private Touch touch;
    [SerializeField] private Vector3 touched_pos;
    [SerializeField] private Vector3 mouse_pos;
    [SerializeField] private LayerMask layer;
    [SerializeField] private NPC npc;
    [SerializeField] private bool can_move;

    [Header("Cat")]
    [SerializeField] private GameObject player;
    [SerializeField] private Rigidbody rigid_;


    private void Awake()
    {
        //컴포넌트 가져오기
        TryGetComponent(out ani);
        TryGetComponent(out trans);
        TryGetComponent(out rigid_);

        //델리게이트 등록하기
        TalkManager.event_talkend += Turn_canmove;

        can_move = false;
        goal = goals[0];
    }

    private void Start()
    {
        StartCoroutine(Find_posttion());
        StartCoroutine(Set_position());
    }

    private void Update()
    {
        Input_touch();
    }

    private void Turn_canmove()
    {
        can_move = !can_move;
    }

    virtual public IEnumerator Find_posttion()
    {
        if (npc == NPC.Squirrel)
        {
            while (true)
            {
                goal.position = player.transform.position;
                yield return null;
            }
        }
        //-------------------------------------------------------
        else //cat
        {
            index = 0;
            while (true)
            {
                index = Random.Range(0, goals.Length);
                if (goals[index] != goal)
                {
                    index = Random.Range(0, goals.Length);
                    //Debug.Log($"이번 목표 / {goal.name}");
                    //Debug.Log($"목표 위치 / {goal.position}");
                    break;
                }
                yield return null;
            }
            yield return null;
        }
    }

    IEnumerator Set_position()
    {
        if (npc == NPC.Squirrel)
        {
            while (true)
            {
                Debug.Log(Vector3.Distance(trans.position, goal.position));
                if (Vector3.Distance(trans.position, goal.position) >= 1f)
                {
                    ani.SetBool("is_walk", true);
                    Vector3 tmprot = goal.position - transform.position;
                    tmprot.y = 0;
                    tmprot.Normalize();
                    transform.rotation = Quaternion.LookRotation(tmprot);
                    if (can_move)
                    {
                        transform.position = Vector3.MoveTowards(transform.position, goal.position, Time.deltaTime);
                    }
                }
                else
                {
                    ani.SetBool("is_walk", false);
                    rigid_.velocity = Vector3.zero;
                }
                yield return null;
            }
        }
        //-------------------------------------------------------
        else
        {
            while (true)
            {
                //Debug.Log(Vector3.Distance(trans.position, goal.position));

                if (Vector3.Distance(trans.position, goal.position) >= 0.75f)
                {
                    ani.SetBool("is_walk", true);
                    Vector3 tmprot = goal.position - transform.position;
                    tmprot.y = 0;
                    tmprot.Normalize();
                    transform.rotation = Quaternion.LookRotation(tmprot);
                    if (can_move)
                    {
                        transform.position = Vector3.MoveTowards(transform.position, goal.position, Time.deltaTime);
                    }
                }

                else
                {
                    ani.SetBool("is_walk", false);
                    yield return new WaitForSeconds(3f);
                    index = Random.Range(0, goals.Length);
                    goal = goals[index];
                    break;
                }
                yield return null;
            }
            StartCoroutine(Set_position());
        }

    }

    private void Input_touch()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.touchCount > 0)
            {
                touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Began)
                {
                    touched_pos = Camera.main.ScreenToWorldPoint(touch.position);
                    Try_raycast(touch.position);
                }
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                //Debug.Log("Input.GetMouseButtonDown(0)");
                mouse_pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Try_raycast(Input.mousePosition);
            }
        }
    }

    private void Try_raycast(Vector3 pos)
    {
        //Debug.Log("Try_raycast");
        Ray ray;
        RaycastHit hit;
        ray = Camera.main.ScreenPointToRay(pos);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layer))
        {
            //Debug.Log("레이 쏨");
            if (hit.collider.CompareTag("NPC"))
            {
                Interactive_NPC();
            }
        }
    }

    private void Interactive_NPC()
    {
        //Debug.Log("NPC찾음");
        TalkManager.instance.Print();
    }

}

public class Squirrel : NPC_YG
{

}
