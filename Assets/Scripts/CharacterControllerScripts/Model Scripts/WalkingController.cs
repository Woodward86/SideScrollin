using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum FacingDirection
{
    Up, 
    Down,
    Left,
    Right,
    Screen
}


public class WalkingController : Controller
{

    //camera
    [SerializeField] Vector3 offset;
    [SerializeField] float verticleBlend = 10f;
    [SerializeField] float horizontalBlend = 1.75f;
    [SerializeField] float depthBlend = 0f;

    //movement information
    Vector3 walkVelocity;
    Vector3 prevWalkVelocity;
    FacingDirection facing = FacingDirection.Right;
    float adjVertVelocity;
    int jumpCounter;
    float jumpPressTime;
    bool jumpRequest;
    bool isGrounded;
    bool isWallSliding;
    float wallSlideTime;
 

    //settings
    public float walkSpeed = 6f;
    public float jumpSpeed = 9f;
    public float fallMultiplier = 4.5f;
    public float lowJumpMultiplier = 3f;
    public float wallStickTime = 1f;
    public float wallSlideSpeed = 3f;
    public float interactDuration = 0.1f;
    public LayerMask rayCastIgnore;

    //delegates and events
    public delegate void FacingChangeHandler (FacingDirection fd);
    public static event FacingChangeHandler OnFacingChange;
    public delegate void HitboxEventHandler (float dur);
    public static event HitboxEventHandler OnInteract;


    void Start()
    {
        if(OnFacingChange != null)
        {
            OnFacingChange(facing);
        }
    }


    public override void ReadInput(InputData data)
    {
        prevWalkVelocity = walkVelocity;
        ResetMovementToZero();

        //TODO: fix unlimited wall stick as long as there is input toward wall
        //set horizontal movement
        if (data.axes[1] != 0f)
        {
            walkVelocity += Vector3.right * data.axes[1] * walkSpeed;
        }

        //TODO: fix unlimited jump while touching walls
        //set vertical jump
        if (data.buttons[0] == true)
        {
            if (jumpPressTime == 0f)
            {
                if (isGrounded || jumpPressTime < 1f && jumpCounter < 2)
                {
                    jumpCounter++;
                    jumpRequest = true;
                    isGrounded = false;
                    adjVertVelocity = jumpSpeed;

                    //harder jump off wall
                    //TODO: decide if this should be more of an added horizontal force then vertical
                    if (isWallSliding && facing.Equals(FacingDirection.Right))
                    {
                        adjVertVelocity = jumpSpeed * 1.25f;
                    }
                }

            }
            jumpPressTime += Time.deltaTime;
        }
        else
        {
            jumpPressTime = 0f;
            jumpRequest = false;
        }

        //TODO: in the air this activates wallSlide
        //check if interact button is pressed
        if (data.buttons[1] == true)
        {
            if (OnInteract != null)
            {
                OnInteract(interactDuration);
            }
        }

        newInput = true;

    }


    void LateUpdate()
    {
        if (!newInput)
        {
            prevWalkVelocity = walkVelocity;
            ResetMovementToZero();
            jumpPressTime = 0f;
            jumpRequest = false;
        }
        if (prevWalkVelocity != walkVelocity)
        {
            // check for face change
            CheckForFacingChange();
        }
        if (!jumpRequest)
        {
            TestGroundedState();
            TestWallSlidingState();
        }

        // camera movement
        CameraMovement();

        // basic movement
        rb.velocity = new Vector3(walkVelocity.x, rb.velocity.y + adjVertVelocity, walkVelocity.z);

        // movement modifiers
        JumpModifier();
        WallSlidingModifier();


        newInput = false;
    }

    //TODO: have this work off boxes on screen to trigger camera follow
    void CameraMovement()
    {
        float horizontalPosition = (transform.position.x + offset.x - pc.transform.position.x) * horizontalBlend;
        float verticalPosition = (transform.position.y + offset.y - pc.transform.position.y) * verticleBlend;
        float depthPosition = (transform.position.z + offset.z - pc.transform.position.z) * depthBlend;
        pc.transform.position += new Vector3(horizontalPosition, verticalPosition, depthPosition) * Time.deltaTime;
    }


    //TODO: replace this with Physics.OverlapBox style or add more rays
    //TODO: add collider "skin" public variable
    void TestGroundedState()
    {
        if (Physics.Raycast(transform.position, Vector3.down, coll.bounds.extents.y + 0.1f, ~rayCastIgnore))
        {
            isGrounded = true;
            jumpCounter = 0;
        }
        else
        {
            isGrounded = false;
        }
    }


    void TestWallSlidingState()
    {
        bool isContactRight = Physics.Raycast(transform.position, Vector3.right, coll.bounds.extents.x + 0.1f, ~rayCastIgnore);
        bool isContactLeft = Physics.Raycast(transform.position, Vector3.left, coll.bounds.extents.x + 0.1f, ~rayCastIgnore);

        if (isContactRight || isContactLeft && !isGrounded)
        {
            isWallSliding = true;
            jumpCounter = 0;
            wallSlideTime += Time.deltaTime;
        }
        else
        {
            isWallSliding = false;
            wallSlideTime = 0f;
        }
    }


    void CheckForFacingChange()
    {
        if (walkVelocity == Vector3.zero)
        {
            return;
        }
        if (walkVelocity.x == 0)
        {
            ChangeFacing(walkVelocity);
        }
        else
        {
            if (prevWalkVelocity.x == 0)
            {
                ChangeFacing(new Vector3(walkVelocity.x, 0, 0));
            }
            else
            {
                Debug.LogWarning("Unexpected walkVelocity value.");
                ChangeFacing(walkVelocity);
            }
        }
    }


    void ChangeFacing(Vector3 dir)
    {
        if (dir.x != 0)
        {
            facing = (dir.x > 0) ? FacingDirection.Right : FacingDirection.Left;
        }

        if (OnFacingChange != null)
        {
            OnFacingChange(facing);
        }

    }


    void JumpModifier()
    {
        //better fall
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        //TODO: change this so it's reading from InputManager
        else if (rb.velocity.y > 0 && !Input.GetKey(KeyCode.Space))
        {
            rb.velocity += Vector3.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }


    void WallSlidingModifier()
    {
        //wallSlide speed
        if (isWallSliding)
        {
            if (rb.velocity.y < 0 && wallSlideTime < wallStickTime)
            {
                rb.velocity = Vector3.up * Physics.gravity.y * wallSlideSpeed * Time.deltaTime;
            }
            else
            {
                isWallSliding = false;
            }
        }
    }


    //TODO: think about an easing function for horizontal movement so the player doesn't stop completely when input is terminated
    void ResetMovementToZero()
    {
        walkVelocity = Vector3.zero;
        adjVertVelocity = 0f;
    }



}
