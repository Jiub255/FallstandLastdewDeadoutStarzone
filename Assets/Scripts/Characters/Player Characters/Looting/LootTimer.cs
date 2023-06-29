using UnityEngine;

public class LootTimer : MonoBehaviour
{
    [SerializeField]
    private Renderer _renderer;
    [SerializeField]
    private Transform _fillBarTransform;

    public void ActivateTimer(bool activate)
    {
        _renderer.enabled = activate;
        _fillBarTransform.GetComponent<Renderer>().enabled = activate;
    }

    public void Tick(float percentTime)
    {
        // Raise fill bar's x scale by percent time elasped. 
        _fillBarTransform.localScale = new Vector3(
            percentTime,
            _fillBarTransform.localScale.y,
            _fillBarTransform.localScale.z);
        // Move fill bar along x axis, so it stays anchored on one side. 
        _fillBarTransform.localPosition = new Vector3(
            0.55f * (1 - percentTime),
            _fillBarTransform.localPosition.y,
            _fillBarTransform.localPosition.z);
    }
}