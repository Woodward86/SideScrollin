using UnityEngine;
using UnityEngine.Networking;

public class TestWeapon : Weapon
{

    public float shotCooldown = .1f;
    public Transform firePosition;

    bool canShoot;
    float ellapsedTime;

    public override void Start()
    {
        base.Start();

        if (isLocalPlayer)
        {
            canShoot = true;
        }
    }

    public override void ReadInput(InputData data)
    {
        if (!canShoot)
            return;

        ellapsedTime += Time.deltaTime;

        if (data.buttons[2] == true)
        {
            Debug.Log("shooting");
            ellapsedTime = 0f;
            CmdFireShot(firePosition.position, firePosition.right);
        }

        newInput = true;
    }


    void LateUpdate()
    {
        if (!newInput)
        {
            ResetShooting();
        }
                
        newInput = false;
    }


    [Command]
    void CmdFireShot(Vector3 origin, Vector3 direction)
    {
        RaycastHit hit;

        Ray ray = new Ray(origin, direction);
        Debug.DrawRay(ray.origin, ray.direction * 3f, Color.red, 1f);

        bool result = Physics.Raycast(ray, out hit, 50f);

        if (result)
        {
            //health stuff
            Debug.Log("hit something");
        }
    }


    void ResetShooting()
    {
        ellapsedTime = 0f;
    }
}
