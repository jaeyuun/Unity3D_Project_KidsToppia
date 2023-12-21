using System.Collections;
using UnityEngine;

public class Nonplayer_YG : MonoBehaviour
{
    // Nonplayer : 랜덤으로 움직이게 하기

    [SerializeField] private Rigidbody rigid;
    [SerializeField] private Transform trans;
    [SerializeField] private Animation ani;

    [SerializeField] private float pos_speed = 1;

    [SerializeField] private float max_force = 5;
    [SerializeField] private float min_force = 0;

    [SerializeField] private float max_time = 2;
    [SerializeField] private float min_time = 1;


    private void Awake()
    {
        //컴포넌트 가져오기
        TryGetComponent(out rigid);
        TryGetComponent(out trans);
        TryGetComponent(out ani);
    }

    private void Start()
    {
        StartCoroutine(Random_Movement());
    }

    private void FixedUpdate()
    {
        Vector3 moveDirection = trans.forward;
        rigid.velocity = Vector3.forward * pos_speed;
        Debug.Log($"rigid.velocity: {rigid.velocity} / pos_speed : {pos_speed}");
    }

    IEnumerator Random_Movement()
    {
        while (true)
        {
            //pos_speed = 1f; //처음에 가만히 있기
            float rot_angle = Random.Range(0, 360); //0~360도까지 방향 지정
            float wait_num = Random.Range(min_time, max_time);
            Debug.Log("rot_angle :" + rot_angle + "/ wait_num : " + wait_num);
            trans.Rotate(new Vector3(0, rot_angle, 0));

            yield return new WaitForSeconds(1f);
            Debug.Log("1초 기다림 완료");

            pos_speed = Random.Range(min_force, max_force); //움직임 값 설정
            yield return new WaitForSeconds(wait_num);
            Debug.Log($"{wait_num}초 기다림 완료");

            //rigid.velocity = Vector3.forward * pos_speed;
        }
    }

}

public enum NPC_state
{

}
public class NPC_Movement_YG : MonoBehaviour
{
    [SerializeField] private Rigidbody rigid;
    [SerializeField] private Transform trans;

    [SerializeField] private Transform[] goals;
    [SerializeField] private Transform goal;
    [SerializeField] private float speed;
    [SerializeField] private bool is_;

    private void Awake()
    {
        //컴포넌트 가져오기
        TryGetComponent(out rigid);
        TryGetComponent(out trans);
        goal = trans;
    }

    private void Update()
    {
        if (trans.position == goal.position)
        {
            Debug.Log("목표지점 도달");
            StartCoroutine(Find_posttion());
        }

        Debug.Log("목표로 가는중");
        trans.position = Vector3.MoveTowards(trans.position, goal.position, Time.deltaTime);
    }

    IEnumerator Find_posttion()
    {
        while (true)
        {
            int index = Random.Range(0, goals.Length);
            if (goals[index] != goal)
            {
                goal = goals[index];
                Debug.Log($"이번 목표 / {goal.name}");
                break;
            }
        }
        Debug.Log("코루틴 끝");
        yield return null;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTriggerEnter");
        if (other.CompareTag("tran"))
        {
            rigid.velocity = Vector3.zero;

            StartCoroutine(Find_posttion());
        }
    }

}


