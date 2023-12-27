using System.Collections;
using UnityEngine;
using Mirror;

public class Nonplayer_YG : NetworkBehaviour
{
    // Nonplayer : 랜덤으로 움직이게 하기

    [SerializeField] private Rigidbody rigid;
    [SerializeField] private Transform trans;
    [SerializeField] private Animator ani;

    [SerializeField] private float max_range = 2f;
    [SerializeField] private float min_range = 1;
    
    [SerializeField] private Vector3 goal;
    [SerializeField] private bool can_move;

    private void Awake()
    {
        //컴포넌트 가져오기
        TryGetComponent(out trans);
        TryGetComponent(out ani);
    }

    private void Start()
    {
        StartCoroutine(Find_posttion());
        StartCoroutine(Set_position());
        goal = transform.position;
    }

    IEnumerator Find_posttion()
    {
        float goal_x = Random.Range(min_range, max_range);
        float goal_z = Random.Range(min_range, max_range);

        goal = new Vector3(transform.position.x + goal_x, transform.position.y, transform.position.z +goal_z);
        //Debug.Log($"목표 위치 / {goal}");
        yield return null;
    }

    IEnumerator Set_position()
    {
        while (true)
        {
            //Debug.Log(Vector3.Distance(trans.position, goal));
            if (Vector3.Distance(trans.position, goal) >= 0.75f)
            {
                ani.SetBool("is_walk", true);
                Vector3 tmprot = goal - transform.position;
                tmprot.y = 0;
                tmprot.Normalize();
                transform.rotation = Quaternion.LookRotation(tmprot);
                transform.position = Vector3.MoveTowards(transform.position, goal, Time.deltaTime);
            }

            else
            {
                ani.SetBool("is_walk", false);
                yield return new WaitForSeconds(3f);
                StartCoroutine(Find_posttion());
                break;
            }
            yield return null;
        }
        StartCoroutine(Set_position());
    }
}
