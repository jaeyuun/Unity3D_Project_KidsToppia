using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public bool walkable; // 레이어로 판단
    public Vector3 worldPosition; // node 위치
    public int gridX; // node x 좌표
    public int gridY; // node y 좌표

    public Node parent;

    public int gCost; // startNode부터 currentNode까지의 비용
    public int hCost; // currentNode 부터 endNode까지의 예상 비용

    public int fCost // A*
    {
        get
        {
            return gCost + hCost;
        }
    }

    public Node(bool _walkable, Vector3 _worldPos, int _gridX, int _gridY)
    {
        walkable = _walkable;
        worldPosition = _worldPos;
        gridX = _gridX;
        gridY = _gridY;
    }
}
