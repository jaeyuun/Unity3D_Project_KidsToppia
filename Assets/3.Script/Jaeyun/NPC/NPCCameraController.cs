using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCCameraController : MonoBehaviour
{
    public Transform npc;
    [SerializeField] private float disY = 0.1f;
    [SerializeField] private float disZ = 1.5f;

    [SerializeField] private float offsetY = 0.5f;


    private void Update()
    {
        MoveCamera();
    }

    private void MoveCamera()
    {
        if (TalkManager.instance.npc != null)
        {
            npc = TalkManager.instance.npc.transform;
        }
        if (npc != null)
        {
            Vector3 npcPos = npc.position + new Vector3(0, offsetY, 0);
            Vector3 lookAt = npcPos - transform.position;
            lookAt = lookAt.normalized;
            transform.position = npcPos + npc.right * 0 + npc.up * disY + npc.forward * disZ;
            transform.rotation = Quaternion.LookRotation(lookAt);
            
        }
    }
}
