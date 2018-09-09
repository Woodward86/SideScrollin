using System.Collections;
using System.Collections.Generic;
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
    //movement information
    Vector3 walkVelocity;
    Vector3 prevWalkVelocity;
    FacingDirection facing;
    float adjVertVelocity;
    int jumpCounter;
    float jumpPressTime;
    bool jumpRequest;
    bool isGrounded;

    //settings
    public float walkSpeed = 3f;
    public float jumpSpeed = 6f;
    public float fallMultiplier = 4f;
    public float lowJumpMultiplier = 3f;


    
    public override void ReadInput(InputData data)
    {
        prevWalkVelocity = walkVelocity;
        ResetMovementToZero();

        //set horizontal movement
        if (data.axes[1] != 0f)
        {
            walkVelocity += Vector3.right * data.axes[1] * walkSpeed;
        }

        //set vertical jump
        if (data.buttons[0] == true)
        {
            if (jumpPressTime == 0f)
            {
                if (isGrounded || jumpPressTime < 1f && jumpCounter < 2)
                {
                    jumpRequest = true;
                    jumpCounter++;
                    adjVertVelocity = jumpSpeed;
                    isGrounded = false;
                }
            }
            jumpPressTime += Time.deltaTime;
        }

        else
        {
            jumpPressTime = 0f;
            jumpRequest = false;
        }

        newInput = true;

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
        }

        // basic movement
        rb.velocity = new Vector3(walkVelocity.x, rb.velocity.y + adjVertVelocity, walkVelocity.z);

        // movement modifiers
        JumpModifier();

        newInput = false;
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

        Debug.Log(facing);
    }


    private void JumpModifier()
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

    //TODO: think about an easing function for horizontal movement so the player doesn't stop completely when input is terminated
    void ResetMovementToZero()
    {
        walkVelocity = Vector3.zero;
        adjVertVelocity = 0f;

    }



}
