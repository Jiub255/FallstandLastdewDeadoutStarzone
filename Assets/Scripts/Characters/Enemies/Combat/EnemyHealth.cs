using UnityEngine;

// Treat health like player pain kinda, as in the enemies stats get weaker as they get more and more hurt. 
public class EnemyHealth : MonoBehaviour
{
	// TODO - Get this from enemy data SO. 
	[SerializeField]
	private int _maxHealth = 100;

	private int CurrentHealth { get; set; }
	private int MaxHealth { get { return _maxHealth; } }

    private void Awake()
    {
		CurrentHealth = MaxHealth;
    }

	/// <summary>
	/// Lowers enemy health by <c>damage</c>. Attacking PC passes reference to their combat state,
	/// so OnEnemyDied can be called by enemy if GetHurt kills the enemy. 
	/// </summary>
	/// <param name="damage"></param>
	/// <param name="attackingPC">Reference to the PC's combat state </param>
    public void GetHurt(int damage, PCCombatState attackingPC)
    {
		CurrentHealth -= damage;

		Debug.Log(transform.parent.name + "'s health: " + CurrentHealth);

		if (CurrentHealth <= 0)
        {
			CurrentHealth = 0;

			Die(attackingPC);
        }
    }

	private void Die(PCCombatState attackingPC)
    {
		Debug.Log("Health <= 0, " + transform.parent.name + " died.");

		// Send signal to attacking PC that enemy is dead, so they can switch states. 
		attackingPC.OnEnemyKilled();

		// Destroy enemy after a delay. 
		Destroy(transform.parent.gameObject, 1f);

		// Disable enemy collider so that PlayerIdleState doesn't detect it during death delay. 
		transform.parent.GetComponent<Collider>().enabled = false;
    }
}