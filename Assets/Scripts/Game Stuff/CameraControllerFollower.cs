using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Something like: transform.localPosition = Vector3.MoveTowards(transform.localPosition, Vector3.zero, speed * Time.deltaTime)
// Do it like this instead though:
    // But what if the target keeps moving?
/*    float lerpTime = 1f;
    float currentLerpTime;*/
// float perc = currentLerpTime / lerpTime;
// transform.position = Vector3.Lerp(startPos, endPos, perc);
// Or:
// float t = currentLerpTime / lerpTime;
// t = Mathf.Sin(t * Mathf.PI * 0.5f); Or: t = t*t*t * (t * (6f*t - 15f) + 10f);
// Have speed be faster when you're far away from target (parent), and slower when you're closer.

public class CameraControllerFollower : MonoBehaviour
{
    [SerializeField]
    private Transform cameraLeader;

    [SerializeField]
    private Transform rotationOrigin;

    [SerializeField]
    private float smoothTime = 0.3f;

    private Vector3 velocity = Vector3.zero;

    private void Update()
    {
        transform.position = Vector3.SmoothDamp(transform.position, cameraLeader.position, ref velocity, smoothTime);
        transform.LookAt(rotationOrigin);
        //transform.localPosition = Vector3.MoveTowards(transform.localPosition, Vector3.zero, speed * Time.deltaTime);
    }
}