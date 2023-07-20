using UnityEngine;

public class LootTimer : MonoBehaviour
{
    private Renderer _renderer;
    private Transform _fillBarTransform;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
        _fillBarTransform = transform.GetChild(0);
    }

    public void ActivateTimer(bool activate)
    {
        _renderer.enabled = activate;
        _fillBarTransform.GetComponent<Renderer>().enabled = activate;
    }

    public void Tick(float percentOfTimeElapsed)
    {
        // Raise fill bar's x scale by percent time elasped. 
        _fillBarTransform.localScale = new Vector3(
            percentOfTimeElapsed,
            _fillBarTransform.localScale.y,
            _fillBarTransform.localScale.z);
        // Move fill bar along x axis, so it stays anchored on one side. 
        _fillBarTransform.localPosition = new Vector3(
            0.55f * (1 - percentOfTimeElapsed),
            _fillBarTransform.localPosition.y,
            _fillBarTransform.localPosition.z);
    }
}