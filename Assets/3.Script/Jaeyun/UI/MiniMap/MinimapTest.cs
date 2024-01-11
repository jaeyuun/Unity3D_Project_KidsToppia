using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapTest : MonoBehaviour
{
    private PathFinding pathFinding;
    private void Start()
    {
        pathFinding = FindObjectOfType<PathFinding>();
        pathFinding.playerObject = gameObject;
        pathFinding.player = gameObject.transform;
    }
}
