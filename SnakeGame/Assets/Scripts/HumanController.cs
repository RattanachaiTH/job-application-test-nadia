using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanController : Controller
{
    override public Vector2 GetDirection()
    {
        input = GetDicition();
        return input;
    }

    override public Vector2 GetDicition()
    {
        return Vector2.zero;
    }

    override public void GetInput()
    {
        if ((Input.GetButton("Up")) && tempDirection != down)
        {
            input = up;
        }
        if (Input.GetButton("Down") && tempDirection != up)
        {
            input = down;
        }
        if (Input.GetButton("Left") && tempDirection != right)
        {
            input = left;
        }
        if (Input.GetButton("Right") && tempDirection != left)
        {
            input = right;
        }
    }
}
