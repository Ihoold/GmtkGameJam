using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public float maxHp;
    public float attackDuration;
    public float attackRange;
    public Image bloodOverlay;
    public BodyUI bodyUI;
    public MenuManager menuManager;
    public bool[] availableLimbs = new bool[5];
    
    [HideInInspector] public bool isAttacking = false;
    bool attackInput;
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
                bodyUI.bodyParts[i].color = bodyUI.UnavailableColor;
            } else if (eglibleLimbs[i]) {
                bodyUI.bodyParts[i].color = bodyUI.EglibleColor;
            } else {
                bodyUI.bodyParts[i].color = bodyUI.AvailableColor;
            }
        }
    }

    void Start() {
        hp = maxHp;
    }

    void CheckHp() {
        if (hp < 0) {
            Die();
        }
    }

    public void Die() {
        menuManager.ShowDeathMenu();
    }

    public Enemy CheckForAttackedEnemy() {
        RaycastHit hit;
        Debug.DrawRay(transform.position, transform.right * attackRange, Color.cyan, 1f);
        if (Physics.Raycast(transform.position, transform.right, out hit, attackRange, 1 << LayerMask.NameToLayer("Enemies"))) {
            return hit.collider.GetComponent<Enemy>();
        }
        return null;
    }

    IEnumerator Attack() {
        isAttacking = true;
        float attackStarted = Time.time;
        Enemy enemy = null;
        GetComponent<Rigidbody>().velocity = Vector3.zero;

        while (Time.time < attackStarted + attackDuration) {
            if (enemy == null) {
                enemy = CheckForAttackedEnemy();
                if (enemy != null) enemy.SetAttacked();
            }
            yield return new WaitForEndOfFrame();
        }

        if (enemy != null) enemy.Die();
        isAttacking = false;
    }

    bool AtLeastOneHandIsAvailable() {
        return availableLimbs[1] || availableLimbs[2];
    }

    void CheckAttack() {
        if (!isAttacking && AtLeastOneHandIsAvailable() && Input.GetKey(KeyCode.E)) {
            StartCoroutine(Attack());
        }
    }

    void Update()
    {
        CheckAttack();
        UpdateOverlayColor();
        CheckEglibleLimbs();
        UpdateLimbsColors();
        CheckHp();
    }

    public void SacraficeLimb(int index) {
        if (!eglibleLimbs[index]) return;    
        availableLimbs[index] = false;
        StartCoroutine(Bleed());
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
