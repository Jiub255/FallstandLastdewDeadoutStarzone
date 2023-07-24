using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO - Make Equipment, PCStat, and PainInjury Managers and PCStateMachine into plain c# classes and construct them and keep references here. 
// Do similar with EnemyController, and EnemyStateMachine and EnemyHealthManager. 
// What to do with InventoryManager/PlayerInventoryManager? Put them on S? On each PC? 
// Putting them on GameManager, at least trying it out. 
// Where to put PCStateMachine? Here or PCManager? How to deal with selected vs not? 
public class PCController
{
    private PainInjuryManager _painInjuryManager;
    private PCStatManager _pcStatManager;
    private EquipmentManager _equipmentManager;
    private PCManager _pcManager;

    public PainInjuryManager PainInjuryManager { get { return _painInjuryManager; } }
    public PCStatManager PCStatManager { get { return _pcStatManager; } }
    public EquipmentManager EquipmentManager { get { return _equipmentManager; } }

    public PCController(SOPCData pcDataSO, PCManager pcManager)
    {
        _painInjuryManager = new(pcDataSO);
        _equipmentManager = new(pcDataSO);
        _pcStatManager = new(pcDataSO, _equipmentManager.EquipmentBonuses);

        _equipmentManager.OnEquipmentChanged += _pcStatManager.CalculateStatModifiers; 
//        Stat.OnBaseValueChanged += _pcStatManager.CalculateStatModifiers;
        // TODO - Get healingRate from StatManager. Still figuring out how to structure code first. 
        //        _painInjuryManager.OnStartHealing += () => StartCoroutine(_painInjuryManager.HealingCoroutine(1f));

        _pcManager = pcManager;
        pcManager.OnDisabled += Disable;
    }

    /// <summary>
    /// Call from higher up's monobehaviour OnDisable. 
    /// </summary>
    public void Disable()
    {
        _equipmentManager.OnEquipmentChanged -= _pcStatManager.CalculateStatModifiers;
//        Stat.OnBaseValueChanged -= _pcStatManager.CalculateStatModifiers;
//        _painInjuryManager.OnStartHealing -= () => StartCoroutine(_painInjuryManager.HealingCoroutine(1f));

        // Will this work? I think so, Disable gets called before unsubscribing itself. 
        _pcManager.OnDisabled -= Disable;
    }
}