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

/*    private PlayerInput playerInput;

    private InputAction mouseRightAction;*/

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

        // [PLAYERHEALTHSCRIPT].onGotHit += ResetLootingState;

        S.I.InputManager.dragCameraAction.performed += ResetLootingState;
    }

    private void OnDisable()
    {
        LootCommand.onClickedLoot -= AssignLootContainer;

        // [PLAYERHEALTHSCRIPT].onGotHit -= ResetLootingState;

        S.I.InputManager.dragCameraAction.performed -= ResetLootingState;
    }

    public void AssignLootContainer(Transform playerTransform, Transform newLootContainer)
    {
        //Debug.Log("Loot container assigned: " + playerTransform.GetInstanceID()
        //    + ", " + newLootContainer.GetInstanceID());

        if (transform == playerTransform)
        {
            lootContainerTransform = newLootContainer;
            lootingState = LootingState.WalkingTowards;
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
        lootingState = LootingState.Looting;

        // Freeze Movement
        S.I.InputManager.selectAction.Disable();

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
            if (inventorySO.HasItem(itemAmount.item) != null)
            {
                inventorySO.HasItem(itemAmount.item).amount += itemAmount.amount;
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
        //Debug.Log("Distance to loot: " + Vector3.Distance(transform.position, lootContainerTransform.position));

        if (Vector3.Distance(transform.position, lootContainerTransform.position) < lootDistance)
        {
            //Debug.Log("Within range of loot");
            return true;
        }
        //Debug.Log("NOT within range of loot");
        return false;
    }

    // Listens for event onHit or right mouse button to reset looting state to NotLooting
    private void ResetLootingState(InputAction.CallbackContext context = new InputAction.CallbackContext())
    {
        if (lootingState != LootingState.NotLooting)
        {
            lootingState = LootingState.NotLooting;
            animator.SetTrigger("StopLooting");
            timerObject.SetActive(false);
            timer = animationLength;
        }
    }
}