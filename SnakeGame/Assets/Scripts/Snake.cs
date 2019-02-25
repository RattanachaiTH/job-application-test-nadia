using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{
    public GameManager gameManager;
    
    private Vector3 position;
    Vector2 direction;
    Vector2 direction_temp;
    Vector2 up, down, right, left;
    float timer;
    bool move;
    bool alive;
    
    void Awake()
    {
        // Initial variable
        up = new Vector2(0, 1);
        down = new Vector2(0, -1);
        right = new Vector2(1, 0);
        left = new Vector2(-1, 0);
        direction_temp = right;
        move = false;
        alive = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (gameManager.GetStatus() == true)
        {
            CheckDirection();
            if (move == true)
            {
                Node nextNode = CheckNextNode(gameManager.GetState());
                if (alive)
                {
                    // Next node is Food
                    if (nextNode == gameManager.GetFoodNode())
                    {
                        gameManager.UpdateState(position, nextNode, true);
                    }
                    else
                    {
                        gameManager.UpdateState(position, nextNode, false);
                    }
                    move = false;
                }
                else
                {
                    gameManager.GameOver();
                    gameManager.SetStatus(false);
                }
            }
        }
    }

    public void SetPosition(Vector2 position)
    {
        this.position = position;
    }

    void CheckDirection()
    {
        if ((Input.GetButton("Up")) && direction != down)
        {
            direction_temp = up;
        }
        if (Input.GetButton("Down") && direction != up)
        {
            direction_temp = down;
        }
        if (Input.GetButton("Left") && direction != right)
        {
            direction_temp = left;
        }
         if (Input.GetButton("Right") && direction != left)
        {
            direction_temp = right;
        }

        Timer();
    }

    void Timer()
    {
        timer += Time.deltaTime;
        if (timer > GameManager.speedRate)
        {
            timer = 0;
            move = true;
        }
    }

    Node CheckNextNode(Node[,] state)
    {
        direction = direction_temp;
        position.x += direction.x;
        position.y += direction.y;
        if (position.x < 0 || position.x >= GameManager.size || position.y < 0 || position.y >= GameManager.size)
        {
            alive = false;
            return null;
        }
        foreach (Node node in gameManager.GetTailList())
        {
            if (node.position.x == position.x && node.position.y == position.y)
            {
                alive = false;
                return null;
            }
        }
        return state[(int)position.x, (int)position.y];
    }
}
