using UnityEngine;

public class EnemyCombatState : MonoBehaviour
{
	[SerializeField]
	private float _timeBetweenAttacks = 2f;
    private float _timer;

    [SerializeField]
    private GameObject _enemyMoveToPCState;

    private Animator _animator;

    private float _attackRadius;

    public Transform Target { get; set; }
    private Transform _transform;

    private void OnEnable()
    {
        _animator = transform.parent.parent.GetComponentInChildren<Animator>();
        _transform = _animator.transform;
        _attackRadius = _enemyMoveToPCState.GetComponent<EnemyMoveToPCState>().AttackRadius;
    }

    private void Start()
    {
        _timer = 0f;
    }

    private void Update()
    {
        _timer += Time.deltaTime;

        if (_timer > _timeBetweenAttacks)
        {
            _timer = 0f;

            // Check if player is still within attack range. 
            if (Vector3.Distance(_transform.position, Target.position) > _attackRadius)
            {
                // Switch back to EnemyMoveToPCState. 
                _enemyMoveToPCState.SetActive(true);
                gameObject.SetActive(false);
                return;
            }

            Attack();
        }
    }

    private void Attack()
    {
        // face PC
        transform.LookAt(Target);

        // play attack animation
       // _animator.SetTrigger("Attack");
        // boxcast immediately after animation to check if PC is still there

        // JUST FOR TESTING 
        Target.GetComponentInChildren<PlayerInjury>().GetHurt(10); 
        Debug.Log("Got injured for 10 by " + transform.parent.parent.name); 
    }

    // Animation event at the end of attack animation calls this method
    // TODO: Wont work with readonly animations from mixamo. Can't change them at all.
    // Maybe use a timer instead? Set to the animation's length? 
/*    public void DoDamage()
    {
        RaycastHit hit;
        // boxcast immediately after animation to check if PC is still there
        bool didBoxcastHit = Physics.BoxCast(_enemyCollider.bounds.center, transform.localScale, transform.forward, out hit, transform.rotation, 3f, _playerCharacterLayer);
        // if so, send damage signal to PC
        if (didBoxcastHit)
        {
            OnHitPC.Invoke(_damage, hit.transform);
        }

        // If killed PC, go back to EnemyMoveToPCState. 
    }*/
}