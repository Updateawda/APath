using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NodeType
{
    walk,
    stop
}
public class AStartNode
{
    public float f;
    public float g;
    public float h;
    public AStartNode father;
    public int x;
    public int y;
    public NodeType type;

    public AStartNode(int x, int y, NodeType type)
    {
        this.x = x;
        this.y = y;
        this.type = type;
    }
}
