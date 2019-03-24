﻿using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class AIController : Controller
{
    private int maxDepth;
    private int reward;
    private int punish;
    // Constructor
    public AIController(State state) : base(state)
    {
        maxDepth = 3;
        reward = 100;
        punish = Int32.MinValue;
    }

    override public Vector2 GetDirection()
    {
        //Vector2 decision = GetDecision();
        Vector2 decision = GetDirectionFromAlphaValue();
        //Debug.Log("Distance: " + GetDistance(state));
        return decision;
    }

    override public Vector2 GetDecision()
    {
        //-------------------

        return GetDirectionFromBest(state);
        
    }

    override public void GetInput()
    {
        // AI no need to get input from player
    }
    
    private Vector2[] GetBestInEachAxis(State state)
    {
        Vector2[] best = new Vector2[2];
        Vector2 head = state.GetHeadPosition();
        Vector2 food = state.GetFoodPosition();
        Vector2 best_x;
        Vector2 best_y;
        float diff_x = head.x - food.x;
        float diff_y = head.y - food.y;

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

        best[0] = best_x;
        best[1] = best_y;
        return best;
    }
    
    // Get Direction from best axis
    private Vector2 GetDirectionFromBest(State state)
    {
        Vector2[] best = GetBestInEachAxis(state);
        Vector2 head = state.GetHeadPosition();
        Vector2 food = state.GetFoodPosition();
        float diff_x = head.x - food.x;
        float diff_y = head.y - food.y;
        if (diff_x > diff_y)
        {
            if (head.x < food.x)
            {
                if (state.direction != left)
                {
                    return right;
                }
                else
                {
                    return best[1];
                }
            }
            else
            {
                if (state.direction != right)
                {
                    return left;
                }
                else
                {
                    return best[1];
                }
            }
        }
        else
        {
            if (head.y < food.y)
            {
                if (state.direction != down)
                {
                    return up;
                }
                else
                {
                    return best[0];
                }
            }
            else
            {
                if (state.direction != up)
                {
                    return down;
                }
                else
                {
                    return best[0];
                }
            }
        }
    }
    private Vector2 AI1()
    {
        List<Vector2> posibleDirection = GetPosibleMoveList(state);
        Vector2 bestDirection = posibleDirection[0];
        int maxValue = Int32.MinValue;
        for (int i = 0; i < posibleDirection.Count; i++)
        {
            State subState = state.CloneState();
            subState.direction = posibleDirection[i];
            subState = subState.GetNextState();
            int value = 0;
            Vector2 best = GetDirectionFromBest(subState);
            if (posibleDirection[i] == best)
            {
                value += reward;
            }
            if (subState.gameover == true)
            {
                value += punish;
            }
            Debug.Log("value " + i + ": " + value);
            if (value > maxValue)
            {
                maxValue = value;
                bestDirection = posibleDirection[i];
            }
        }
        return bestDirection;
    }

    private Vector2 GetDirectionFromAlphaValue()
    {
        List<Vector2> posibleDirection = GetPosibleMoveList(state);
        Vector2 bestDirection = posibleDirection[0];
        int maxValue = Int32.MinValue;
        for (int i = 0; i < posibleDirection.Count; i++)
        {
            State subState = state.CloneState();
            subState.direction = posibleDirection[i];
            int value = GetAlphaValue(subState.GetNextState(), 0, 0);
            Debug.Log("value " + i + ": " + value);
            if (value > maxValue)
            {
                maxValue = value;
                bestDirection = posibleDirection[i];
            }
        }
        return bestDirection;
    }

    private int GetAlphaValue(State state, int alpha, int depth)
    {
        if (state.gameover == true)
        {
            return punish;
        }
        else if (depth >= maxDepth)
        {
            return alpha;
        }
        else if (state.eat == true)
        {
            return reward;
        }
        if (state.direction == GetDirectionFromBest(state))
        {
            alpha += state.size / 2;
        }
        alpha += state.size - GetDistance(state);
        List<Vector2> posibleDirection = GetPosibleMoveList(state);
        int maxValue = Int32.MinValue;
        for (int i = 0; i < posibleDirection.Count; i++)
        {
            State subState = state.CloneState();
            subState.direction = posibleDirection[i];
            int value = GetAlphaValue(subState.GetNextState(), alpha, depth + 1);
            if (value > maxValue)
            {
                maxValue = value;
            }
        }
        return alpha;
    }

    private int GetDistance(State state)
    {
        Vector2 head = state.GetHeadPosition();
        Vector2 food = state.GetFoodPosition();
        int distance = (int)Math.Sqrt(Math.Pow((food.x - head.x), 2) + Math.Pow((food.y - head.y), 2));
        return distance;
    }
}
