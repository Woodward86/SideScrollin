﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class WalkingController : Controller
{

    //camera
    [SerializeField] Vector3 offset;
    [SerializeField] float verticleBlend = 10f;
    [SerializeField] float horizontalBlend = 1.75f;
    [SerializeField] float depthBlend = 0f;

    //movement information
    Vector3 walkVelocity;
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


    public override void Start()
    {
        base.Start();
    }


    public override void ReadInput(InputData data)
    {
        ResetMovementToZero();

        //TODO: fix unlimited wall stick as long as there is input toward wall
        //set horizontal movement
        if (data.axes[1] != 0f)
        {
            walkVelocity += Vector3.right * data.axes[1] * walkSpeed;
            //TODO: looks like the way I'm turning the character is causing issues when networked
            if(data.axes[1] > 0f)
            {
                transform.localRotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
            }
            else
                transform.localRotation = Quaternion.Euler(0.0f, 180f, 0.0f);
        }

        //TODO: fix unlimited jump while touching walls
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

                    //harder jump off wall
                    //TODO: decide if this should be more of an added horizontal force then vertical
                    if (isWallSliding)
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

        //check if interact button is pressed
        if (data.buttons[1] == true)
        {
            Debug.Log("Interact button pressed");
        }

        newInput = true;

    }


    void LateUpdate()
    {
        if (!newInput)
        {
            ResetMovementToZero();
            jumpPressTime = 0f;
            jumpRequest = false;
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
        if (Physics.Raycast(transform.position, Vector3.down, coll.bounds.extents.y + 0.1f))
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
        bool isContactRight = Physics.Raycast(transform.position, Vector3.right, coll.bounds.extents.x + 0.1f);
        bool isContactLeft = Physics.Raycast(transform.position, Vector3.left, coll.bounds.extents.x + 0.1f);

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


    void JumpModifier()
    {
        //better fall
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        //TODO: change this so it's reading from InputManager
        else if (rb.velocity.y > 0 && !jumpRequest)
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