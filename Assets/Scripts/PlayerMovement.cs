using UnityEngine;


public class PlayerMovement : MonoBehaviour
{
    [Header("Horizontal Movement")]
    [SerializeField] float forwardForce = 40f;
    [SerializeField] float backwardForce = -40f;

    [Header("Jump")]
    [SerializeField] float jumpVelocity = 10f;
    [SerializeField] float fallMultiplier = 2.5f;
    [SerializeField] float lowJumpMultiplier = 2f;

    public Rigidbody rb;

	// Update is called once per frame
	void FixedUpdate ()
    {
        ForceMovement();
    }


    private void Update()
    {
        BetterJump();
    }

    // TODO clamp forward and backward speed
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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.velocity = Vector3.up * jumpVelocity; 
        }
    }


    private void BetterJump()
    {
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (rb.velocity.y > 0 && !Input.GetKey(KeyCode.Space))
        {
            rb.velocity += Vector3.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.fixedDeltaTime;
        }
    }
               
}
