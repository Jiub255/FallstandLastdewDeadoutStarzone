using System.Collections;
using UnityEngine;

// TODO - Make this a plain C# class, and pass the EquipmentManager in the constructor? No reason for it to be a MB.
public class PainInjuryManager : MonoBehaviour
{
    private SOPCData _pcSO;
    private int _relief = 0;
    private bool _healing = false;
    private PCSlot _slot;

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

    private void OnEnable()
    {
        _pcSO = GetComponentInParent<PCController>().PCSO;
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
    /// </summary>
    /// <param name="healingRate">Injury points healed per second. </param>
    public void StartHealing(float healingRate)
    {
        _healing = true;
        StartCoroutine(HealingCoroutine(healingRate));
    }

    public void StopHealing()
    {
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

    public void Heal(int amount)
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
        StartCoroutine(RelievePainCoroutine(amount, duration));
    }

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