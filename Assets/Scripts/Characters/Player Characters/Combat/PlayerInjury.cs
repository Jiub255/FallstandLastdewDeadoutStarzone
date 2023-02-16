using System;
using UnityEngine;

public class PlayerInjury : MonoBehaviour
{
   // public static event Action<int> OnInjuryChanged;

    public int Injury { get; private set; } = 0;

    private PlayerPain _playerPain;

    public PCSlot Slot { get; set; } 

    private void Start()
    {
        _playerPain = GetComponent<PlayerPain>();
    }

    public void GetHurt(int damage)
    {
        Injury += damage;
        Slot.UpdateInjuryBar(Injury);
        //OnInjuryChanged?.Invoke(_injury);
        _playerPain.AddPain(damage);

        if (Injury >= 100)
        {
            Debug.Log("Injury >= 100, you died.");
            // Die();
        }
    }

    public void Heal(int amount)
    {
        Injury -= amount;
        Slot.UpdateInjuryBar(Injury);
        //OnInjuryChanged?.Invoke(Injury);
        _playerPain.HealPain(amount);

        if (Injury < 0)
        {
            Injury = 0;
            Slot.UpdateInjuryBar(Injury);
            //OnInjuryChanged?.Invoke(Injury);
        }
    }
}