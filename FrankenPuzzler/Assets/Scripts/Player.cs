using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public float maxHp;
    public Image bloodOverlay;
    public BodyUI bodyUI;
    public bool[] availableLimbs = new bool[5];
    
    Trigger currentTrigger = null;
    bool[] eglibleLimbs = new bool[5];
    float hp;

    void CheckEglibleLimbs() {
        for (int i = 0; i < 5; i++) {
            eglibleLimbs[i] = (availableLimbs[i] && (currentTrigger != null) && currentTrigger.IsLimbEglibleForTrigger(i));
        }
    }

    IEnumerator Bleed() {
        while (IsSomeLimbMissing()) {
            hp -= 0.1f;
            yield return new WaitForSeconds(0.1f);
        }
    }

    bool IsSomeLimbMissing() {
        for (int i = 0; i < 5; i++) {
            if (!availableLimbs[i]) return true;
        }
        return false;
    }

    void UpdateOverlayColor() {
        Color newColor = bloodOverlay.color;
        newColor.a = 1 - (hp / maxHp); 
        bloodOverlay.color = newColor;
    }

    void UpdateLimbsColors() {
        for (int i = 0; i < 5; i++) {
            if (!availableLimbs[i]) {
                bodyUI.bodyParts[i].color = Color.red;
            } else if (eglibleLimbs[i]) {
                bodyUI.bodyParts[i].color = Color.yellow;
            } else {
                bodyUI.bodyParts[i].color = Color.white;
            }
        }
    }

    void Start() {
        hp = maxHp;
    }

    void Update()
    {
        UpdateOverlayColor();
        CheckEglibleLimbs();
        UpdateLimbsColors();
    }

    public void SacraficeLimb(int index) {
        if (!eglibleLimbs[index]) return;    
        availableLimbs[index] = false;
        currentTrigger.PerformAction();
        currentTrigger = null;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Trigger" && !other.GetComponent<Trigger>().used) currentTrigger = other.GetComponent<Trigger>();
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.tag == "Trigger") currentTrigger = null;
    }
}
