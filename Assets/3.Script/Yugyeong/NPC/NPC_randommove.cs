using System.Collections;
using UnityEngine;

    public class NPC_randommove : NPC_YG //랜덤이동하는 NPC
    {
        [Header("Random_move")]
        [SerializeField] private Transform[] goals;
        [SerializeField] private int index;

        public override void Awake()
        {
            base.Awake();
            can_move = false;
            goal = goals[0];
        }

        override public IEnumerator Find_posttion()
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

        override public IEnumerator Set_position()
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
                }
                yield return null;
            }
        }
    }
