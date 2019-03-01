using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : Controller
{
    // Constructor
    public AIController(State state) : base(state)
    {
    }

    override public Vector2 GetDirection()
    {
        Vector2 decision = GetDecision();
        return decision;
    }

    override public Vector2 GetDecision()
    {
        //-------------------
        // AI DESISION PART (Future work)
        State nextState = state.GetNextState();
        Vector2 head = state.GetHeadPosition();
        Vector2 food = state.GetFoodPosition();
        Vector2 nextHead = nextState.GetHeadPosition();
        Vector2 nextFood = nextState.GetFoodPosition();
        Vector2 direction = state.direction;
        float diff_x = head.x - food.x;
        float diff_y = head.y - food.y;
        Vector2 best_x;
        Vector2 best_y;
        if (diff_x <= 0f)
        {
            best_x = right;
        }
        else
        {
            best_x = left;
        }
        if (diff_y <= 0f)
        {
            best_y = up;
        }
        else
        {
            best_y = down;
        }

        if (diff_x > diff_y)
        {
            if (head.x < food.x)
            {
                if (direction != left)
                {
                    return right;
                }
                else
                {
                    return best_y;
                }
            }
            else
            {
                if (direction != right)
                {
                    return left;
                }
                else
                {
                    return best_y;
                }
            }
        }
        else
        {
            if (head.y < food.y)
            {
                if (direction != down)
                {
                    return up;
                }
                else
                {
                    return best_x;
                }
            }
            else
            {
                if (direction != up)
                {
                    return down;
                }
                else
                {
                    return best_x;
                }
            }
        }
    }

    override public void GetInput()
    {
        // AI no need to get input from player
    }
    
}
