public class PCStateFactory
{
	private PCStateMachine _context;

    // State instances (to avoid creating new one each time it is needed)
    // ONLY WORKS if no data stored in concrete states. Otherwise that data will get recycled. 
    // If putting data in concrete states, "return new _pC[StateName]State;" in each return method, 
    //     so states will have fresh data. 

    // Superstates 
    private PCDoingNothingState _pCDoingNothingState;
    private PCLootingState _pCLootingState;
    private PCFightingState _pCFightingState;

    // Substates 
    private PCMovingSubstate _pCMovingSubstate;
    private PCIdlingSubstate _pCIdlingSubstate;
    private PCLootingSubstate _pCLootingSubstate;
    private PCAttackingSubstate _pCAttackingSubstate;

	public PCStateFactory(PCStateMachine currentContext)
    {
        _context = currentContext;

        // Superstates 
        _pCDoingNothingState = new PCDoingNothingState(_context, this);
        _pCLootingState = new PCLootingState(_context, this);
        _pCFightingState = new PCFightingState(_context, this);

        // Substates 
        _pCMovingSubstate = new PCMovingSubstate(_context, this);
        _pCIdlingSubstate = new PCIdlingSubstate(_context, this);
        _pCLootingSubstate = new PCLootingSubstate(_context, this);
        _pCAttackingSubstate = new PCAttackingSubstate(_context, this);
    }

    // Superstate Getters 
    public PCBaseState GetDoingNothingState() => _pCDoingNothingState;
    public PCBaseState GetLootingState() => _pCLootingState;
    public PCBaseState GetFightingState() => _pCFightingState;
    
    // Substate Getters 
    public PCBaseState GetMovingSubstate() => _pCMovingSubstate;
    public PCBaseState GetIdlingSubstate() => _pCIdlingSubstate;
    public PCBaseState GetLootingSubstate() => _pCLootingSubstate;
    public PCBaseState GetAttackingSubstate() => _pCAttackingSubstate;
}