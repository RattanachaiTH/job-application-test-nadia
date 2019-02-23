using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{
    public Vector3 position;
    public GameManager gameManager;

    Vector2 direction;
    Vector2 direction_temp;
    Vector2 up, down, right, left;
    float timer;
    bool move;
    bool alive;

    // Start is called before the first frame update
    void Start()
    {
        //gameManager = gameManamgerObj.transform.GetComponent<GameManager>();
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
        if (gameManager.status == true)
        {
            CheckDirection();
            if (move == true)
            {
                Node nextNode = CheckNextNode(gameManager.state);
                if (alive)
                {
                    // Next node is Food
                    if (nextNode == gameManager.nodeFood)
                    {
                        gameManager.UpdateSnake(position, nextNode, true);
                    }
                    else
                    {
                        gameManager.UpdateSnake(position, nextNode, false);
                    }
                    move = false;
                }
                else
                {
                    gameManager.status = false;
                }
            }
        }
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
        if (timer > gameManager.speedRate)
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
        if (position.x < 0 || position.x >= gameManager.size || position.y < 0 || position.y >= gameManager.size)
        {
            alive = false;
            return null;
        }
        else
        {
            return state[(int)position.x, (int)position.y];
        }
    }
}
