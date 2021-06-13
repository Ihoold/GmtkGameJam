using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum LimbType {
    Arm,
    Leg,
    Head
}


public class Limb : MonoBehaviour
{
    public LimbType type;

    public bool DoesLimbFitInIndex(int index) {
        switch(type) {
            case LimbType.Arm:
                return (index == 1) || (index == 2);
            case LimbType.Leg:
                return (index == 3) || (index == 4);
            case LimbType.Head:
                return index == 0;
            default:
                return false; 
        }
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.Rotate(this.transform.up, 180 * Time.deltaTime);
    }
}
