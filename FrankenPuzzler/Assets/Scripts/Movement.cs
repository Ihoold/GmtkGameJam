using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float walkingSpeed;
    public float jumpHeight;
    public TrackGround groundCheck;

    float horizontalMoveInput;
    bool jumpInput;
    float jumpVelocity;
    float currentMoveSpeed = 0f;
    Rigidbody rb;

    public void SetWalkingSpeed() {
        currentMoveSpeed = walkingSpeed;
    }

    void RecordMovementInput() {
        horizontalMoveInput = Input.GetAxisRaw("Horizontal");
    }

    bool JumpButtonPressed() {
        return Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.JoystickButton0);
    }

    void RecordJumpInput() {
        jumpInput = groundCheck.isGrounded && JumpButtonPressed();
    }

    float ProcessHorizontalMovement() {
        return horizontalMoveInput * currentMoveSpeed;
    }

    float ProcessVerticalMovement() {
        if (jumpInput) {
            return jumpVelocity;
        } else {
            return rb.velocity.y;
        }
    }

    void ProcessMovement() {
        rb.velocity = new Vector3(ProcessHorizontalMovement(), ProcessVerticalMovement(), 0);
    }

    void CalculateJumpVelocity() {
        jumpVelocity = 2 * jumpHeight / Mathf.Sqrt(-2 * jumpHeight / Physics.gravity.y);
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        CalculateJumpVelocity();
        SetWalkingSpeed();
    }

    void Update()
    {
        RecordMovementInput();
        RecordJumpInput();
    }

    void FixedUpdate() {
        ProcessMovement();
    }
}
