using UnityEngine;
using UnityEngine.Networking;

public class BulletController : NetworkBehaviour
{
    //settings
    public float speed = 20f;
    public float damage = 1f;
    [SerializeField] float shellLifeTime = 2f;

    float age;

    public Rigidbody rb;


	void Start ()
    {
        rb.velocity = transform.right * speed;
	}

    [ServerCallback]
    void Update()
    {
        age += Time.deltaTime;
        if (age > shellLifeTime)
        {
            NetworkServer.Destroy(gameObject);
        }
    }


    void OnTriggerEnter(Collider hitInfo)
    {

        if (!isServer)
        {
            return;
        }

        PlayerStats enemy = hitInfo.GetComponent<PlayerStats>();

        if (enemy != null)
        {
            enemy.TakeDamage(damage);
        }
        Destroy(gameObject);
    }

}
