using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInjury : MonoBehaviour
{
    public int Injury { get; private set; } = 0;

    private PlayerPain _playerPain;

    public PCSlot Slot { get; set; } 

    private void Start()
    {
        _playerPain = GetComponent<PlayerPain>();
    }

    public void GetHurt(int damage)
    {
        Injury += damage;
        Slot.UpdateInjuryBar(Injury);
        _playerPain.IncreasePain(damage);

        if (Injury >= 100)
        {
            Debug.Log("Injury >= 100, you died.");
            Die();
        }
    }

    public void Heal(int amount)
    {
        Injury -= amount;
        Slot.UpdateInjuryBar(Injury);
        _playerPain.ReducePain(amount);

        if (Injury < 0)
        {
            Injury = 0;
            Slot.UpdateInjuryBar(Injury);
        }
    }

    private void Die()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}