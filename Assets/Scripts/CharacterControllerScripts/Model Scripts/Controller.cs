using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public abstract class Controller : MonoBehaviour
{
    public abstract void ReadInput(InputData data);

    protected Rigidbody rb;
    protected Collider coll;
    protected Camera pc;
    protected bool newInput;

   
    public virtual void Start()
    {
        rb = GetComponent<Rigidbody>();
        coll = GetComponent<Collider>();
        pc = GameObject.FindGameObjectWithTag("PlayerCamera").GetComponent<Camera>() as Camera;
    }

}
