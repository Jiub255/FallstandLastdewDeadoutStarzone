using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Have this component on each PC, and have it new up a pain and injury class and manage them. One less component at least. 
public class PainInjuryManager : MonoBehaviour
{
	public PlayerPain PlayerPain { get; private set; } 
	public PlayerInjury PlayerInjury { get; private set; }

	public PCSlot Slot { get; set; }

    private void OnEnable()
    {
        PlayerPain = new();
        PlayerInjury = new(PlayerPain);
    }

    public void GetHurt(int damage)
    {
        PlayerInjury.GetHurt(damage);
        Slot.UpdateInjuryBar(PlayerInjury.Injury);
        Slot.UpdatePainBar(PlayerPain.EffectivePain);
    }

    // Should items be able to heal injury during combat? I think no, only painkillers during combat, actual healing takes time and rest. 
    // Medical items can speed up recovery maybe? And medical buildings and PCs with high medical skill? 
    public void Heal(int amount)
    {
        PlayerInjury.Heal(amount);
        Slot.UpdateInjuryBar(PlayerInjury.Injury);
        Slot.UpdatePainBar(PlayerPain.EffectivePain);
    }

    public void TemporarilyRelievePain(int amount, float duration)
    {
        StartCoroutine(RelievePainCoroutine(amount, duration));
    }

    private IEnumerator RelievePainCoroutine(int amount, float duration)
    {
        PlayerPain.Relief += amount;
        Slot.UpdatePainBar(PlayerPain.EffectivePain);

        yield return new WaitForSeconds(duration);

        PlayerPain.Relief -= amount;
        Slot.UpdatePainBar(PlayerPain.EffectivePain);
    }
}