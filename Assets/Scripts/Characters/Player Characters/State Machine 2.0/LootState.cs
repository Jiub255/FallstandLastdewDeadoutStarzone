using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootState : MonoBehaviour
{
	[SerializeField]
	private GameObject _timerObject;

    // For getting the duration of the loot animation. 
    [SerializeField]
    private AnimationClip _lootAnimation;

    [SerializeField]
    private Transform _fillBarTransform;

    [SerializeField]
    private InventorySO _inventorySO;

    [SerializeField]
    private GameObject _idleState;

    private Animator _animator;
    private float _animationLength;
    private float _timer;

    // RunToLootState sets this when switching to this state. 
    public Transform LootContainerTransform { get; set; }

    private void OnEnable()
    {
        // Set animation to looting. 
        _animator = transform.parent.parent.GetComponent<Animator>();
        _animator.SetTrigger("Looting");

        // Activate timer object. 
        _timerObject.SetActive(true);

        // Set timer. 
        _animationLength = _lootAnimation.length;
        _timer = _animationLength; 
    }

    private void OnDisable()
    {
        // Deactivate timer object
        _timerObject.SetActive(false);

        // Set animation back to idle. 
        _animator.SetTrigger("StopLooting");
    }

    private void Update()
    {
        // Increment timer. 
        _timer -= Time.deltaTime;

        // If finished looting, 
        if (_timer <= 0)
        {
            // Add loot to inventory. 
            AddLoot();

            // Activate Idle state. 
            _idleState.SetActive(true);

            // Deactivate this state. 
            gameObject.SetActive(false);
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

    private void AddLoot()
    {
        LootContainer lootContainer = LootContainerTransform.GetComponent<LootContainer>();
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
    }
}