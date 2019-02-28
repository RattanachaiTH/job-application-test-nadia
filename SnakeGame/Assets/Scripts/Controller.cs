using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Controller
{
    
    public Vector2 currentDirection;
    public Vector2 input;
    public Vector2 up;
    public Vector2 down;
    public Vector2 left;
    public Vector2 right;

    public Controller()
    {
        up = new Vector2(0, 1);
        down = new Vector2(0, -1);
        right = new Vector2(1, 0);
        left = new Vector2(-1, 0);
        
    }
    public abstract Vector2 GetDirection();
    public abstract Vector2 GetDicition();
    public abstract void GetInput();
}
