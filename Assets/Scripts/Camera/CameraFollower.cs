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

public class CameraFollower : MonoBehaviour
{
    [SerializeField]
    private Transform _cameraLeader;

    [SerializeField]
    private Transform _rotationOrigin;

    [SerializeField]
    private float _smoothTime = 0.3f;

    private Vector3 _velocity = Vector3.zero;

    [SerializeField]
    private float _maxSpeed = 25f;

    private void Update()
    {
        // TODO: Set a max speed? Might be good for camera drag.
        transform.position = Vector3.SmoothDamp(transform.position, _cameraLeader.position, ref _velocity, _smoothTime, _maxSpeed/*Mathf.Infinity*/, Time.unscaledDeltaTime);
        transform.LookAt(_rotationOrigin);
    }
}