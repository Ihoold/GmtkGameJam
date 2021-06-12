using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAside : MonoBehaviour
{
    public Vector3 direction;
    public float time;

    IEnumerator TimedMove() {
        float startTime = Time.time;
        Vector3 startPosition = transform.position;

        while (Time.time < startTime + time) {
            transform.position = Vector3.Lerp(startPosition, startPosition + direction, (Time.time - startTime) / time);
            yield return new WaitForEndOfFrame();
        }
    }

    public void TriggerAction() {
        StartCoroutine(TimedMove());
    }
}
