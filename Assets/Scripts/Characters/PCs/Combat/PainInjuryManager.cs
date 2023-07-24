//using System;
using System.Collections;
//using System.Threading;
using UnityEngine;

// TODO - Make this a plain C# class, and pass the EquipmentManager in the constructor? No reason for it to be a MB.
public class PainInjuryManager
{
    private SOPCData _pcSO;
    private PCSlot _slot;

    // Should these two be in SOPCData instead? 
    private int _relief = 0;
    private bool _healing = false;
//    private Timer _timer; 

    /// <summary>
    /// Gets set by PCSlot, in SetupSlot(), right after being instantiated. 
    /// </summary>
    public PCSlot Slot { get { return _slot; } set { _slot = value; } }

    private int Pain
    {
        get
        {
            int pain = _pcSO.Injury - _relief;
            if (pain < 0)
            {
                pain = 0;
            }
            return pain;
        }
    }

    public PainInjuryManager(SOPCData pcDataSO)
    {
        _pcSO = pcDataSO;
    }

    /// <summary>
    /// Adds <c>damage</c> amount to Injury, calls <c>Die()</c> if over 100. Updates PC slot's UI. 
    /// </summary>
    /// <param name="damage"></param>
    public void GetHurt(int damage)
    {
        _pcSO.Injury += damage;
        _pcSO.Pain = Pain;

        if (_pcSO.Injury >= 100)
        {
            _pcSO.Injury = 100;
            _pcSO.Pain = Pain;
            Debug.Log("Injury >= 100, you died.");
            Die();
        }

        Slot.UpdateInjuryBar(_pcSO.Injury);
        Slot.UpdatePainBar(Pain);
    }

    // TODO - Have a "Healing" bool, and a healing rate.  
    // Should items be able to heal injury during combat? I think no, only painkillers during combat, actual healing takes time (outside of combat) and rest. 
    // Medical items can speed up recovery. And medical buildings and PCs with high medical skill. 
    /// <summary>
    /// When healing, call heal every x seconds based on healing rate until full or healing is set to false.
    /// <br/> Called by getting in bed to rest? 
    /// </summary>
    /// <remarks>
    /// TODO - Get healingRate from StatManager somehow, instead of taking a float. 
    /// </remarks>
    /// <param name="healingRate">Injury points healed per second. </param>
    public void StartHealing(float healingRate)
    {
        _healing = true;

        S.I.StartCoroutine(HealingCoroutine(healingRate));

        // TESTING trying c# timer instead of coroutine.
/*        int healingPeriodMS = Mathf.RoundToInt(1000 / healingRate);
        _timer = new Timer(HealTimer, 1, 0, healingPeriodMS);
        _timer = new Timer(new TimerCallback(HealTimer));*/
    }

/*    void HealTimer(object state)
    {
        Debug.Log($"HealTimer called at {DateTime.Now.TimeOfDay}");
        int amount = (int)state;
        Heal(amount);
    }*/
    
    /// <summary>
    /// Called by getting out of bed? 
    /// </summary>
    public void StopHealing()
    {
//        _timer.Dispose();

        if (_healing) _healing = false;
    }

    private IEnumerator HealingCoroutine(float healingRate)
    {
        while (_healing)
        {
            yield return HealCoroutine(healingRate);
        }
    }

    private IEnumerator HealCoroutine(float healingRate)
    {
        yield return new WaitForSeconds(1f / healingRate);
        Heal(1);
    }

    private void Heal(int amount)
    {
        _pcSO.Injury -= amount;
        _pcSO.Pain = Pain;

        if (_pcSO.Injury < 0)
        {
            _pcSO.Injury = 0;
            _pcSO.Pain = Pain;
        }

        Slot.UpdateInjuryBar(_pcSO.Injury);
        Slot.UpdatePainBar(Pain);
    }

    /// <summary>
    /// Not sure what to do here yet. 
    /// </summary>
    private void Die()
    {
//        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    /// <summary>
    /// Called by painkiller items' events. Actually called by events in classes that inherit SOEffect, which get called by SOUsableItem. 
    /// </summary>
    public void TemporarilyRelievePain(int amount, float duration)
    {
        S.I.StartCoroutine(RelievePainCoroutine(amount, duration));

/*        _relief += amount;

        int msDuration = Mathf.RoundToInt(duration * 1000);
        Timer timer = null;
        (Timer, int) timerAndAmount = new(timer, amount);
        timer = new(delegate { StopPainRelief(timerAndAmount); }, null, 0, msDuration);*/
    }

/*    private void StopPainRelief(object state)
    {
        (Timer, int) timerAndAmount = ((Timer, int))state;

        _relief -= timerAndAmount.Item2;
        timerAndAmount.Item1.Dispose();
    }*/

    private IEnumerator RelievePainCoroutine(int amount, float duration)
    {
        _relief += amount;
        _pcSO.Pain = Pain;
        Slot.UpdatePainBar(Pain);

        yield return new WaitForSeconds(duration);

        _relief -= amount;
        _pcSO.Pain = Pain;
        Slot.UpdatePainBar(Pain);
    }
}