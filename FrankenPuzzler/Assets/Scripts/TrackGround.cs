using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackGround : MonoBehaviour
{
    public bool isGrounded = false;
    public bool isNearLadder = false;

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
        isNearLadder = Physics.Raycast(transform.position, transform.forward, 1f, LayerMask.NameToLayer("Ladders"), QueryTriggerInteraction.Collide);
    }

    void Update() {
        CheckIfNearLadder();
    }
}
