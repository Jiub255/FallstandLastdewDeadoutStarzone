public class PCStateFactory
{
	private PCStateMachine _context;

    // State instances (to avoid creating new one each time it is needed)
    // ONLY WORKS if no data stored in concrete states. Otherwise that data will get recycled. 
    // If putting data in concrete states, return new _pC[StateName]State; in each return method, 
    //     so states will have fresh data. 
    private PCIdleState _pCIdleState;
    private PCWalkState _pCWalkState;
    private PCLootState _pCLootState;
    private PCSelectedState _pCSelectedState;
    private PCNotSelectedState _pCNotSelectedState;

	public PCStateFactory(PCStateMachine currentContext)
    {
        _context = currentContext;

        _pCIdleState = new PCIdleState(_context, this);
        _pCWalkState = new PCWalkState(_context, this);
        _pCLootState = new PCLootState(_context, this);
        _pCSelectedState = new PCSelectedState(_context, this);
        _pCNotSelectedState = new PCNotSelectedState(_context, this);
    }

    public PCBaseState Idle() 
    {
        return _pCIdleState;
    }
    public PCBaseState Walk()
    {
        return _pCWalkState;
    }
    public PCBaseState Loot() 
    {
        return _pCLootState;
    }
    public PCBaseState Selected() 
    {
        return _pCSelectedState;
    }
    public PCBaseState NotSelected() 
    {
        return _pCNotSelectedState;
    }
}