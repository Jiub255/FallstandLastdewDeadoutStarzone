using System.Collections;
using UnityEngine;

public class PainInjuryManager
{
    /// <summary>
    /// Gets set by PCSlot, in SetupSlot(), right after being instantiated. 
    /// </summary>
    public PCSlot Slot { get; set; }
    private SOPCData PCDataSO { get; }
    /// <summary>
    /// Just to get Healing Rate. Might do differently later. 
    /// </summary>
    private SOCurrentTeam CurrentTeamSO { get; }
    private int Pain
    {
        get
        {
            int pain = PCDataSO.Injury - PCDataSO.Relief;
            if (pain < 0)
            {
                pain = 0;
            }
            return pain;
        }
    }

    public PainInjuryManager(SOPCData pcDataSO, SOCurrentTeam currentTeamSO)
    {
        PCDataSO = pcDataSO;
        CurrentTeamSO = currentTeamSO;
    }

    /// <summary>
    /// Adds <c>damage</c> amount to Injury, calls <c>Die()</c> if over 100. Updates PC slot's UI. 
    /// </summary>
    /// <param name="damage"></param>
    public void GetHurt(int damage)
    {
        PCDataSO.Injury += damage;
        PCDataSO.Pain = Pain;

        if (PCDataSO.Injury >= 100)
        {
            PCDataSO.Injury = 100;
            PCDataSO.Pain = Pain;
            Debug.Log("Injury >= 100, you died.");
            Die();
        }

        Slot.UpdateInjuryBar(PCDataSO.Injury);
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
    public void StartHealing()
    {
        PCDataSO.Healing = true;

        S.I.StartCoroutine(HealingCoroutine(CurrentTeamSO.HealingRate));
    }
    
    /// <summary>
    /// Called by getting out of bed? 
    /// </summary>
    public void StopHealing()
    {
        if (PCDataSO.Healing) PCDataSO.Healing = false;
    }

    private IEnumerator HealingCoroutine(float healingRate)
    {
        while (PCDataSO.Healing)
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
        PCDataSO.Injury -= amount;
        PCDataSO.Pain = Pain;

        if (PCDataSO.Injury < 0)
        {
            PCDataSO.Injury = 0;
            PCDataSO.Pain = Pain;
        }

        Slot.UpdateInjuryBar(PCDataSO.Injury);
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
    }

    private IEnumerator RelievePainCoroutine(int amount, float duration)
    {
        PCDataSO.Relief += amount;
        PCDataSO.Pain = Pain;
        Slot.UpdatePainBar(Pain);

        yield return new WaitForSeconds(duration);

        PCDataSO.Relief -= amount;
        PCDataSO.Pain = Pain;
        Slot.UpdatePainBar(Pain);
    }
}