using UnityEngine;


public class PlayerMovement : MonoBehaviour
{
    public Rigidbody rb;

    [Header("Horizontal Movement")]
    [SerializeField] float forwardForce = 3f;
    [SerializeField] float backwardForce = 3f;

    // This is for TransformMovement()
    [SerializeField] float speed = 5f;

    [Header("Jump")]
    [SerializeField] float jumpVelocity = 10f;
    [SerializeField] float fallMultiplier = 2.5f;
    [SerializeField] float lowJumpMultiplier = 2f;
    
    bool jumpRequest;
    bool forwardRequest;
    bool backwardRequest;

    private void Update()
    {
        MovementRequests();
        TransformMovement();
    }

    // Update is called once per frame
    void FixedUpdate ()
    {
        ForceApplication();
        BetterJump();
    }

    // TODO clamp forward and backward speed
    private void MovementRequests()
    {
        if (Input.GetKey(KeyCode.D))
        {
            forwardRequest = true;
        }
        if (Input.GetKey(KeyCode.A))
        {
            backwardRequest = true;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpRequest = true;
        }
    }


    private void ForceApplication()
    {
        if (forwardRequest)
        {
            // TODO figure out how to clamp added force
            if (rb.velocity.x >= 0 && rb.velocity.x < 5)
            {
                rb.AddForce(Vector3.right * forwardForce * Time.deltaTime, ForceMode.Impulse);
                forwardRequest = false;
                print(rb.velocity.x);
            }
            if (rb.velocity.x >= 1)
            {
                rb.AddForce(Vector3.right * 0, ForceMode.Impulse);
                print("Max velocity reached");
                forwardRequest = false;
            }
        }
        if (backwardRequest)
        {
            rb.AddForce(Vector3.left * backwardForce * Time.deltaTime, ForceMode.Impulse);
            backwardRequest = false;
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
    
    // TODO smooth these
    private void TransformMovement()
    {
        if (Input.GetKey(KeyCode.H))
        {
            transform.position += Vector3.right * Time.deltaTime * speed;
        }
        if (Input.GetKey(KeyCode.F))
        {
            transform.position += Vector3.left * Time.deltaTime * speed;
        }
    }
}
