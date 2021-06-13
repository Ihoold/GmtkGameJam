using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathZone : MonoBehaviour
{

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.tag == "Player") {
            collision.collider.gameObject.GetComponent<Player>().Die();
        }
    }
}
