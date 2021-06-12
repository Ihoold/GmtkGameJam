using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public float hp;
    public Image bloodOverlay;

    void Update()
    {
        Color newColor = bloodOverlay.color;
        newColor.a = 1 - (hp / 100); 
        bloodOverlay.color = newColor;
    }
}
