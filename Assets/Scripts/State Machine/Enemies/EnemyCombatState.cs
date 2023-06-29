using UnityEngine;

public class EnemyCombatState : State<EnemyController>
{
    private Transform _target;
    private float _attackRadiusSquared;
    private float _timeBetweenAttacks;

    private float _timer;
    private Transform _transform;

    // For changing animation eventually. Maybe use an animation controlling class instead?
//    private Animator _animator;

    public EnemyCombatState(EnemyController characterController, Transform target, float attackRadius, float timeBetweenAttacks) : base(characterController)
    {
        _target = target;
        _attackRadiusSquared = attackRadius * attackRadius;
        _timeBetweenAttacks = timeBetweenAttacks;

        _timer = 0f;
        _transform = characterController.transform;

        characterController.PathNavigator.StopMoving();

//      _animator = characterController.GetComponentInChildren<Animator>();
    }

    public override void Update()
    {
        _timer += Time.deltaTime;

        if (_timer > _timeBetweenAttacks)
        {
            _timer = 0f;

            // Check if player is still within attack range. 
            if ((_target.position - _transform.position).sqrMagnitude > _attackRadiusSquared)
            {
                // Switch back to EnemyMoveToPCState. 
                // Just chooses another random target for now, but this should pass back target eventually. 
                _stateMachine.ChangeStateTo(_stateMachine.ApproachPC());
                return;
            }

            Attack();
        }
    }

    private void Attack()
    {
        // face PC
        _stateMachine.transform.LookAt(_target);

        // play attack animation
        // _animator.SetTrigger("Attack");
        // boxcast immediately after animation to check if PC is still there

        // JUST FOR TESTING 
        _target.GetComponentInChildren<PlayerInjury>().GetHurt(10);
        Debug.Log("Got injured for 10 by " + _stateMachine.transform.name);
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

    public override void Exit() {}
    public override void FixedUpdate() {}
}