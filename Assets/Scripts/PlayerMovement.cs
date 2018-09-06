using UnityEngine;


public class PlayerMovement : MonoBehaviour
{
    public Rigidbody rb;

    [Header("Horizontal Movement")]
    [SerializeField] float horizontalForce = 60f;
    [SerializeField] float maxSpeed = 7.5f;

    [Header("Jump")]
    [SerializeField] float jumpVelocity = 10f;
    [SerializeField] float fallMultiplier = 4f;
    [SerializeField] float lowJumpMultiplier = 3f;
    
    bool jumpRequest;
    bool forwardRequest;
    bool backwardRequest;

    void Update()
    {
        MovementRequests();
    }


    void FixedUpdate ()
    {
        ForceApplication();
        BetterJump();
    }


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


    //TODO holding jump feels to floaty need to implement double jump and remove infinite jump
    //TODO deceleration is to fast when transitioning between in-air max speed and running max speed 
    private void ForceApplication()
    {
        if (forwardRequest)
        {
            if (rb.velocity.x == 0f || rb.velocity.x < maxSpeed)
            {
                rb.AddForce(Vector3.right * horizontalForce * Time.deltaTime, ForceMode.Impulse);
                forwardRequest = false;
            }
            if (rb.velocity.x > maxSpeed)
            {
                rb.AddForce(Vector3.right * maxSpeed * Time.deltaTime, ForceMode.Impulse);
                forwardRequest = false;
            }
        }
        if (backwardRequest)
        {
            if (rb.velocity.x == 0f || rb.velocity.x > -maxSpeed)
            {
                rb.AddForce(Vector3.left * horizontalForce * Time.deltaTime, ForceMode.Impulse);
                backwardRequest = false;
            }
            if (rb.velocity.x < -maxSpeed)
            {
                rb.AddForce(Vector3.left * maxSpeed * Time.deltaTime, ForceMode.Impulse);
                backwardRequest = false;
            }
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
    
}
