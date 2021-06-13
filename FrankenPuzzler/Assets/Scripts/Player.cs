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
    public GameObject[] limbs = new GameObject[5];
    public GameObject darknessUI;
    
    [HideInInspector] public bool isAttacking = false;
    bool attackInput;
    Animator animator;
    Limb currentLimb;
    Trigger currentTrigger = null;
    bool[] eglibleLimbs = new bool[5];
    bool[] droppedLimbs = new bool[5];
    float hp;

    void CheckEglibleLimbs() {
        for (int i = 0; i < 5; i++) {
            eglibleLimbs[i] = (availableLimbs[i] && (currentTrigger != null) && currentTrigger.IsLimbEglibleForTrigger(i));
        }
    }

    void CheckDroppedLimbs() {
        for (int i = 0; i < 5; i++) {
            droppedLimbs[i] = (!availableLimbs[i] && (currentLimb != null) && currentLimb.DoesLimbFitInIndex(i));
        }
    }

    void HideUnavailableLimbs() {
        for (int i = 0; i < 5; i++) {
            limbs[i].SetActive(availableLimbs[i]);
        }
        GetComponent<Movement>().ToggleCrawling(!availableLimbs[3] || !availableLimbs[4]);
        darknessUI.SetActive(!availableLimbs[0]);
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
            if (droppedLimbs[i]) {
                bodyUI.bodyParts[i].color = bodyUI.DroppedColor;
            } else if (!availableLimbs[i]) {
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
        animator = GetComponent<Animator>();
    }

    void CheckHp() {
        if (hp < 0) {
            Die();
        }
    }

    public void Die() {
        menuManager.ShowDeathMenu();
    }

    public bool isInFront(GameObject obj){
        return Vector3.Dot(-GetComponent<Movement>().modelRotation.forward, transform.InverseTransformPoint(obj.transform.position)) > 0;
    }

    public Enemy CheckForAttackedEnemy() {
        Collider[] colliders = Physics.OverlapSphere(transform.position, attackRange, 1 << LayerMask.NameToLayer("Enemies"));
        foreach (Collider col in colliders) {
            if (isInFront(col.gameObject)) {
                return col.GetComponent<Enemy>();
            }
        }
        return null;
    }

    IEnumerator Attack() {
        isAttacking = true;
        float attackStarted = Time.time;
        Enemy enemy = null;
        GetComponent<Rigidbody>().velocity = Vector3.zero;

        animator.SetTrigger((availableLimbs[1]) ? "AttackingR" : "AttackingL");

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
        CheckDroppedLimbs();
        HideUnavailableLimbs();
        UpdateLimbsColors();
        CheckHp();
    }

    public void RestoreLimb(int index) {
        availableLimbs[index] = true;
        GameObject.Destroy(currentLimb.gameObject);
        currentLimb = null;
    }

    public void SacraficeLimb(int index) {
        if (droppedLimbs[index]) RestoreLimb(index);
        if (!eglibleLimbs[index]) return;    
        availableLimbs[index] = false;
        StartCoroutine(Bleed());
        currentTrigger.PerformAction();
        currentTrigger = null;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Trigger" && !other.GetComponent<Trigger>().used) currentTrigger = other.GetComponent<Trigger>();
        if (other.gameObject.tag == "Limb") currentLimb = other.GetComponent<Limb>();
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.tag == "Trigger") currentTrigger = null;
        if (other.gameObject.tag == "Limb") currentLimb = null;
    }
}
