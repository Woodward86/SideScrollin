using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerMovement : MonoBehaviour
{
    public Rigidbody rb;

    [Header("Horizontal Movement")]
    [SerializeField] float horizontalForce = 70f;
    [SerializeField] float maxGroundSpeed = 12f;
    [SerializeField] float maxAirSpeed = 5f;

    float maxSpeed;
    bool forwardRequest;
    bool backwardRequest;

    [Header("Jump")]
    [SerializeField] float jumpVelocity = 1000f;
    [SerializeField] float fallMultiplier = 4f;
    [SerializeField] float lowJumpMultiplier = 3f;
    [SerializeField] int maxJumpNumber = 2;
    [SerializeField] float groundedSkin = 0.05f;
    [SerializeField] LayerMask mask;

    bool jumpRequest;
    bool grounded;
    int jumpCounter;

    Vector3 playerSize;
    Vector3 boxSize;

    
    private void Awake()
    {
        playerSize = GetComponent<BoxCollider>().size;
        boxSize = new Vector3(playerSize.x, groundedSkin, playerSize.z);
    }


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
        if (Input.GetKeyDown(KeyCode.Space) && grounded || Input.GetKeyDown(KeyCode.Space) && jumpCounter < maxJumpNumber)
        {
            jumpRequest = true;
        }
    }

 
    //TODO jump is way to high if player hits jump 2 times really quickly
    //TODO player sticks to much when landing on ground 
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
            rb.AddForce(Vector3.up * jumpVelocity * Time.deltaTime, ForceMode.Impulse);
            jumpRequest = false;
            grounded = false;
            maxSpeed = maxAirSpeed;
            jumpCounter++;
        }
        else
        {
            Vector3 boxCenter = transform.position + Vector3.down * (playerSize.y + boxSize.y) * 0.5f;
            Collider[] hitColliders = Physics.OverlapBox(boxCenter, boxSize, Quaternion.identity, mask);
            if (hitColliders.Length > 0)
            {
                grounded = true;
                maxSpeed = maxGroundSpeed;
                jumpCounter = 0;
            }
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
