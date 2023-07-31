/// <summary>
/// Creates and controls PCStateMachine, PCStatManager, PainInjuryManager, and EquipmentManager. <br/>
/// Do similar with EnemyController, and EnemyStateMachine and EnemyHealthManager. 
/// </summary>
public class PCController
{
    private PCStatManager PCStatManager { get; }
    public PCStateMachine PCStateMachine { get; }
    public PainInjuryManager PainInjuryManager { get; }
    public EquipmentManager EquipmentManager { get; }

    public PCController(SOPCData pcDataSO, SOTeamData currentTeamSO)
    {
        PCStateMachine = new(pcDataSO);
        PainInjuryManager = new(pcDataSO, currentTeamSO);
        EquipmentManager = new(pcDataSO);
        PCStatManager = new(pcDataSO, EquipmentManager.EquipmentBonuses);

        EquipmentManager.OnEquipmentChanged += PCStatManager.CalculateStatModifiers; 
    }

    /// <summary>
    /// Call from PCManager's OnDisable, which is called from GameManager's actual monobehaviour OnDisable. 
    /// </summary>
    public void OnDisable()
    {
        EquipmentManager.OnEquipmentChanged -= PCStatManager.CalculateStatModifiers;
    }
}