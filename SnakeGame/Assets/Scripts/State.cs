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

    // Constructor
    public State(int size, Vector2 head, Vector2 food, Vector2 direction, List<Vector2> tailList)
    {
        // Initial variable
        gameover = false;
        eat = false;
        state = new Type[size, size];
        this.size = size;
        this.direction = direction;
        this.tailList = new List<Vector2>();

        // fill Blank 
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                state[i, j] = Type.BLANK;
            }
        }
        // fill Head and Food
        state[(int)food.x, (int)food.y] = Type.FOOD;

        if (head == food)
        {
            eat = true;
        }
        else
        {
            eat = false;
            tailList.RemoveAt(tailList.Count - 1);
        }
        
        // fill Tails
        foreach (Vector2 tail in tailList)
        {
            state[(int)tail.x, (int)tail.y] = Type.TAIL;
            tailList.Add(tail);
            if (head == tail)
            {
                gameover = true;
            }
        }

        // fill Head and check leave the field
        if ((int)head.x < 0 || (int)head.x >= GameManager.size || (int)head.y < 0 || (int)head.y >= GameManager.size)
        {
            gameover = false;
        }
        else
        {
            state[(int)head.x, (int)head.y] = Type.HEAD;
        }
    }

    // Method
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
        return Vector2.zero;
    }

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
        return Vector2.zero;
    }
    
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

    public State GetNextState()
    {
        // Prepare variable
        Vector2 head = GetHeadPosition();
        Vector2 food = GetFoodPosition();
        List<Vector2> tailList = CloneList(this.tailList);

        // Process
        head.x = direction.x;
        head.y = direction.y;
        tailList.Insert(0, head);
        return new State(size, head, food, direction, tailList);
    }

    public List<Vector2> CloneList(List<Vector2> list)
    {
        List<Vector2> clone = new List<Vector2>();
        foreach ( Vector2 item in list)
        {
            clone.Add(item);
        }
        return clone;
    }


}
