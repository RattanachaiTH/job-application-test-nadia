using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Controller
{
    // Initial variable
    public State state;
    public Vector2 input;
    public Vector2 up;
    public Vector2 down;
    public Vector2 left;
    public Vector2 right;

    // Constructor
    public Controller(State state)
    {
        up = new Vector2(0, 1);
        down = new Vector2(0, -1);
        right = new Vector2(1, 0);
        left = new Vector2(-1, 0);
        this.state = state;
    }

    public void SetState(State state)
    {
        this.state = state;
    }

    public List<Vector2> GetPosibleMoveList(State state)
    {
        List<Vector2> list = new List<Vector2>();
        Vector2 stateDirection = state.direction;
        list.Add(up);
        list.Add(down);
        list.Add(right);
        list.Add(left);
        list.Remove(stateDirection * new Vector2(-1, -1));
        return list;
    }

    // Get direction for next state
    public abstract Vector2 GetDirection();

    // Calculate decision
    public abstract Vector2 GetDecision();

    // Get input from player
    public abstract void GetInput();
}
