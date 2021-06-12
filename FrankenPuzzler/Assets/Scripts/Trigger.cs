using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum TriggerType {
    Lever,
    Button,
    Slot
}


public class Trigger : MonoBehaviour
{
    public TriggerType type;
    public GameObject actionObject;

    [HideInInspector] public bool used = false;

    public bool IsLimbEglibleForTrigger(int limbIndex) {
        switch(type) {
            case TriggerType.Lever:
                return (limbIndex == 1 || limbIndex == 2);
            case TriggerType.Slot:
                return limbIndex == 0;
            case TriggerType.Button:
                return true;
            default:
                return false;
        }
    }

    public void PerformAction() {
        used = true;
        actionObject.SendMessage("TriggerAction");
    }
}
