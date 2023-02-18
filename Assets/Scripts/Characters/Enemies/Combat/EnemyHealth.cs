using UnityEngine;

// Treat health like player pain kinda, as in the enemies stats get weaker as they get more and more hurt. 
public class EnemyHealth : MonoBehaviour
{
	[SerializeField]
	private int _maxHealth = 100;
	private int _health = 0;

    private void Awake()
    {
		_health = _maxHealth;
    }

    public void GetHurt(int damage, CombatState attackingPC)
    {
		_health -= damage;

		Debug.Log(transform.parent.name + "'s health: " + _health);

		if (_health <= 0)
        {
			_health = 0;

			Die(attackingPC);
        }
    }

	private void Die(CombatState attackingPC)
    {
		Debug.Log("Health <= 0, " + transform.parent.name + " died.");

		// Send signal to attacking PC that enemy is dead, so they can switch states. 
		attackingPC.OnEnemyKilled();

		Destroy(transform.parent.gameObject);
    }
}