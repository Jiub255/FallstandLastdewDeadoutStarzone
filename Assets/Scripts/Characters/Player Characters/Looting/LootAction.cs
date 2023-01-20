using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public enum LootingState
{   
    NotLooting,
    WalkingTowards,
    Looting
}

// Put on each player character
public class LootAction : MonoBehaviour
{
    public LootingState LootingState/* { get; set; }*/ = LootingState.NotLooting;

    private Transform _lootContainerTransform;
    [SerializeField]
    private float _lootDistance = 2.5f;

    private Animator _animator;

    [SerializeField]
    private InventorySO _inventorySO;

    // Timer stuff
    [SerializeField]
    private AnimationClip _lootAnimation;
    
    [SerializeField]
    private GameObject _timerObject;

    [SerializeField]
    private Transform _fillBarTransform;

    private float _animationLength;
    private float _timer;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _animationLength = _lootAnimation.length;
        _timer = _animationLength;
    }

    private void Start()
    {
        // listen for event from LootContainer (event will pass character and container transforms)
        LootCommand.OnClickedLoot += AssignLootContainer;

        // Before moving
        PCMovement.OnMove += ResetLootingState;

        // Before attacking
        // PlayerShoot.onShoot += ResetLootingState;

        // After getting hit
        // [PLAYERHEALTHSCRIPT].onGotHit += ResetLootingState;
    }

    private void OnDisable()
    {
        LootCommand.OnClickedLoot -= AssignLootContainer;

        PCMovement.OnMove -= ResetLootingState;
        // PlayerShoot.onShoot -= ResetLootingState;
        // [PLAYERHEALTHSCRIPT].onGotHit -= ResetLootingState;
    }

    // Beginning of the looting process
    public void AssignLootContainer(Transform playerTransform, Transform newLootContainer)
    {
        ResetLootingState();

        if (transform == playerTransform)
        {
            _lootContainerTransform = newLootContainer;

            //ChangeLootingState(LootingState.WalkingTowards);
            StartCoroutine(WaitThenChangeState());
        }
    }

    // I hate this
    // Or could check lootingState in PCMovement, and don't call onMove if you're in NotLooting.
    private IEnumerator WaitThenChangeState()
    {
        yield return new WaitForEndOfFrame();

        ChangeLootingState(LootingState.WalkingTowards);
    }

    private void Update()
    {
        Debug.Log("Looting State: " + LootingState.ToString());

        if (LootingState == LootingState.WalkingTowards)
        {
            if (HaveReachedLoot())
            {
                StartLooting();
            }
        }
        else if (LootingState == LootingState.Looting)
        {
            _timer -= Time.deltaTime;

            // If finished looting
            if (_timer <= 0)
            {
                // Calls ResetLootingState, which resets timer and animation, and deactivates timer object.
                AddLoot();
            }
            else
            {
                // Get percent of time elapsed
                float percentTime = (_animationLength - _timer) / _animationLength;
                // Raise fill bar's x scale by that percent
                _fillBarTransform.localScale = new Vector3(
                    percentTime, 
                    _fillBarTransform.localScale.y, 
                    _fillBarTransform.localScale.z);
                // Move fill bar along x axis, so it stays anchored on one side
                _fillBarTransform.localPosition = new Vector3(
                    0.55f * (1 - percentTime), 
                    _fillBarTransform.localPosition.y, 
                    _fillBarTransform.localPosition.z);
            }
        }
    }

    private void StartLooting()
    {
        // Set state to looting
        ChangeLootingState(LootingState.Looting);

        // Freeze Movement
        //S.I.IM.PC.Scavenge.Select.Disable();

        // Face the loot container
        transform.LookAt(_lootContainerTransform);

        // Play loot animation/timer
        //    Have Loot animation get called from trigger
        //    How to make it cancellable if you get hit/push cancel
        _animator.SetTrigger("Looting");
        // When timer up, call AddLoot.

        // Activate timer game object
        _timerObject.SetActive(true);
    }

    // Called by animation event at the end of the loot animation.
    private void AddLoot()
    {
        LootContainer lootContainer = _lootContainerTransform.GetComponent<LootContainer>();
        foreach (ItemAmount itemAmount in lootContainer.LootItemAmounts) 
        {
            // Check to see if you already have an itemAmount that matches the item,
            //    then add however many
            if (_inventorySO.GetItemAmountFromItemSO(itemAmount.InventoryItemSO) != null)
            {
                _inventorySO.GetItemAmountFromItemSO(itemAmount.InventoryItemSO).Amount += itemAmount.Amount;
            }
            else
            {
                _inventorySO.ItemAmounts.Add(itemAmount);
            }
        }

        ResetLootingState();
    }

    private bool HaveReachedLoot()
    {
        if (Vector3.Distance(transform.position, _lootContainerTransform.position) < _lootDistance)
        {
            return true;
        }
        return false;
    }

    // Call this before moving, starting looting, or attacking. (Maybe other things later)
    // Listens for event onHit
    private void ResetLootingState(InputAction.CallbackContext context = new InputAction.CallbackContext()) // is new needed? Default value?
    {
        if (LootingState != LootingState.NotLooting)
        {
            ChangeLootingState(LootingState.NotLooting);
            _animator.SetTrigger("StopLooting");
            _timerObject.SetActive(false);
            _timer = _animationLength;
        }
    }

    private void ChangeLootingState(LootingState newLootingState)
    {
        LootingState = newLootingState;
    }
}