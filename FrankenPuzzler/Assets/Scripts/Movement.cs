using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float walkingSpeed;
    public float crawlingSpeed;
    public float jumpHeight;
    public float climbingSpeed;
    public TrackGround groundCheck;

    float horizontalMoveInput;
    float jumpInputTime;
    bool jumpInput;
    float ladderInput;

    [HideInInspector] public bool isCrawling;
    float jumpVelocity;
    float currentMoveSpeed;
    Rigidbody rb;

    public void SetWalkingSpeed() {
        currentMoveSpeed = walkingSpeed;
    }

    public void SetCrawlingSpeed() {
        currentMoveSpeed = crawlingSpeed;
    }

    void RecordMovementInput() {
        horizontalMoveInput = Input.GetAxisRaw("Horizontal");
    }

    bool JumpButtonPressed() {
        if (!Input.GetKeyDown(KeyCode.Space)) {
            return false;
        }
        jumpInputTime = Time.time;
        return true;
    }

    void RecordJumpInput() {
        jumpInput = jumpInput || (!isCrawling && (groundCheck.isGrounded || groundCheck.isNearLadder) && JumpButtonPressed());
    }

    void RecordLadderInput()
    {
        ladderInput = climbingSpeed * Input.GetAxisRaw("Vertical"); 
    }

    float ProcessHorizontalMovement() {
        return horizontalMoveInput * currentMoveSpeed;
    }

    float ProcessVerticalMovement() {
        float velocity;
        rb.useGravity = true;
        if (jumpInput && (Time.time - jumpInputTime < 0.1f)) {
            velocity =  jumpVelocity;
        } else if (groundCheck.isNearLadder && ladderInput > 0) {
            rb.useGravity = false;
            velocity = ladderInput;
        } else {
            velocity = rb.velocity.y;
        }
        jumpInput = false;
        return velocity;
    }

    void ToggleCrawling(bool toggle) {
        isCrawling = toggle;
        if (isCrawling) {
            SetCrawlingSpeed();
            transform.localScale = new Vector3(transform.localScale.x, 0.5f, transform.localScale.z);
        } else {
            SetWalkingSpeed();
            transform.localScale = new Vector3(transform.localScale.x, 1f, transform.localScale.z);
        }
    }

    void ProcessMovement() {
        rb.velocity = new Vector3(ProcessHorizontalMovement(), ProcessVerticalMovement(), 0);
    }

    void CalculateJumpVelocity() {
        jumpVelocity = Mathf.Sqrt(-2 * jumpHeight * Physics.gravity.y);
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        CalculateJumpVelocity();
        SetWalkingSpeed();
    }

    void Update()
    {
        if (GetComponent<Player>().isAttacking) return;
        RecordMovementInput();
        RecordJumpInput();
        RecordLadderInput();
        if (Input.GetKeyDown(KeyCode.C)) ToggleCrawling(!isCrawling);
    }


    void FixedUpdate() {
        if (GetComponent<Player>().isAttacking) return;
        ProcessMovement();
    }
}
