using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Movement : MonoBehaviour
{
    public float walkingSpeed;
    public float crawlingSpeed;
    public float jumpHeight;
    public float climbingSpeed;
    public TrackGround groundCheck;
    public Transform modelRotation;
    public BoxCollider standingCollider;
    public BoxCollider crawlingCollider;

    float horizontalMoveInput;
    float jumpInputTime;
    bool jumpInput;
    float ladderInput;

    [HideInInspector] public bool isCrawling;
    bool jumping;
    float jumpVelocity;
    float currentMoveSpeed;
    Rigidbody rb;
    Animator animator;

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

    bool IsAbleToClimb() {
        return GetComponent<Player>().availableLimbs[1] && GetComponent<Player>().availableLimbs[2];
    }

    void RecordLadderInput()
    {
        if (IsAbleToClimb()) {
            ladderInput = climbingSpeed * Input.GetAxisRaw("Vertical"); 
        } else {
            ladderInput = 0f;
        }
    }

    void FixModelRotation() {
        if (!jumpInput && ladderInput > 0 && groundCheck.isNearLadder) {
            modelRotation.transform.rotation = Quaternion.RotateTowards(modelRotation.transform.rotation, Quaternion.Euler(0, 180, 0), 720f * Time.deltaTime);
            return;
        }

        if (rb.velocity.x < -0.05f) {
            modelRotation.transform.rotation = Quaternion.RotateTowards(modelRotation.transform.rotation, Quaternion.Euler(0, 90, 0), 720f * Time.deltaTime);
            return;
        } 
        
        if (rb.velocity.x > 0.05f) {
            modelRotation.transform.rotation = Quaternion.RotateTowards(modelRotation.transform.rotation, Quaternion.Euler(0, 270, 0), 720f * Time.deltaTime);
            return;
        }

        if (!jumping && (IsGoingUpOnLadder() || IsGoingDownOnLadder())) {
            modelRotation.transform.rotation = Quaternion.RotateTowards(modelRotation.transform.rotation, Quaternion.Euler(0, 180, 0), 720f * Time.deltaTime);
            return;
        }

        if (Mathf.Abs(ladderInput) > 0.1f) {
            modelRotation.transform.rotation = Quaternion.RotateTowards(modelRotation.transform.rotation, Quaternion.Euler(0, (ladderInput > 0) ? 180 : 0, 0), 720f * Time.deltaTime);
            return;
        }

        // don't return to idle
        // modelRotation.transform.rotation = Quaternion.RotateTowards(modelRotation.transform.rotation, Quaternion.Euler(0, 0, 0), 720f * Time.deltaTime);
    }

    float ProcessHorizontalMovement() {
        float horizontalMovement = horizontalMoveInput * currentMoveSpeed;
        animator.SetBool("Moving", Mathf.Abs(horizontalMovement) > 0.01f);
        return horizontalMovement;
    }

    bool IsGoingUpOnLadder() {
        return groundCheck.isNearLadder && ladderInput > 0.1f;
    }

    bool IsGoingDownOnLadder() {
        return (groundCheck.isOverLadder || groundCheck.isNearLadder) && (ladderInput < -0.1f);
    }

    float ProcessVerticalMovement() {
        float velocity;
        rb.useGravity = true;
        animator.SetBool("Climbing", false);
        if (jumpInput && (Time.time - jumpInputTime < 0.1f)) {
            velocity = jumpVelocity;
            animator.SetTrigger("Jumping");
            jumping = true;
        } else if ((IsGoingUpOnLadder() || IsGoingDownOnLadder()) && !jumping) {
            animator.SetBool("Climbing", true);
            rb.useGravity = false;
            velocity = ladderInput;
        } else if (!groundCheck.isNearLadder && groundCheck.isOverLadder) {
            rb.useGravity = false;
            velocity = 0f;
        } else {
            velocity = rb.velocity.y;
        }
        if (velocity <= 0.01f) {
            jumping = false;
        }
        jumpInput = false;
        return velocity;
    }

    public void ToggleCrawling(bool toggle) {
        isCrawling = toggle;
        if (isCrawling) {
            SetCrawlingSpeed();
            standingCollider.transform.gameObject.SetActive(false);
            crawlingCollider.transform.gameObject.SetActive(true);
        } else {
            SetWalkingSpeed();
            standingCollider.transform.gameObject.SetActive(true);
            crawlingCollider.transform.gameObject.SetActive(false);
        }
        animator.SetBool("Crawling", isCrawling);
    }

    void ProcessMovement() {
        rb.velocity = new Vector3(ProcessHorizontalMovement(), ProcessVerticalMovement(), 0);
        if (Mathf.Abs(rb.velocity.y) < 0.01f) rb.MovePosition(new Vector3(rb.position.x, rb.position.y + 0.01f, rb.position.z));
    }

    void CalculateJumpVelocity() {
        jumpVelocity = Mathf.Sqrt(-2 * jumpHeight * Physics.gravity.y);
    }

    public void SetAnimatorGroundedParam() {
        animator.SetBool("Grounded", groundCheck.isGrounded);
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        CalculateJumpVelocity();
        SetWalkingSpeed();
    }

    void Update()
    {
        if (GetComponent<Player>().isAttacking) return;
        RecordMovementInput();
        RecordJumpInput();
        RecordLadderInput();
        SetAnimatorGroundedParam();
        if (Input.GetKeyDown(KeyCode.R)) SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }


    void FixedUpdate() {
        if (GetComponent<Player>().isAttacking) return;
        ProcessMovement();
        FixModelRotation();
    }
}
