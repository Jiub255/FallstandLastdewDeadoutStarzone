using UnityEngine;

public class LootState : MonoBehaviour
{
    // For getting the duration of the loot animation. 
    [SerializeField]
    private AnimationClip _lootAnimation;

    [SerializeField]
    private InventorySO _inventorySO;

    [SerializeField]
    private GameObject _idleState;

    private Animator _animator;
    private float _animationLength;
    private float _timer;
	private GameObject _timerObject;
    private Transform _fillBarTransform;

    // RunToLootState sets this when switching to this state. 
    public Transform LootContainerTransform { get; set; }

    private LootContainer _lootContainer;

    private void OnEnable()
    {
        _lootContainer = LootContainerTransform.GetComponentInChildren<LootContainer>();
        // Set LootContainer's IsBeingLooted to true. 
        _lootContainer.IsBeingLooted = true;

        // Move to exact position in front of loot. In Loot container game object, have a looting position child object to mark where to move. 
        transform.parent.parent.position = _lootContainer.LootPosition.position;

        // Face the loot container. 
        transform.parent.parent.LookAt(LootContainerTransform);

        // Set animation to looting. 
        _animator = transform.parent.parent.GetComponentInChildren<Animator>();
        _animator.SetTrigger("Loot");

        // Set timer. 
        _animationLength = _lootAnimation.length;
        _timer = _animationLength;
        _timerObject = transform.parent.parent.GetComponentInChildren<LootTimer>(true).gameObject;
        _fillBarTransform = _timerObject.transform.GetChild(0);

        // Activate timer object. 
        _timerObject.SetActive(true);
    }

    private void OnDisable()
    {
        // Deactivate timer object
        _timerObject.SetActive(false);

        // Set LootContainer's IsBeingLooted to false. 
        _lootContainer.IsBeingLooted = false;
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

            // Set LootContainer's Looted to true. 
            _lootContainer.Looted = true;

            StateSwitcher.Switch(gameObject, _idleState);
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
        LootContainer lootContainer = LootContainerTransform.GetComponentInChildren<LootContainer>();

        foreach (ItemAmount itemAmount in lootContainer.LootItemAmounts)
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
}