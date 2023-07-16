using System.Collections;
using UnityEngine;

public class PlayerPain
{
    // Only serialized to easily see in inspector for now.
    [SerializeField]
    private int _truePain = 0;
    // Only serialized to easily see in inspector for now.
    [SerializeField]
    private int _relief = 0;

    public PlayerPain()
    {
        _truePain = 0;
        _relief = 0;
    }

    public int Relief { get { return _relief; } set { _relief = value; } }

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
    }

    public void ReducePain(int amount)
    {
        _truePain -= amount;
        if (_truePain < 0)
        {
            _truePain = 0;
        }
    }
}