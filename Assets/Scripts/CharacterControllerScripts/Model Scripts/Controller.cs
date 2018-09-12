using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(FacingController))]
public abstract class Controller : MonoBehaviour
{

    //TODO: Add method ReadInput
    public abstract void ReadInput(InputData data);

    protected Rigidbody rb;
    protected Collider coll;
    protected FacingController fc;
    protected Camera pc;
    protected bool newInput;

    protected bool started;

    void Awake()
    {
        started = true;
        rb = GetComponent<Rigidbody>();
        coll = GetComponent<Collider>();
        fc = GetComponent<FacingController>();
        pc = GameObject.FindGameObjectWithTag("PlayerCamera").GetComponent<Camera>() as Camera;

        //TODO: need to move this to a GameState controller
        Camera.main.enabled = false;
        pc.enabled = true;
    }
}
