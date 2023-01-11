using UnityEngine;
using UnityEngine.InputSystem;

enum LootingState
{   
    NotLooting,
    WalkingTowards,
    Looting
}

// Put on each player character
public class LootAction : MonoBehaviour
{
    private LootingState lootingState = LootingState.NotLooting;

    private Transform lootContainerTransform;
    [SerializeField]
    private float lootDistance = 2f;

    private Animator animator;

    [SerializeField]
    private InventorySO inventorySO;

    // Timer stuff
    [SerializeField]
    private AnimationClip lootAnimation;
    
    [SerializeField]
    private GameObject timerObject;

    [SerializeField]
    private Transform fillBarTransform;

    private float animationLength;
    private float timer;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        animationLength = lootAnimation.length;
        timer = animationLength;
    }

    private void Start()
    {
        // listen for event from LootContainer (event will pass character and container transforms)
        LootCommand.onClickedLoot += AssignLootContainer;

        // Before moving
        PCMovement.onMove += ResetLootingState;

        // Before attacking
        // PlayerShoot.onShoot += ResetLootingState;

        // After getting hit
        // [PLAYERHEALTHSCRIPT].onGotHit += ResetLootingState;
    }

    private void OnDisable()
    {
        LootCommand.onClickedLoot -= AssignLootContainer;

        PCMovement.onMove -= ResetLootingState;
        // PlayerShoot.onShoot -= ResetLootingState;
        // [PLAYERHEALTHSCRIPT].onGotHit -= ResetLootingState;
    }

    // Beginning of the looting process
    public void AssignLootContainer(Transform playerTransform, Transform newLootContainer)
    {
        ResetLootingState();

        if (transform == playerTransform)
        {
            lootContainerTransform = newLootContainer;
            ChangeLootingState(LootingState.WalkingTowards);
        }
    }

    private void Update()
    {
       // Debug.Log("Player #" + transform.GetInstanceID() + ", Looting State: " + lootingState.ToString());

        if (lootingState == LootingState.WalkingTowards)
        {
            if (HaveReachedLoot())
            {
                StartLooting();
                // Activate timer game object
                timerObject.SetActive(true);
            }
        }
        else if (lootingState == LootingState.Looting)
        {
            timer -= Time.deltaTime;

            // If finished looting
            if (timer <= 0)
            {
                // Calls ResetLootingState, which resets timer and animation, and deactivates timer object.
                AddLoot();
            }
            else
            {
                // Get percent of time elapsed
                float percentTime = (animationLength - timer) / animationLength;
                // Raise fill bar's x scale by that percent
                fillBarTransform.localScale = new Vector3(
                    percentTime, 
                    fillBarTransform.localScale.y, 
                    fillBarTransform.localScale.z);
                // Move fill bar along x axis, so it stays anchored on one side
                fillBarTransform.localPosition = new Vector3(
                    0.55f * (1 - percentTime), 
                    fillBarTransform.localPosition.y, 
                    fillBarTransform.localPosition.z);
            }
        }
    }

    private void StartLooting()
    {
        // Set state to looting
        ChangeLootingState(LootingState.Looting);

        // Freeze Movement
        S.I.IM.PC.Scavenge.Select.Disable();

        // Face the loot container
        transform.LookAt(lootContainerTransform);

        // Play loot animation/timer
        //    Have Loot animation get called from trigger
        //    How to make it cancellable if you get hit/push cancel
        animator.SetTrigger("Looting");
        // When timer up, call AddLoot.
    }

    // Called by animation event at the end of the loot animation.
    private void AddLoot()
    {
        Debug.Log("Added Loot");

        LootContainer lootContainer = lootContainerTransform.GetComponent<LootContainer>();
        foreach (ItemAmount itemAmount in lootContainer.lootItemAmounts) 
        {
            // Check to see if you already have an itemAmount that matches the item,
            //    then add however many
            if (inventorySO.GetItemAmount(itemAmount.inventoryItemSO) != null)
            {
                inventorySO.GetItemAmount(itemAmount.inventoryItemSO).amount += itemAmount.amount;
            }
            else
            {
                inventorySO.itemAmounts.Add(itemAmount);
            }
        }

        ResetLootingState();
    }

    private bool HaveReachedLoot()
    {
        if (Vector3.Distance(transform.position, lootContainerTransform.position) < lootDistance)
        {
            return true;
        }
        return false;
    }

    // Call this before moving, starting looting, or attacking. (Maybe other things later)
    // Listens for event onHit
    private void ResetLootingState(InputAction.CallbackContext context = new InputAction.CallbackContext()) // is new needed?
    {
        if (lootingState != LootingState.NotLooting)
        {
            ChangeLootingState(LootingState.NotLooting);
            animator.SetTrigger("StopLooting");
            timerObject.SetActive(false);
            timer = animationLength;
        }
    }

    private void ChangeLootingState(LootingState newLootingState)
    {
        lootingState = newLootingState;
    }
}