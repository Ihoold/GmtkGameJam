using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelWin : MonoBehaviour
{
    public bool isActive = false;

    public void TriggerAction() {
        isActive = true;
    }

    void OnTriggerEnter(Collider collider) {
        if (!isActive) return;
        if (collider.gameObject.tag != "Player") return;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
