using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BodyUI : MonoBehaviour
{
    public Player player;

    public Image[] bodyParts = new Image[5];

    public void SacraficeLimb(int index) {
        player.SacraficeLimb(index);
    }
}
