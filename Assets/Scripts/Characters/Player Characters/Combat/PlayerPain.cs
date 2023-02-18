using System.Collections;
using UnityEngine;

public class PlayerPain : MonoBehaviour
{
    // Only serialized to easily see in inspector for now.
    [SerializeField]
    private int _truePain = 0;
    // Only serialized to easily see in inspector for now.
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
    }

    public void HealPain(int amount)
    {
        _truePain -= amount;
        Slot.UpdatePainBar(EffectivePain);

        if (_truePain < 0)
        {
            _truePain = 0;
            Slot.UpdatePainBar(EffectivePain);
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

        yield return new WaitForSeconds(duration);

        _relief -= amount;
        Slot.UpdatePainBar(EffectivePain);
    }
}