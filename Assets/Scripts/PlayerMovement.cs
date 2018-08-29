using UnityEngine;


public class PlayerMovement : MonoBehaviour
{
    public Rigidbody rb;

    [Header("Horizontal Movement")]
    [SerializeField] float forwardForce = 40f;
    [SerializeField] float backwardForce = -40f;

    // This is for TransformMovement()
    [SerializeField] float speed = 5f;

    [Header("Jump")]
    [SerializeField] float jumpVelocity = 10f;
    [SerializeField] float fallMultiplier = 2.5f;
    [SerializeField] float lowJumpMultiplier = 2f;
    
    bool jumpRequest;

	// Update is called once per frame
	void FixedUpdate ()
    {
        ForceApplication();
        BetterJump();
    }


    private void Update()
    {
        TransformMovement();
        MovementRequests();
    }

    // TODO clamp forward and backward speed
    // TODO finish refactoring inputs into MovementRequest() and addforce into ForceApplication() that works off boolean logic
    private void MovementRequests()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpRequest = true;
        }
    }


    private void ForceApplication()
    {
        if (Input.GetKey(KeyCode.D))
        {
            rb.AddForce(forwardForce * Time.deltaTime, 0f, 0f, ForceMode.Impulse);
        }
        if (Input.GetKey(KeyCode.A))
        {
            rb.AddForce(backwardForce * Time.deltaTime, 0f, 0f, ForceMode.Impulse);
        }
        if (jumpRequest)
        {
            rb.AddForce(Vector3.up * jumpVelocity, ForceMode.Impulse);
            jumpRequest = false;
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
            rb.velocity += Vector3.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }
    
    // TODO finish this for backward too
    private void TransformMovement()
    {
        if(Input.GetKey(KeyCode.H))
        {
            transform.position += Vector3.right * Time.deltaTime * speed;
        }
    }
}
