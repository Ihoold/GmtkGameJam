using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathZone : MonoBehaviour
{
    Player player;

    void Start() {
        player = GameObject.FindObjectOfType<Player>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.tag == "Player") {
            player.Die();
        }
    }
}
