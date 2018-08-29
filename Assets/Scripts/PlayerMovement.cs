using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] float forwardForce = 40f;
    [SerializeField] float backwardForce = -40f;
    [SerializeField] float upwardForce = 100f;

    public Rigidbody rb;

	// Update is called once per frame
	void FixedUpdate ()
    {
        TransformMovement();
        ForceMovement();
    }

    private void ForceMovement()
    {
        if (Input.GetKey(KeyCode.D))
        {
            rb.AddForce(forwardForce * Time.deltaTime, 0f, 0f, ForceMode.VelocityChange);
        }
        if (Input.GetKey(KeyCode.A))
        {
            rb.AddForce(backwardForce * Time.deltaTime, 0f, 0f, ForceMode.VelocityChange);
        }
        if (Input.GetKey(KeyCode.Space))
        {
            rb.AddForce(0f, upwardForce * Time.deltaTime, 0f, ForceMode.VelocityChange); 
        }
    }


    private void TransformMovement()
    {
        // Try to build this out
    }
}
