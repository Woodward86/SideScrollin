using UnityEngine;
using UnityEngine.Networking;

public class PrefabWeapon : Weapon
{

    public GameObject bulletPrefab;

    float ellapsedTime;

    public override void ReadInput(InputData data)
    {
        if (!canShoot)
            return;

        ellapsedTime += Time.deltaTime;

        if (data.buttons[2] == true && ellapsedTime > shotCooldown)
        {
            ellapsedTime = 0f;
            CmdShoot();
        }

        newInput = true;
    }


    [Command]
    void CmdShoot()
    {
        Instantiate(bulletPrefab, firePosition.position, firePosition.rotation);
    }


    void LateUpdate()
    {
        if (!newInput)
        {
            ResetShooting();
        }

        newInput = false;
    }


    void ResetShooting()
    {
        ellapsedTime = 0f;
    }
}
