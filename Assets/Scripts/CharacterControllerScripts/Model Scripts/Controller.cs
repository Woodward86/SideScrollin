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
    protected bool newInput;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        coll = GetComponent<Collider>();
        fc = GetComponent<FacingController>();

    }

}
