using UnityEngine;
using UnityEngine.Networking;

public class PlayerShooting : NetworkBehaviour
{
    [SerializeField] float shotCooldown = .3f;
    [SerializeField] Transform firePosition;

    float ellapsedTime;
    bool canShoot;


    void Start()
    {
        //TODO: initialize some shot FX

        if (isLocalPlayer)
        {
            canShoot = true;
        }
            
    }


    void Update()
    {

        if (!canShoot)
        {
            return;
        }

        ellapsedTime += Time.deltaTime; 

        //TODO: going to need to make this work with InputManager or ReadInput method from controller class
        //if (/*get fire button down && ellapsedTime > shotCooldown */)
        //{
        //    ellapsedTime = 0f;
        //}

    }
}
