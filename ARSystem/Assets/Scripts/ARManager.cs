using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ARManager : MonoBehaviour
{
    public GameObject target1;
    public GameObject target2;
    public float swapTime;

    private DefaultTrackableEventHandler handler1;
    private DefaultTrackableEventHandler handler2;
    private GameObject sphere1;
    private GameObject sphere2;
    private bool tracked1;
    private bool tracked2;
    private bool swap;
    private float timeCount;

    void Awake()
    {
        // Get Components
        handler1 = target1.transform.GetComponent<DefaultTrackableEventHandler>();
        handler2 = target2.transform.GetComponent<DefaultTrackableEventHandler>();
        swap = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        UpdateTracking();
        RemoveLost();
        CheckTracked();
    }

    void UpdateTracking()
    {
        tracked1 = handler1.tracked;
        tracked2 = handler2.tracked;
    }

    void RemoveLost()
    {
        if (sphere1 != null && tracked1 == false)
        {
            Destroy(sphere1);
        }
        if (sphere2 != null && tracked2 == false)
        {
            Destroy(sphere2);
        }
    }

    void CheckTracked()
    {
        if (sphere1 == null && tracked1 == true)
        {
            sphere1 = CreateSphere(target1);
        }
        if (sphere2 == null && tracked2 == true)
        {
            sphere2 = CreateSphere(target2);
        }
    }

    void Swapping()
    {
        if (tracked1 == true && tracked2 == true)
        {
            if (swap == true)
            {
                timeCount += Time.deltaTime;
                if (timeCount > swapTime)
                {
                    sphere1.transform.parent = target2.transform;
                    sphere2.transform.parent = target1.transform;
                }
            }
            else
            {
                swap = true;
                timeCount = 0f;
            }
        }
        else
        {
            swap = false;
        }
    }

    GameObject CreateSphere(GameObject target)
    {

        float scale = 15f;
        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.transform.localScale = new Vector3(scale, scale, scale);
        sphere.transform.position = target.transform.position;
        sphere.transform.parent = target.transform;
        sphere.GetComponent<Renderer>().material.shader = Shader.Find("Transparent/Diffuse");
        sphere.GetComponent<Renderer>().material.SetColor("_Color", Random.ColorHSV());
        return sphere;
    }
}
