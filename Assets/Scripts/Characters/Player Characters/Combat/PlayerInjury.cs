using System;
using UnityEngine;

public class PlayerInjury : MonoBehaviour
{
    public static event Action<int> OnInjuryChanged;

    // Only serialized to easily see in inspector for now.
    [SerializeField]
    private int _injury = 0;

    private PlayerPain _playerPain;

    private void Start()
    {
        _playerPain = GetComponent<PlayerPain>();
    }

    public void GetHurt(int damage)
    {
        _injury += damage;
        OnInjuryChanged?.Invoke(_injury);
        _playerPain.AddPain(damage);

        if (_injury >= 100)
        {
            Debug.Log("Injury >= 100, you died.");
            // Die();
        }
    }

    public void Heal(int amount)
    {
        _injury -= amount;
        OnInjuryChanged?.Invoke(_injury);
        _playerPain.HealPain(amount);

        if (_injury < 0)
        {
            _injury = 0;
            OnInjuryChanged?.Invoke(_injury);
        }
    }
}