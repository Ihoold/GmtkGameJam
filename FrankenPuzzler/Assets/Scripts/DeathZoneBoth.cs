using UnityEngine;

public class DeathZoneBoth : MonoBehaviour
{
    Player player;

    void Start() {
        player = GameObject.FindObjectOfType<Player>();
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Player") {
            player.Die();
            return;
        }

        if (collider.gameObject.layer == LayerMask.NameToLayer("Enemies")) {
            collider.gameObject.GetComponent<Enemy>().Die();
        }
    }
}
