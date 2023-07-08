using UnityEngine;

public class FocalPointFollow : MonoBehaviour
{
	[SerializeField]
	private Transform _focalPointLeader;
    [SerializeField, Range(0.1f, 1.0f)]
    private float _smoothTime = 0.3f;

    private Vector3 _velocity = Vector3.zero;
    private Transform _transform;

    private void Awake()
    {
        _transform = transform;
    }

    private void Update()
    {
        _transform.position = Vector3.SmoothDamp(
            _transform.position,
            _focalPointLeader.position,
            ref _velocity,
            _smoothTime,
            /*_maxSpeed*/Mathf.Infinity,
            Time.unscaledDeltaTime);
    }
}