using Mirror;
using System.Collections;
using UnityEngine;

public class Nonplayer_data
{
    public string player_id;
    public char is_open;
    public char give_food;
    public char is_solved;

    public Nonplayer_data(string playerid, char isopen, char givefood, char issolved)
    {
        player_id = playerid;
        is_open = isopen;
        give_food = givefood;
        is_solved = issolved;
    }
}

public class Nonplayer_YG : NetworkBehaviour
{
    // Nonplayer : 랜덤으로 움직이게 하기

    [SerializeField] private Rigidbody rigid;
    [SerializeField] private Transform trans;
    [SerializeField] private Animator ani;

    [SerializeField] private float angle;

    [SerializeField] private float max_range = 1f;
    [SerializeField] private float min_range = 0f;

    [SerializeField] private float move_speed = 0.5f;
    [SerializeField] private float max_distance = 7f;

    [SerializeField] private float return_transpeed = 5f; //커질수록 늦게
    [SerializeField] private float return_rotspeed = 5f; //커질수록 늦게

    [SerializeField] private LayerMask layer;
    [SerializeField] private Vector3 goal;

    [SerializeField] private bool is_turn;
    [SerializeField] private bool is_whale;

    [SerializeField] public Study_YG data;

    private void Awake()
    {
        //컴포넌트 가져오기
        trans = GetComponent<Transform>();
        ani = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        goal = trans.parent.GetChild(0).transform.position;
        StartCoroutine(Change());
        ani.SetBool("is_walk", true);
    }

    private void FixedUpdate()
    {
        Move();
        Try_raycast();
    }

    IEnumerator Change()
    {
        float tmp = Random.Range(1, 3);
        while (true)
        {
            angle = Random.Range(0f, 270f);
            if (Vector3.Distance(trans.position, goal) <= max_distance)
            {
                is_turn = true;
            }
            else
            {
                is_turn = false;
            }
            yield return new WaitForSeconds(tmp);
        }
    }

    private void Move()
    {

        if (is_turn)
        {
            trans.Translate(Vector3.forward * move_speed);

            Quaternion tmp = Quaternion.Euler(trans.rotation.x, trans.rotation.y + angle - 180, trans.rotation.z);
            rigid.MoveRotation(Quaternion.Lerp(rigid.rotation, tmp, Time.deltaTime / return_transpeed));
        }

        else
        {
            rigid.MovePosition(Vector3.Lerp(rigid.position, goal, Time.deltaTime / return_transpeed));

            Vector3 direction = (goal - trans.position).normalized;
            Quaternion rot = Quaternion.LookRotation(direction);
            trans.rotation = Quaternion.Lerp(trans.rotation, rot, Time.deltaTime / return_rotspeed);
        }
    }

    private void Try_raycast()
    {
        RaycastHit hit;
        //Debug.DrawRay(transform.position + new Vector3(0, 0.2f, 0), transform.forward + new Vector3(0, 0.2f, 0) * 0.5f, Color.red);
        if (Physics.Raycast(transform.position + new Vector3(0, 0.2f, 0), transform.forward, out hit, 0.8f, layer))
        {
            is_turn = false;
        }
    }
}
