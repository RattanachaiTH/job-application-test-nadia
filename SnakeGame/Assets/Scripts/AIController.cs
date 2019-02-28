using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : Controller
{
    override public Vector2 GetDirection()
    {
        Vector2 decision = GetDecision();
        return decision;
    }

    override public Vector2 GetDecision()
    {
        //-------------------
        // AI DESISION PART (Future work)
        //-------------------
        return Vector2.zero;
    }

    override public void GetInput()
    {
        // AI no need to get input from player
    }
}
