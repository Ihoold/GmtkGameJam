using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float walkingSpeed = 15f;

    float horizontalMoveInput;
    float currentMoveSpeed = 0f;
    Rigidbody rb;

    public void SetWalkingSpeed() {
        currentMoveSpeed = walkingSpeed;
    }

    void RecordMovementInput() {
        horizontalMoveInput = Input.GetAxisRaw("Horizontal");
    }

    void ProcessHorizontalMovement() {
        rb.velocity = new Vector3(horizontalMoveInput * currentMoveSpeed, 0, 0);
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        SetWalkingSpeed();
    }

    void Update()
    {
        RecordMovementInput();
    }

    void FixedUpdate() {
        ProcessHorizontalMovement();
    }
}
