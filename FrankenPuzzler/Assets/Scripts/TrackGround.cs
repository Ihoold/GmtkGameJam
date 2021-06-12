using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackGround : MonoBehaviour
{
    public bool isGrounded = false;

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
}
