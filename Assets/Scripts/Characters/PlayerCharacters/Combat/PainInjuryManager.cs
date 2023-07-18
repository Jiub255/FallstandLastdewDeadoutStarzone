using System.Collections;
using UnityEngine;

// Have this component on each PC, and have it new up a pain and injury class and manage them. One less component at least. 
public class PainInjuryManager : MonoBehaviour
{
    [SerializeField]
    private SOPC _pcSO;

    // Only serialized to easily see in inspector for now.
    [SerializeField]
    private int _relief = 0;

	public PCSlot Slot { get; set; }
    public int Relief { get { return _relief; } }

    public int EffectivePain
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
        SORelievePain.OnRelievePainEffect += TemporarilyRelievePain;
    }

    private void OnDisable()
    {
        SORelievePain.OnRelievePainEffect -= TemporarilyRelievePain;
    }

    public void GetHurt(int damage)
    {
        _pcSO.Injury += damage;

        if (_pcSO.Injury >= 100)
        {
            _pcSO.Injury = 100;
            Debug.Log("Injury >= 100, you died.");
            Die();
        }

        Slot.UpdateInjuryBar(_pcSO.Injury);
        Slot.UpdatePainBar(EffectivePain);
    }

    // Should items be able to heal injury during combat? I think no, only painkillers during combat, actual healing takes time (outside of combat) and rest. 
    // Medical items can speed up recovery maybe? And medical buildings and PCs with high medical skill? 
    public void Heal(int amount)
    {
        _pcSO.Injury -= amount;

        if (_pcSO.Injury < 0)
        {
            _pcSO.Injury = 0;
        }

        Slot.UpdateInjuryBar(_pcSO.Injury);
        Slot.UpdatePainBar(EffectivePain);
    }

    private void Die()
    {
        //        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    /// <summary>
    /// Called by painkiller items' events. 
    /// </summary>
    /// <remarks>
    /// Actually called by events in classes that inherit SOEffect, which get called by SOUsableItem. 
    /// </remarks>
    /// <param name="PCInstanceID"></param>
    /// <param name="amount"></param>
    /// <param name="duration"></param>
    public void TemporarilyRelievePain(int PCInstanceID, int amount, float duration)
    {
        if (transform.root.gameObject.GetInstanceID() == PCInstanceID)
        {
            StartCoroutine(RelievePainCoroutine(amount, duration));
        }
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