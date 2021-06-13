using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFall : MonoBehaviour
{
    bool isActive = false;
    
    void OnTriggerStay(Collider collider) {
        if (!isActive) return;

        if (collider.gameObject.layer == LayerMask.NameToLayer("Enemies")) {
            Enemy enemy = collider.GetComponent<Enemy>();
            enemy.SetAttacked();
            Rigidbody rb = collider.GetComponent<Rigidbody>();
            rb.isKinematic = false;
        }
    }

    public void TriggerAction() {
        isActive = true;
    }
}
