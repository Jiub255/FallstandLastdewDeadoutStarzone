using System;
using System.Collections;
using UnityEngine;

public class PlayerPain : MonoBehaviour
{
   // public static event Action<int> OnPainChanged;

    // Only serialized to easily see in inspector for now.
    [SerializeField]
    private int _truePain = 0;
    [SerializeField]
    private int _relief = 0;

    public PCSlot Slot { get; set; }

    public int EffectivePain 
    { 
        get 
        {
            int pain = _truePain - _relief;
            if (pain < 0)
            {
                pain = 0;
            }
            return pain; 
        }  
    }

    public void AddPain(int damage)
    {
        _truePain += damage;
        Slot.UpdatePainBar(EffectivePain);
        //OnPainChanged?.Invoke(EffectivePain);
    }

    public void HealPain(int amount)
    {
        _truePain -= amount;
        Slot.UpdatePainBar(EffectivePain);
        //OnPainChanged?.Invoke(EffectivePain);

        if (_truePain < 0)
        {
            _truePain = 0;
            Slot.UpdatePainBar(EffectivePain);
            // OnPainChanged?.Invoke(EffectivePain);
        }
    }

    public void RelievePain(int amount, float duration)
    {
        StartCoroutine(PainKiller(amount, duration)); 
    }

    IEnumerator PainKiller(int amount, float duration)
    {
        _relief += amount;
        Slot.UpdatePainBar(EffectivePain);
        //OnPainChanged?.Invoke(EffectivePain);

        yield return new WaitForSeconds(duration);

        _relief -= amount;
        Slot.UpdatePainBar(EffectivePain);
        //OnPainChanged?.Invoke(EffectivePain);
    }
}