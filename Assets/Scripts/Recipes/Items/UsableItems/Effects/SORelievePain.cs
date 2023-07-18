using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Effects/SORelievePain", fileName = "Relieve Pain")]
public class SORelievePain : SOEffect
{
    public static event Action<int, int, float> OnRelievePainEffect;

    [SerializeField]
    private int _painReliefAmount;

    [SerializeField]
    private float _duration = 10f;

    public override void ApplyEffect(SOUsableItem item)
    {
        OnRelievePainEffect?.Invoke(_currentTeamSO.CurrentMenuSOPC.PCInstance.GetInstanceID(), _painReliefAmount, _duration);
    }
}