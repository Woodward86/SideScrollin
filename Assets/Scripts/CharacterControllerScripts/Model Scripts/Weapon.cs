using UnityEngine;
using UnityEngine.Networking;

public abstract class Weapon : NetworkBehaviour
{

    public abstract void ReadInput(InputData data);

    protected bool newInput;
    protected bool canShoot;

    public virtual void Start()
    {
        if (isLocalPlayer)
        {
            canShoot = true;
        }
    }

}
