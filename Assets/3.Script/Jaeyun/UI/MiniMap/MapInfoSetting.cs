using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapInfoSetting : MonoBehaviour
{
    [SerializeField] private PathFinding[] pathFinding;

    private void Awake()
    {
        pathFinding = FindObjectsOfType<PathFinding>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            for (int i = 0; i < pathFinding.Length; i++)
            {
                pathFinding[i].isFinding = false;
            }
        }
    }
}
