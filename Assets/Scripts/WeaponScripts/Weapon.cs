using UnityEngine;
using UnityEngine.Networking;

public abstract class Weapon : NetworkBehaviour
{

    public abstract void ReadInput(InputData data);

    protected bool newInput;
    protected bool canShoot;

    //weapon settings
    public float shotCooldown = .05f;

    public Transform firePosition;


    public virtual void Start()
    {
        if (isLocalPlayer)
        {
            canShoot = true;
        }
    }

}
