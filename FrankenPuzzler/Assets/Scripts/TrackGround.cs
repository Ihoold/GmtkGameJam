using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackGround : MonoBehaviour
{
    public bool isGrounded = false;
    public bool isNearLadder = false;
    public bool isOverLadder = false;

    void OnTriggerEnter(Collider other) {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground")) {
            isGrounded = true;
        }
    }

    void OnTriggerStay(Collider other) {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground")) {
            isGrounded = true;
        }
    }
    
    void OnTriggerExit(Collider other) {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground")) {
            isGrounded = false;
        }
    }

    void CheckIfNearLadder() {
        isNearLadder = Physics.Raycast(transform.position, transform.forward, 1f, 1 << LayerMask.NameToLayer("Ladders"), QueryTriggerInteraction.Collide);
        isOverLadder = Physics.Raycast(transform.position, transform.forward - (transform.up * 0.1f), 1f, 1 << LayerMask.NameToLayer("Ladders"), QueryTriggerInteraction.Collide);
    }

    void Update() {
        CheckIfNearLadder();
    }
}
