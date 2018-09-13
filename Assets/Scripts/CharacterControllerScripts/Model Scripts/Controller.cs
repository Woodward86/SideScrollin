using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(FacingController))]
public abstract class Controller : NetworkBehaviour
{

    public abstract void ReadInput(InputData data);

    protected Rigidbody rb;
    protected Collider coll;
    protected FacingController fc;
    protected Camera pc;
    protected bool newInput;

   
    public virtual void Start()
    {
        if (!isLocalPlayer)
        {
            Destroy(this);
            return;
        }

        rb = GetComponent<Rigidbody>();
        coll = GetComponent<Collider>();
        fc = GetComponent<FacingController>();
        pc = GameObject.FindGameObjectWithTag("PlayerCamera").GetComponent<Camera>() as Camera;
    }

}
