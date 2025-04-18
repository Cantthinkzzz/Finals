using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceEffect : MonoBehaviour
{
    public float bounceHeight= 0.4f;
    public float bounceDuration=0.5f;
    public int bounceCount=2;
    public void startBounce() {
        StartCoroutine(BounceHandler());
    }

    private IEnumerator BounceHandler() {
        Debug.Log("Bouncing start");
        Vector3 startPosition = transform.position;
        float localHeight = bounceHeight;
        float localDuration = bounceDuration;
        for(int i=0; i<bounceCount; i++) {
            //local coroutine for a single bounce
            yield return Bounce(startPosition, localHeight, localDuration/2);
            localHeight *=0.5f;
            localDuration *= 0.8f;
            Debug.Log($"Bouncing, i ={i}");
        }
        transform.position=startPosition;
    }

    private IEnumerator Bounce(Vector3 start, float height, float duration) {
        Vector3 peak = start + height * Vector3.up;
        float elapsed = 0f;
        //to peak
        while(elapsed < duration) {
            transform.position = Vector3.Lerp(start, peak, elapsed/duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        elapsed= 0f;
        //to cliff
        while(elapsed < duration) {
            transform.position = Vector3.Lerp(peak, start, elapsed/duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
    }
}
