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

    public void IncreasePain(int amount)
    {
        _truePain += amount;
        Slot.UpdatePainBar(EffectivePain);
    }

    public void ReducePain(int amount)
    {
        _truePain -= amount;
        if (_truePain < 0)
        {
            _truePain = 0;
        }
        Slot.UpdatePainBar(EffectivePain);
    }

    public void TemporarilyRelievePain(int amount, float duration)
    {
        StartCoroutine(RelievePainCoroutine(amount, duration)); 
    }

    private IEnumerator RelievePainCoroutine(int amount, float duration)
    {
        _relief += amount;
        Slot.UpdatePainBar(EffectivePain);

        yield return new WaitForSeconds(duration);

        _relief -= amount;
        Slot.UpdatePainBar(EffectivePain);
    }
}