using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    // Public valiable
    public Vector2 position;
    public Node head;
    public Node tail;

    // Private valiable
    private SpriteRenderer spriteRenderer;

    // Constructor
    public Node(Vector2 position)
    {
        this.position = position;
    }
}
