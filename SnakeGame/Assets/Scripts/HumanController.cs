using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanController : Controller
{
    // Constructor
    public HumanController(State state) : base(state)
    {
    }

    override public Vector2 GetDirection()
    {
        // Human no need to calculate decision
        // Vector2 decision = GetDecision();
        return input;
    }

    override public Vector2 GetDecision()
    {
        // Human no need to calculate decision
        return Vector2.zero;
    }

    override public void GetInput()
    {
        // Get input from player
        input = state.direction;
        if ((Input.GetButton("Up")) && state.direction != down)
        {
            input = up;
        }
        if (Input.GetButton("Down") && state.direction != up)
        {
            input = down;
        }
        if (Input.GetButton("Left") && state.direction != right)
        {
            input = left;
        }
        if (Input.GetButton("Right") && state.direction != left)
        {
            input = right;
        }
    }
}
