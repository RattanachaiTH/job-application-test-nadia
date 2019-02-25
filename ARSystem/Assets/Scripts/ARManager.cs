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
    public  GameObject test;
    private bool tracked1;
    private bool tracked2;
    private bool swapWaiting;
    private bool swaped;
    private float timeCount;
    private int count;

    void Awake()
    {
        // Get Components
        test = null;
        handler1 = target1.transform.GetComponent<DefaultTrackableEventHandler>();
        handler2 = target2.transform.GetComponent<DefaultTrackableEventHandler>();
        swapWaiting = false;
        swaped = false;
        swapTime = 2f;
        count = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        UpdateTracking();
        RemoveLost();
        CheckTracked();
        Swapping();
        //print("target1: " + tracked1 + ", target2: " + tracked2 + ", swaped: " + swaped);
        string s1 = "None";
        string s2 = "None";
        if (sphere1 != null)
        {
            s1 = sphere1.transform.name;
        }
        if (sphere2 != null)
        {
            s2 = sphere2.transform.name;
        }
        print("Sphere1: " + s1 + ", Sphere2: " + s2);
    }

    void UpdateTracking()
    {
        tracked1 = handler1.tracked;
        tracked2 = handler2.tracked;
    }

    void RemoveLost()
    {
        if (tracked1 == true && tracked2 == false)
        {
            if (sphere2 != null)
            {
                Destroy(sphere2);
                if (swaped == true)
                {
                    sphere1.transform.position = target1.transform.position;
                    sphere1.transform.parent = target1.transform;
                    EnableRenderer(sphere1);
                }
            }

            swaped = false;
        }
        else if (tracked1 == false && tracked2 == true)
        {
            if (sphere1 != null)
            {

                Destroy(sphere1);
                if (swaped == true)
                {
                    sphere2.transform.position = target2.transform.position;
                    sphere2.transform.parent = target2.transform;
                    EnableRenderer(sphere2);
                }
            }

            swaped = false;
        }
        /*
        if (tracked1 == true && tracked2 == false)
        {
            if (sphere2 != null)
            {
                Destroy(sphere2);
                if (swaped == true)
                {
                    sphere1.transform.position = target1.transform.position;
                    sphere1.transform.parent = target1.transform;
                    EnableRenderer(sphere1);
                }
            }

            swaped = false;
        }
        else if (tracked1 == false && tracked2 == true)
        {
            if (sphere1 != null)
            {

                Destroy(sphere1);
                if (swaped == true)
                {
                    sphere2.transform.position = target2.transform.position;
                    sphere2.transform.parent = target2.transform;
                    EnableRenderer(sphere2);
                }
            }

            swaped = false;
        }
        */
        else if (tracked1 == false && tracked2 == false)
        {
            if (sphere1 != null)
            {
                Destroy(sphere1);
            }
            if (sphere2 != null)
            {
                Destroy(sphere2);
            }
            swaped = false;
        }
    }

    void CheckTracked()
    {
        /*
        if (swaped == true)
        {
            if (sphere1 == null && tracked2 == true)
            {
                sphere1 = CreateSphere(target2);
            }
            if (sphere2 == null && tracked1 == true)
            {
                sphere2 = CreateSphere(target1);
            }
        }
        else
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
        */
        
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
            if (swapWaiting == true)
            {
                timeCount += Time.deltaTime;
                if (timeCount > swapTime)
                {
                    if (swaped == true)
                    {
                        sphere2.transform.position = target2.transform.position;
                        sphere2.transform.parent = target2.transform;
                        sphere1.transform.position = target1.transform.position;
                        sphere1.transform.parent = target1.transform;
                    }
                    else
                    {
                        sphere1.transform.position = target2.transform.position;
                        sphere1.transform.parent = target2.transform;
                        sphere2.transform.position = target1.transform.position;
                        sphere2.transform.parent = target1.transform;
                    }
                    swaped = Crossing(swaped);
                    swapWaiting = false;
                    timeCount = 0f;
                }
            }
            else
            {
                swapWaiting = true;
                timeCount = 0f;
            }
        }
        else
        {
            swapWaiting = false;
            timeCount = 0f;
        }
    }

    GameObject CreateSphere(GameObject target)
    {
        count++;
        float scale = 15f;
        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.transform.name = "Sphere" + count;
        sphere.transform.localScale = new Vector3(scale, scale, scale);
        sphere.transform.position = target.transform.position;
        sphere.transform.parent = target.transform;
        sphere.GetComponent<Renderer>().material.shader = Shader.Find("Transparent/Diffuse");
        sphere.GetComponent<Renderer>().material.SetColor("_Color", Random.ColorHSV());
        return sphere;
    }

    bool Crossing(bool cossing)
    {
        if (cossing == true)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    void EnableRenderer(GameObject obj)
    {
        obj.transform.GetComponent<SphereCollider>().enabled = true;
        obj.transform.GetComponent<MeshRenderer>().enabled = true;

    }
}
