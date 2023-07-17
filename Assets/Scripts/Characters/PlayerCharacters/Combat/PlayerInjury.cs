using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInjury
{
/*//    public int Injury { get; private set; } = 0;

    private PlayerPain _playerPain;
    private SOPC _pcSO;

    public PlayerInjury(PlayerPain playerPain, SOPC pcSO)
    {
        _playerPain = playerPain;
        _pcSO = pcSO;
    }

    public void GetHurt(int damage)
    {
        _pcSO.Injury += damage;
        _playerPain.IncreasePain(damage);

        if (_pcSO.Injury >= 100)
        {
            Debug.Log("Injury >= 100, you died.");
            Die();
        }
    }

    // Should items be able to heal injury during combat? I think no, only painkillers during combat, actual healing takes time and rest. 
    // Medical items can speed up recovery maybe? And medical buildings and PCs with high medical skill? 
    public void Heal(int amount)
    {
        _pcSO.Injury -= amount;
        _playerPain.ReducePain(amount);

        if (_pcSO.Injury < 0)
        {
            _pcSO.Injury = 0;
        }
    }

    private void Die()
    {
//        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }*/
}