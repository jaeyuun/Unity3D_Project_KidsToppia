using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCLookAt : MonoBehaviour
{
    public Transform player = null;

    private void Update()
    {
        NPCLookAtPlayer();
    }

    private void NPCLookAtPlayer()
    {
        if (player != null)
        {
            Vector3 lookPlayer = player.position - transform.position;
            transform.rotation = Quaternion.LookRotation(lookPlayer);
        }
    }
}
