using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    // Public valiable
    public enum Type { None, Head, Body, Food };
    public Vector2 position;
    public Type type;
    public Node head;
    public Node tail;

    // Private valiable
    private SpriteRenderer spriteRenderer;

    // Constructor
    public Node(Type type, Vector2 position)
    {
        this.type = type;
        this.position = position;
    }
}
