using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapInfoSetting : MonoBehaviour
{
    [SerializeField] private PathFinding pathFinding;

    private void Awake()
    {
        pathFinding = FindObjectOfType<PathFinding>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            pathFinding.isFinding = false;
        }
    }
}
