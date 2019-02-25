using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ARManager : MonoBehaviour
{
    // Public valiable
    public GameObject target1;
    public GameObject target2;
    public GameObject bin;
    public float swapTime;

    // Private valiable
    private DefaultTrackableEventHandler handler1;    // Vuforia components
    private DefaultTrackableEventHandler handler2;
    private bool tracked1;
    private bool tracked2;
    private bool swapWaiting;
    private float timeCount;
    private float sphereScale;
    private float sphereDistant;
    private int count;
    
    void Awake()
    {
        // initial valiable
        handler1 = target1.transform.GetComponent<DefaultTrackableEventHandler>();
        handler2 = target2.transform.GetComponent<DefaultTrackableEventHandler>();
        swapWaiting = false;
        sphereScale = 15f;
        sphereDistant = 5f;
        count = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        UpdateHandler();
        CheckLost();
        CheckFound();
        Swapping();
    }

    // Get tracking value from Vuforia
    void UpdateHandler()
    {
        tracked1 = handler1.tracked;
        tracked2 = handler2.tracked;
    }

    // Check Lost any tracked image and Move it to bin object
    void CheckLost()
    {
        if (target1.transform.childCount != 0 && tracked1 == false)
        {
            target1.transform.GetChild(0).parent = bin.transform;
        }
        if (target2.transform.childCount != 0 && tracked2 == false)
        {
            target2.transform.GetChild(0).parent = bin.transform;
        }
    }

    // Check found any tracked image and create new sphere object
    void CheckFound()
    {
        if (target1.transform.childCount == 0 && tracked1 == true)
        {
            CreateSphere(target1);
        }
        if (target2.transform.childCount == 0 && tracked2 == true)
        {
            CreateSphere(target2);
        }
    }

    // Swapping between two spheres after swapTime
    void Swapping()
    {
        if (tracked1 == true && tracked2 == true)
        {
            if (swapWaiting == true)
            {
                timeCount += Time.deltaTime;
                // When swapTime pass
                if (timeCount > swapTime)
                {
                    target1.transform.GetChild(0).position = GetTargetPosition(target2);
                    target1.transform.GetChild(0).parent = target2.transform;
                    target2.transform.GetChild(0).position = GetTargetPosition(target1);
                    target2.transform.GetChild(0).parent = target1.transform;
                    timeCount = 0f;
                }
            }
            else
            {
                // First time for tracking two spheres
                swapWaiting = true;
                timeCount = 0f;
            }
        }
        else
        {
            // When lost some tracking
            swapWaiting = false;
            timeCount = 0f;
        }
    }

    // Function for creating new sphere
    void CreateSphere(GameObject target)
    {
        count++;
        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.transform.name = "Sphere" + count;
        sphere.transform.localScale = new Vector3(sphereScale, sphereScale, sphereScale);
        sphere.transform.position = GetTargetPosition(target);
        sphere.transform.parent = target.transform;
        sphere.GetComponent<Renderer>().material.shader = Shader.Find("Transparent/Diffuse");
        sphere.GetComponent<Renderer>().material.SetColor("_Color", Random.ColorHSV());
    }

    // Function for setting a sphere position
    Vector3 GetTargetPosition(GameObject target)
    {
        float x = target.transform.position.x + (sphereDistant * target.transform.up.x);
        float y = target.transform.position.y + (sphereDistant * target.transform.up.y);
        float z = target.transform.position.z + (sphereDistant * target.transform.up.z);
        Vector3 targetPosition = new Vector3(x, y, z);
        return targetPosition;
    }
}
