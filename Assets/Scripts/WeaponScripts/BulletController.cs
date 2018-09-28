using UnityEngine;

public class BulletController : MonoBehaviour
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


    void Update()
    {
        age += Time.deltaTime;
        if (age > shellLifeTime)
        {
            Destroy(gameObject);
        }
    }


    void OnTriggerEnter(Collider hitInfo)
    {

        PlayerStats enemy = hitInfo.GetComponent<PlayerStats>();

        if (enemy != null)
        {
            enemy.TakeDamage(damage);
        }
        Destroy(gameObject);
    }

}
