using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State
{
    // Variable
    public enum Type { BLANK, HEAD, TAIL, FOOD };
    public Type[,] state;
    public int size;
    public Vector2 direction;
    public List<Vector2> tailList;
    public bool gameover;
    public bool eat;
    public int round;

    // Constructor
    public State(int size, Vector2 head, Vector2 food, Vector2 direction, List<Vector2> tailList, bool noFood, int round)
    {
        // Initial variable
        gameover = false;
        eat = false;
        state = new Type[size, size];
        this.size = size;
        this.direction = direction;
        this.tailList = new List<Vector2>();
        this.round = round;

        // fill Blank 
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                state[i, j] = Type.BLANK;
            }
        }

        // fill Food
        if (noFood == false)
        {
            state[(int)food.x, (int)food.y] = Type.FOOD;
        }

        // Check end food
        if (head == food)
        {
            eat = true;
        }
        else
        {
            eat = false;
            int length = tailList.Count;
            if (length > 0)
            {
                tailList.RemoveAt(length - 1);
            }
        }
        
        // fill Tails
        foreach (Vector2 tail in tailList)
        {
            state[(int)tail.x, (int)tail.y] = Type.TAIL;
            this.tailList.Add(tail);
            if (head == tail)
            {
                gameover = true;
            }
        }

        // fill Head and check leave the field
        if ((int)head.x < 0 || (int)head.x >= size || (int)head.y < 0 || (int)head.y >= size)
        {
            gameover = true;
        }
        else
        {
            state[(int)head.x, (int)head.y] = Type.HEAD;
        }
    }

    // ----------------------------------
    // Method
    //-----------------------------------

    // Get head position in this state
    public Vector2 GetHeadPosition()
    {
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                if (state[i, j] == Type.HEAD)
                {
                    return new Vector2(i, j);
                }
            }
        }
        return new Vector2(-1, -1);
    }

    // Get food position in this state
    public Vector2 GetFoodPosition()
    {
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                if (state[i, j] == Type.FOOD)
                {
                    return new Vector2(i, j);
                }
            }
        }
        return new Vector2(-100, -100);
    }
    
    // Get Blank position list
    public List<Vector2> GetBlankPositionList()
    {
        List<Vector2> list = new List<Vector2>();
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                if (state[i, j] == Type.BLANK)
                {
                    list.Add(new Vector2(i, j));
                }
            }
        }
        return list;
    }
    
    // Get Tail posionn list
    public List<Vector2> GetTailPositionList()
    {
        List<Vector2> list = new List<Vector2>();
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                if (state[i, j] == Type.TAIL)
                {
                    list.Add(new Vector2(i, j));
                }
            }
        }
        return list;
    }

    // Calculate next state from current direction
    public State GetNextState()
    {
        // Prepare variable
        Vector2 head = GetHeadPosition();
        Vector2 food = GetFoodPosition();
        List<Vector2> tailList = CloneList(this.tailList);

        // Process
        tailList.Insert(0, head);
        head.x += direction.x;
        head.y += direction.y;
        return new State(size, head, food, direction, tailList, false, round + 1);
    }

    // Create Clone list
    public List<Vector2> CloneList(List<Vector2> list)
    {
        List<Vector2> clone = new List<Vector2>();
        foreach ( Vector2 item in list)
        {
            clone.Add(item);
        }
        return clone;
    }

    public State CloneState()
    {
        State cloneState = new State(size, GetHeadPosition(), GetFoodPosition(), direction, CloneList(tailList), false, round);
        return cloneState;
    }
    // Add new food to state
    public void AddFood()
    {
        Vector2 old = GetFoodPosition();
        List<Vector2> blankList = GetBlankPositionList();
        int length = blankList.Count;
        int random = Random.Range(0, length);
        Vector2 newFood = blankList[random];
        state[(int)newFood.x, (int)newFood.y] = Type.FOOD;
    }

}
