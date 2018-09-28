using UnityEngine;


public enum FacingDirection
{
    Up,
    Down,
    Left,
    Right
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
    FacingDirection facing = FacingDirection.Right;
    float adjVertVelocity;
    float adjHorizVelocity;
    int jumpCounter;
    float jumpPressTime;
    bool jumpRequest;
    bool isGrounded;
    bool isContactLeft;
    bool isContactRight;
    bool isCrouching;
    float crouchPressTime;
    bool isWallSliding;
    float wallSlideTime;
    bool isStrafing;
    float strafePressTime;

    //settings
    public float walkSpeed = 6f;
    public float crouchSpeedDivisor = 1.5f;
    public float strafeSpeedDivisor = 2.5f;
    public float jumpSpeed = 9f;

    public float fallMultiplier = 4.5f;
    public float lowJumpMultiplier = 3f;
    public float wallStickTime = 1f;
    public float wallSlideSpeed = 3f;


    public override void Start()
    {
        base.Start();
    }


    public override void ReadInput(InputData data)
    {
        ResetMovementToZero();

        //set horizontal movement
        if (data.axes[1] != 0f)
        {
            //unlimited wall stick fix
            if (isContactLeft && data.axes[1] < 0)
            {
                walkVelocity += Vector3.right * 0f;
            }
            else if (isContactRight && data.axes[1] > 0)
            {
                walkVelocity += Vector3.right * 0f;
            }
            else
            {
                walkVelocity += Vector3.right * data.axes[1] * walkSpeed;
            }

            //set facing direction
            //TODO: write isStrafing cleaner, maybe separate rotation into a different function
            if(isStrafing)
            {
                rb.velocity += walkVelocity / strafeSpeedDivisor;
            }
            else
            {
                if (data.axes[1] > 0f)
                {
                    transform.localRotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
                    facing = FacingDirection.Right;
                }
                else
                {
                    transform.localRotation = Quaternion.Euler(0.0f, 180f, 0.0f);
                    facing = FacingDirection.Left;
                }
            }


        }


        //TODO: max height of double jump is way to high the second jump velocity should be added from base vert velocity not the new adjusted vert velocity
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
                }

                //no vert jumping while facing the wall
                if (isWallSliding && isContactLeft && facing == FacingDirection.Left)
                {
                    adjVertVelocity = 0f;
                }
                if (isWallSliding && isContactRight && facing == FacingDirection.Right)
                {
                    adjVertVelocity = 0f;
                }

                //jumping while not facing the wall
                if (isWallSliding && isContactLeft && facing == FacingDirection.Right)
                {
                    adjVertVelocity = jumpSpeed * 1.25f;
                    adjHorizVelocity = jumpSpeed;
                }
                if (isWallSliding && isContactRight && facing == FacingDirection.Left)
                {
                    adjVertVelocity = jumpSpeed * 1.25f;
                    adjHorizVelocity = -jumpSpeed;
                }

            }
            jumpPressTime += Time.deltaTime;
        }
        else
        {
            ResetJump();
        }


        //check for crouch press
        if (data.buttons[4] == true)
        {
            isCrouching = true;
            crouchPressTime += Time.deltaTime;
        }
        else
        {
            ResetCrouch();
        }


        //check if interact button is pressed
        if (data.buttons[5] == true)
        {
            isStrafing = true;
            strafePressTime += Time.deltaTime;
        }
        else
        {
            ResetStrafe();
        }


        newInput = true;

    }


    void LateUpdate()
    {
        if (!newInput)
        {
            ResetMovementToZero();
            ResetJump();
            ResetCrouch();
            ResetStrafe();
        }

        TestGroundedState();
        TestWallSlidingState();

        // camera movement
        CameraMovement();

        // basic movement
        rb.velocity = new Vector3(walkVelocity.x + adjHorizVelocity, rb.velocity.y + adjVertVelocity, walkVelocity.z);

        // movement modifiers
        JumpModifier();
        WallSlidingModifier();
        CrouchModifier();
        StrafeModifier();

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
        if (Physics.Raycast(transform.position, Vector3.down, cColl.bounds.extents.y + 0.225f))
        {
            isGrounded = true;
            jumpCounter = 1;
        }
        else
        {
            isGrounded = false;
        }
    }


    void TestWallSlidingState()
    {
        isContactRight = Physics.Raycast(transform.position, Vector3.right, cColl.bounds.extents.x + 0.1f);
        isContactLeft = Physics.Raycast(transform.position, Vector3.left, cColl.bounds.extents.x + 0.1f);

        if (isContactLeft && !isGrounded || isContactRight && !isGrounded)
        {
            isWallSliding = true;
            wallSlideTime += Time.deltaTime;
            jumpCounter = 1;
        }
        else
        {
            ResetWallSlide();
        }
    }

    //TODO: crouch has some weird display issues when networked
    //TODO: need to keep crouch enabled if your under somethings
    void CrouchModifier()
    {
        if (isCrouching)
        {
            rb.velocity -= walkVelocity / crouchSpeedDivisor;
            bColl.enabled = false;
            isStrafing = false;
        }
        else
        {
            isCrouching = false;
            bColl.enabled = true;
        }
    }


    void JumpModifier()
    {
        //better fall
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (rb.velocity.y > 0 && !jumpRequest)
        {
            rb.velocity += Vector3.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }


    void WallSlidingModifier()
    {
        //wallSlide speed
        if (isWallSliding && rb.velocity.y < 0 && wallSlideTime < wallStickTime)
        {
            rb.velocity = Vector3.up * Physics.gravity.y * wallSlideSpeed * Time.deltaTime;
        }
    }


    void StrafeModifier()
    {
        if (isStrafing)
        {
            rb.velocity -= walkVelocity / strafeSpeedDivisor;
        }
    }


    void ResetMovementToZero()
    {
        walkVelocity = Vector3.zero;
        adjVertVelocity = 0f;
        adjHorizVelocity = 0f;
    }


    void ResetJump()
    {
        jumpPressTime = 0f;
        jumpRequest = false;
    }


    void ResetWallSlide()
    {
        wallSlideTime = 0f;
        isWallSliding = false;
    }


    void ResetCrouch()
    {
        crouchPressTime = 0f;
        isCrouching = false;
    }


    void ResetStrafe()
    {
        strafePressTime = 0f;
        isStrafing = false;
    }



}
