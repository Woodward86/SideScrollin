using UnityEngine;
using UnityEngine.Networking;

public class PlayerStats : NetworkBehaviour
{

    [SerializeField] float maxHealth = 100f;

    PlayerNetwork_Extension player;
    float health;


    void Awake()
    {
        player = GetComponent<PlayerNetwork_Extension>();
    }


    [ServerCallback]
    void OnEnable()
    {
        health = maxHealth;
        //Debug.Log(health);
    }


    [Server]
    public bool TakeDamage(float weaponDamage)
    {
        bool isDead = false;

        if (health <= 0)
        {
            return isDead;
        }

        health -= weaponDamage;
        isDead = health <= 0;

        RpcTakeDamage(isDead);

        return isDead;
    }


    [ClientRpc]
    void RpcTakeDamage(bool isDead)
    {
        if (isDead)
        {
            player.Die();
        }

    }

}
