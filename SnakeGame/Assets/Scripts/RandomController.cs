using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomController : Controller
{
    override public Vector2 GetDirection()
    {
        //Vector2 decision = GetDecision();
        return input;
    }

    override public Vector2 GetDecision()
    {
        return input;
    }

    override public void GetInput()
    {
        input = state.direction;
        int random = Random.Range(0, 4);
        if (random == 0 && state.direction != down)
        {
            input = up;
        }
        else if (random == 1 && state.direction != up)
        {
            input = down;
        }
        else if (random == 2 && state.direction != right)
        {
            input = left;
        }
        else if (random == 3 && state.direction != left)
        {
            input = right;
        }
    }
}
