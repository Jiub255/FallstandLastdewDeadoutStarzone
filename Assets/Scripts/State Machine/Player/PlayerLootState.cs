using UnityEngine;

public class PlayerLootState : PlayerState
{

    private float _lootAnimationDuration;
    private InventorySO _inventorySO;
    private LootTimer _lootTimer;

    private float _timer;
//    private GameObject _timerObject;
//    private Transform _fillBarTransform;

    // ApproachLootState sets this when switching to this state. 
    public Transform LootContainerTransform { get; set; }

    private LootContainer _lootContainer;


    public PlayerLootState(PlayerController characterController, LootContainer lootContainer, AnimationClip lootAnimation, InventorySO inventorySO, LootTimer lootTimer) : base(characterController)
    {
        _lootContainer = lootContainer;
        _lootAnimationDuration = lootAnimation.length;
        _inventorySO = inventorySO;
        _lootTimer = lootTimer;

        // Set LootContainer's IsBeingLooted to true. 
        _lootContainer.IsBeingLooted = true;

        // Move to exact position in front of loot. In Loot container game object, have a looting position child object to mark where to move. 
        _stateMachine.transform.root.position = _lootContainer.LootPositionTransform.position;

        // Face the loot container. 
        _stateMachine.transform.root.LookAt(LootContainerTransform);

        // Set animation to looting. 
        _stateMachine.Animator.SetTrigger("Loot");

        // TODO - Do this cleaner. Not sure exactly how yet. 
        // Just get LootTimer from PlayerController serialized field, then call _lootTimer.Tick() in update and enable/disable it that way too. 
        // Set timer. 
/*        _timer = _lootAnimationDuration;
        _timerObject = _stateMachine.transform.parent.parent.GetComponentInChildren<LootTimer>(true).gameObject;
        _fillBarTransform = _timerObject.transform.GetChild(0);*/

        // Activate timer object. 
        _lootTimer.ActivateTimer(true);
//        _timerObject.SetActive(true);
    }

    public override void Exit()
    {
        // Deactivate timer object
        _lootTimer.ActivateTimer(false);

        // Set LootContainer's IsBeingLooted to false. 
        _lootContainer.IsBeingLooted = false;
    }

    public override void Update()
    {
        // Increment timer. 
        _timer -= Time.deltaTime;

        // If finished looting, 
        if (_timer <= 0)
        {
            // Add loot to inventory. 
            AddLoot();

            // Set LootContainer's Looted to true. 
            _lootContainer.Looted = true;

            _stateMachine.ChangeStateTo(_stateMachine.Idle());
        }
        else
        {
            // Get percent of time elapsed
            float percentTime = (_lootAnimationDuration - _timer) / _lootAnimationDuration;
            // Tick timer by that amount. 
            _lootTimer.Tick(percentTime);
        }
    }

    private void AddLoot()
    {
        //LootContainer lootContainer = LootContainerTransform.GetComponentInChildren<LootContainer>();

        foreach (ItemAmount itemAmount in _lootContainer.LootItemAmounts)
        {
            if (itemAmount.ItemSO.GetType() == typeof(UsableItemSO))
            {
                // Check to see if you already have an itemAmount that matches the item,
                //    then add however many
                if (_inventorySO.GetItemAmountFromItemSO(itemAmount.ItemSO) != null)
                {
                    _inventorySO.GetItemAmountFromItemSO(itemAmount.ItemSO).Amount += itemAmount.Amount;
                }
                else
                {
                    _inventorySO.ItemAmounts.Add(itemAmount);
                }
            }
        }
    }

    public override void FixedUpdate() {}
}