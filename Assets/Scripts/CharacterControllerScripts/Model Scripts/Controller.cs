using UnityEngine;


[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public abstract class Controller : MonoBehaviour
{
    public abstract void ReadInput(InputData data);

    protected Rigidbody rb;
    protected Collider bColl;
    protected Collider cColl;
    protected Camera pc;
    protected bool newInput;

   
    public virtual void Start()
    {
        rb = GetComponent<Rigidbody>();
        bColl = GetComponent<Collider>();
        cColl = GetComponent<Collider>();
        pc = GameObject.FindGameObjectWithTag("PlayerCamera").GetComponent<Camera>() as Camera;
    }

}
