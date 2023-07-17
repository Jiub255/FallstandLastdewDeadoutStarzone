using UnityEngine;

// TODO - Just get rid of PlayerPain class? Since true pain is always equal to injury, could just have 
// a relief field and EffectivePain property in PlayerInjury. Or even just in the manager, no need for these classes. 
// Unless there is going to be a pain tolerance stat. But that seems over involved. 
public class PlayerPain
{
/*    // Only serialized to easily see in inspector for now.
    [SerializeField]
    private int _truePain = 0;*//*
    // Only serialized to easily see in inspector for now.
    [SerializeField]
    private int _relief = 0;

    private SOPC _pcSO;

    public PlayerPain(SOPC pcSO)
    {
        _relief = 0;

        _pcSO = pcSO;
    }

    public int Relief { get { return _relief; } set { _relief = value; } }

    public int EffectivePain 
    { 
        get 
        {
            int pain = _pcSO.TruePain - _relief;
            if (pain < 0)
            {
                pain = 0;
            }
            return pain; 
        }  
    }

    public void IncreasePain(int amount)
    {
        _pcSO.TruePain += amount;
    }

    public void ReducePain(int amount)
    {
        _pcSO.TruePain -= amount;
        if (_pcSO.TruePain < 0)
        {
            _pcSO.TruePain = 0;
        }
    }*/
}