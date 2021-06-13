using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Transform[] points;
    public float speed;

    bool attacked = false;
    int currentPoint;
    Rigidbody rb;

    void Start() {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate() {
        if (attacked) return;
        if (rb.position == points[currentPoint].position) currentPoint = (currentPoint + 1) % points.Length;
        rb.MovePosition(Vector3.MoveTowards(rb.position, points[currentPoint].position, speed * Time.deltaTime));
    }

    public void Die() {
        this.gameObject.SetActive(false);
    }

    internal void SetAttacked()
    {
        attacked = true;
    }
}
