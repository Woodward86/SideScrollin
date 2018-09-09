using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(PlayerCollision))]
public class PlayerMovement : MonoBehaviour
{
    bool m_Started;
    public Rigidbody rb;
    

    [Header("Horizontal Movement")]
    [SerializeField] float horizontalForce = 70f;
    [SerializeField] float maxGroundSpeed = 12f;
    [SerializeField] float maxAirSpeed = 5f;
    [SerializeField] float maxWallSlideSpeed = 3f;

    float maxSpeed;

    [Header("Jump")]
    [SerializeField] float jumpVelocity = 1000f;
    [SerializeField] float fallMultiplier = 4f;
    [SerializeField] float lowJumpMultiplier = 3f;
    [SerializeField] int maxJumpNumber = 2;
    [SerializeField] float groundedSkin = 0.05f;
    [SerializeField] LayerMask groundMask;
    [SerializeField] LayerMask wallMask;
    
    bool forwardRequest;
    bool backwardRequest;
    bool jumpRequest;
    int jumpCounter;
    bool isGrounded;
    bool isWallSliding;
    float wallTimeCounter;

    Vector3 playerSize;
    Vector3 downBox;
    Vector3 frontBox;
    Vector3 backBox;


    private void Awake()
    {
        m_Started = true;
        playerSize = GetComponent<BoxCollider>().size;
        downBox = new Vector3(playerSize.x - 0.8f, groundedSkin, playerSize.z - 0.8f);

        // TODO figure out why the boxes have to be so small to collide all the time
        frontBox = new Vector3(groundedSkin, playerSize.y - 0.8f, playerSize.z - 0.8f);
        backBox = new Vector3(groundedSkin, playerSize.y - 0.8f, playerSize.z - 0.8f);

        rb.useGravity = true;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }


    void Update()
    {
        MovementRequests();
    }


    void FixedUpdate ()
    {
        ForceApplication();
        BetterJump();
        WallJumps();
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
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded || Input.GetKeyDown(KeyCode.Space) && jumpCounter < maxJumpNumber)
        {
            jumpRequest = true;
        }
        if (Input.GetKeyDown(KeyCode.Space) && isWallSliding && jumpCounter < maxJumpNumber)
        {
            jumpRequest = true;
        }
    }

 
    private void ForceApplication()
    {
        if (forwardRequest)
        {
            ApplyForwardForce();
        }
        if (backwardRequest)
        {
            ApplyBackwardForce();
        }
        if (jumpRequest)
        {
            ApplyJumpForce();
        }
        else
        {
            ApplyGroundedState();
            ApplyWallSlideState();
        }
    }

    // TODO player stops to abruptly when landing from a jump
    private void ApplyForwardForce()
    {
        transform.localRotation = Quaternion.Euler(0.0f, 0f, 0f);

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


    private void ApplyBackwardForce()
    {
        transform.localRotation = Quaternion.Euler(0.0f, 180f, 0f);

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


    private void ApplyJumpForce()
    {
        rb.AddForce(Vector3.up * jumpVelocity * Time.deltaTime, ForceMode.Impulse);
        jumpRequest = false;
        isGrounded = false;
        maxSpeed = maxAirSpeed;
        jumpCounter++;
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

    // TODO build out wall jumps
    private void WallJumps()
    {
        // TODO this should push you off the wall slightly before getting back on the wall
        if (isWallSliding && Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.Space))
        {
            rb.AddForce(Vector3.right * jumpVelocity * Time.deltaTime, ForceMode.Impulse);
            rb.AddForce(Vector3.up * jumpVelocity * Time.deltaTime, ForceMode.Impulse);
            rb.AddForce(Vector3.left * jumpVelocity * Time.deltaTime, ForceMode.Impulse);
        }
    }


    private void ApplyGroundedState()
    {
        Vector3 downBoxCenter = transform.position + Vector3.down * (playerSize.y + downBox.y) * 0.5f;
        Collider[] hitColliders = Physics.OverlapBox(downBoxCenter, downBox, Quaternion.identity, groundMask);
        if (hitColliders.Length > 0)
        {
            isGrounded = true;
            maxSpeed = maxGroundSpeed;
            jumpCounter = 0;
            ResetWallSlideState();
        }
    }

    // TODO using wallTimeCounter variable have a timer for how long you can hold on to walls without free falling down
    private void ApplyWallSlideState()
    {
        Vector3 frontBoxCenter = transform.position + Vector3.right * (playerSize.x + frontBox.x) * 0.5f;
        Collider[] frontColliders = Physics.OverlapBox(frontBoxCenter, frontBox, Quaternion.identity, wallMask);
        Vector3 backBoxCenter = transform.position + Vector3.left * (playerSize.x + backBox.x) * 0.5f;
        Collider[] backColliders = Physics.OverlapBox(backBoxCenter, backBox, Quaternion.identity, wallMask);

        if (frontColliders.Length > 0 || backColliders.Length > 0)
        {
            wallTimeCounter += Time.deltaTime;
            isWallSliding = true;
            rb.velocity = Vector3.up * Physics.gravity.y * Time.deltaTime;
            isGrounded = false;
            Debug.Log(rb.velocity);
        }
        else
        {
            ResetWallSlideState();
        }
    }


    private void ResetWallSlideState()
    {
        isWallSliding = false;
        wallTimeCounter = 0.0f;
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if (m_Started)
        {
            Vector3 downBoxCenter = transform.position + Vector3.down * (playerSize.y + downBox.y) * 0.5f;
            Gizmos.DrawWireCube(downBoxCenter, downBox);

            Vector3 frontBoxCenter = transform.position + Vector3.right * (playerSize.x + frontBox.x) * 0.5f;
            Gizmos.DrawWireCube(frontBoxCenter, frontBox);

            Vector3 backBoxCenter = transform.position + Vector3.left * (playerSize.x + backBox.x) * 0.5f;
            Gizmos.DrawWireCube(backBoxCenter, backBox);
        }
    }
}
