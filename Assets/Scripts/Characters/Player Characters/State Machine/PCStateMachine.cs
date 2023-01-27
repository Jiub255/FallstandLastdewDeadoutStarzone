using UnityEngine;

// Put all methods/variables/properties that the concrete states use here. 
// Put logic inside of concrete states (Can access machine and factory from states).
// Put this on each PC. 
public class PCStateMachine : MonoBehaviour
{
    // MACHINE/FACTORY VARIABLES  
    private PCBaseState _currentState;
    private PCStateFactory _stateFactory;

    // STATE-SPECIFIC VARIABLES/METHODS 

    // MOVEMENT - Put stuff from PCMovement here. 
    // VARIABLES 

    // METHODS 


    // LOOTING - Put stuff from LootAction here. 
    // VARIABLES 
    private Transform _lootContainerTransform;
    [SerializeField]
    private float _lootDistance = 2.5f;
    private Animator _animator;
    [SerializeField]
    private InventorySO _inventorySO;
    // Loot Timer 
    [SerializeField]
    private AnimationClip _lootAnimation;
    [SerializeField]
    private GameObject _timerObject;
    [SerializeField]
    private Transform _fillBarTransform;
    private float _animationLength;
    private float _timer;
    // METHODS 


    // COMBAT - Build combat system in here. 
    // VARIABLES 


    // METHODS 



    // PROPERTIES 
    // MACHINE/FACTORY REFERENCES 
    public PCBaseState CurrentState { get { return _currentState; } set { _currentState = value; } }
    public PCStateFactory StateFactory { get { return _stateFactory; } }
    // MOVEMENT 


    // LOOTING 
    public Transform LootContainerTransform { get { return _lootContainerTransform; } set { _lootContainerTransform = value; } }
    public float LootDistance { get { return _lootDistance; } set { _lootDistance = value; } }
    public Animator Animator { get { return _animator; } set { _animator = value; } }
    public InventorySO InventorySO { get { return _inventorySO; } set { _inventorySO = value; } }
    public AnimationClip LootAnimation { get { return _lootAnimation; } set { _lootAnimation = value; } }
    public GameObject TimerObject { get { return _timerObject; } set { _timerObject = value; } }
    public Transform FillBarTransform { get { return _fillBarTransform; } set { _fillBarTransform = value; } }
    public float AnimationLength { get { return _animationLength; } set { _animationLength = value; } }
    public float Timer { get { return _timer; } set { _timer = value; } }
    // COMBAT 



    private void Awake()
    {
        // Each machine has it's own instance of the factory. 
        // TODO: Could maybe do static states? As long as there's no data in the states themselves. 
        _stateFactory = new PCStateFactory(this);

        _currentState = _stateFactory.GetDoingNothingState();
        _currentState.EnterState();

        // MOVEMENT 

        // LOOTING 
        _animator = GetComponent<Animator>();
        _animationLength = _lootAnimation.length;
        _timer = _animationLength;
        // COMBAT 

    }

    private void Update()
    {
        Debug.Log("Current State: " + _currentState.ToString());
        if (_currentState.CurrentSuperState != null)
            Debug.Log("Current Superstate; " + _currentState.CurrentSuperState.ToString());
        if (_currentState.CurrentSubState != null)
            Debug.Log("Current Substate; " + _currentState.CurrentSubState.ToString());

        _currentState.UpdateStates();

        // Put "Any State" logic in here? Gets run no matter what state you're in. 
    }

    // Needed? Useless or harmful?
/*    private void OnDisable()
    {
        // Or ExitStates() if using it. 
        _currentState.ExitState();
    }*/
}