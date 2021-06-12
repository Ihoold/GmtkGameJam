using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float walkingSpeed;
    public float crawlingSpeed;
    public float jumpHeight;
    public TrackGround groundCheck;

    float horizontalMoveInput;
    bool jumpInput;

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
        return Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.JoystickButton0);
    }

    void RecordJumpInput() {
        jumpInput = !isCrawling && groundCheck.isGrounded && JumpButtonPressed();
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
        if (Input.GetKeyDown(KeyCode.C)) ToggleCrawling(!isCrawling);
    }

    void FixedUpdate() {
        ProcessMovement();
    }
}
