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
        GameObject bulletInstance = Instantiate(bulletPrefab, firePosition.position, firePosition.rotation) as GameObject;
        NetworkServer.Spawn(bulletInstance);
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
