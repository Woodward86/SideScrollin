using UnityEngine;
using UnityEngine.Networking;

public abstract class Weapon : NetworkBehaviour
{

    public abstract void ReadInput(InputData data);

    protected bool newInput;

    public virtual void Start()
    {
        
    }
}
