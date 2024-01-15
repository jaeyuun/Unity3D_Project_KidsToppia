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

    [SerializeField] private float horizontal;
    [SerializeField] private float vertical;
    [SerializeField] private float angle;

    [SerializeField] private float max_range = 1f;
    [SerializeField] private float min_range = 0f;
    [SerializeField] private float move_speed = 1f;
    [SerializeField] private float max_distance = 2f;
    [SerializeField] private LayerMask layer;
    [SerializeField] private Vector3 goal;
    [SerializeField] private bool is_stop;
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
        StartCoroutine(Input_change());
        is_stop = true;
    }

    private void FixedUpdate()
    {
        Test();
        Try_raycast();
    }

    IEnumerator Input_change()
    {
        float tmp = Random.Range(1, 6);
        while (true)
        {
            horizontal = Random.Range(0.5f, 1f);
            vertical = Random.Range(0.5f, 1f);
            angle = Random.Range(0f, 270f);

            yield return new WaitForSeconds(tmp);

            horizontal = 0f;
            vertical = 0f;

            yield return new WaitForSeconds(1f);
        }
    }

    private void Test()
    {
        if (Vector3.Distance(trans.position, goal) <= max_distance)
        {
            rigid.velocity = new Vector3(horizontal * move_speed, rigid.velocity.y, vertical * move_speed);

            if (horizontal != 0 || vertical != 0)
            {
                ani.SetBool("is_walk", true);
                trans.rotation = Quaternion.Euler(trans.rotation.x, trans.rotation.y + angle, trans.rotation.z);
                //trans.rotation = Quaternion.LookRotation(new Vector3(rigid.velocity.x, rigid.velocity.y, rigid.velocity.z));
            }

            else
            {
                ani.SetBool("is_walk", false);
            }
        }
        else
        {
            rigid.MovePosition(Vector3.Lerp(rigid.position, goal, Time.deltaTime / 2));
        }

    }



    private void Try_raycast()
    {
        RaycastHit hit;
        Debug.DrawRay(transform.position + new Vector3(0, 0.2f, 0), transform.forward + new Vector3(0, 0.2f, 0) * 0.5f, Color.red);
        if (Physics.Raycast(transform.position + new Vector3(0, 0.2f, 0), transform.forward, out hit, 0.5f,layer))
        {
            if (is_stop)
            {
                is_stop = false;

                horizontal = 0;
                vertical = 0;

                //랜덤 애니메이션 출력

                Invoke("Reset_Input", 1f);
                float tmp = Random.Range(45,180);
                trans.rotation = Quaternion.Euler(trans.rotation.x, trans.rotation.y + tmp, trans.rotation.z);

            }
        }
    }

    private void Reset_Input()
    {
        is_stop = true;
        Debug.Log("Reset_Input");
        horizontal = Random.Range(-1f, 1f);
        vertical = Random.Range(-1f, 1f);
    }

    IEnumerator Find_posttion()
    {
        float goal_x = Random.Range(min_range, max_range);
        float goal_z = Random.Range(min_range, max_range);
        float tmp = Random.Range(0, 2);

        if (tmp == 0)
        {
            goal = new Vector3(transform.position.x + goal_x, transform.position.y, transform.position.z + goal_z);
        }

        else
        {
            goal = new Vector3(transform.position.x - goal_x, transform.position.y, transform.position.z - goal_z);
        }

        yield return null;
    }

    IEnumerator Set_position()
    {
        while (true)
        {
            //Debug.Log(Vector3.Distance(trans.position, goal));
            if (Vector3.Distance(trans.position, goal) >= 0.5f)
            {
                ani.SetBool("is_walk", true);
                Vector3 tmprot = goal - transform.position;
                tmprot.y = 0;
                tmprot.Normalize();
                horizontal =0;
                transform.rotation = Quaternion.LookRotation(tmprot);
                transform.position = Vector3.MoveTowards(transform.position, goal, Time.deltaTime);
            }

            else
            {

                //Play_ani();
                yield return new WaitForSeconds(3f);
                StartCoroutine(Find_posttion());
                break;
            }
            yield return null;
        }
        StartCoroutine(Set_position());
    }

    private void Play_ani()
    {
        int ran = Random.Range(1, 2);
        if (ran == 1)
        {
            ani.SetTrigger("State1");
        }
        else
        {
            return;
        }
    }
}
