using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Transform[] points;
    public float speed;
    public GameObject droppedLimb = null;

    bool attacked = false;
    int currentPoint;
    Rigidbody rb;
    Animator animator;

    void Start() {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    void FixedUpdate() {
        if (attacked) {
            animator.SetBool("Moving", false);
            return;
        }
        animator.SetBool("Moving", true);
        if (rb.position == points[currentPoint].position) currentPoint = (currentPoint + 1) % points.Length;

        float direction = (rb.position - points[currentPoint].position).x;
        if (direction > 0.1f) {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, 270, 0), 720f * Time.deltaTime);
        } else if (direction < 0.1f) {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, 90, 0), 720f * Time.deltaTime);
        }

        rb.MovePosition(Vector3.MoveTowards(rb.position, points[currentPoint].position, speed * Time.deltaTime)); 
    }

    public void Die() {
        this.gameObject.SetActive(false);
        GameObject.Instantiate(droppedLimb, transform.position, Quaternion.identity);
    }

    internal void SetAttacked()
    {
        attacked = true;
    }
}
