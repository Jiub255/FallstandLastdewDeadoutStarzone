using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// TODO - Have this be a centralized place to control which character is selected. It's all over the place right now. 
// In PCStateMachine, PCSelector, Transparentizer, probably other places. 
// Do like SOListSOPC with events for every change? Or just have scripts that need it reference this? Events seems better. 

/// <summary>
/// Put PCStateMachine here instead? And have it just run through the dictionary and run Update and whatever else each frame?
/// Then could handle selected vs not selected better maybe? But how? 
/// Put PCStateMachine on PCController, <br/>
/// (first make the state machine a non-MB) <br/>
/// then change its update to depend on this one's. <br/>
/// Copy list of all active PCSODatas, <br/>
/// Then (if it's not null) get CurrentPCSOData, run its state through its machine, but pass a selected bool, then remove from copied list. <br/>
/// Run through the rest of copied list, but pass not selected bool. <br/>
/// Each state will need to take a "selected" bool (but they don't actually have to use it). Kind of like substates. <br/>
/// Seems like a better, more centralized way to deal with selected PC. Tie in other scripts that use it? Or keep using SOCurrentTeam actually. 
/// /// </summary>
public class PCManager : MonoBehaviour
{
    /// <summary>
    /// Any non-MonoBehaviour that gets this PCManager in its constructor can subscribe to this to unsubscribe from its own events. 
    /// </summary>
    public event Action OnDisabled;

    [SerializeField]
	private SOCurrentTeam _currentTeamSO;

    private Dictionary<SOPCData, PCController> _pcDict = new();

    private void OnEnable()
    {
        SpawnPoint.OnSceneStart += InitializeScene;

    }

    /// <summary>
    /// Called by SpawnPoint event on scene load. 
    /// </summary>
    private void InitializeScene(Vector3 spawnPosition)
    {
        InstantiatePCs(spawnPosition);

        PopulateDictionary();

        // Equipment 
        SOEquipmentItem.OnEquip += (item) =>
        {
            _pcDict[_currentTeamSO.CurrentMenuSOPC].EquipmentManager.Equip(item);
        };
        SOEquipmentItem.OnUnequip += (item) =>
        {
            _pcDict[_currentTeamSO.CurrentMenuSOPC].EquipmentManager.Unequip(item);
        };

        // Usable items 
        SORelievePain.OnRelievePainEffect += (amount, duration) =>
        {
            _pcDict[_currentTeamSO.CurrentMenuSOPC].PainInjuryManager.TemporarilyRelievePain(amount, duration);
        };

        S.I.IM.PC.World.SelectOrCenter.canceled/*performed*/ += HandleClick;
    }

    private void OnDisable()
    {
        SpawnPoint.OnSceneStart -= InitializeScene;

        // Equipment 
        SOEquipmentItem.OnEquip -= (item) =>
        {
            _pcDict[_currentTeamSO.CurrentMenuSOPC].EquipmentManager.Equip(item);
        };
        SOEquipmentItem.OnUnequip -= (item) =>
        {
            _pcDict[_currentTeamSO.CurrentMenuSOPC].EquipmentManager.Unequip(item);
        };

        // Usable items 
        SORelievePain.OnRelievePainEffect -= (amount, duration) =>
        {
            _pcDict[_currentTeamSO.CurrentMenuSOPC].PainInjuryManager.TemporarilyRelievePain(amount, duration);
        };

        S.I.IM.PC.World.SelectOrCenter.canceled/*performed*/ -= HandleClick;

        OnDisabled?.Invoke();
    }

    private void HandleClick(InputAction.CallbackContext context)
    {
        if (_currentTeamSO.SelectedPC != null)
        {
            SOPCData pcDataSO = _currentTeamSO[_currentTeamSO.SelectedPC];
//            _pcDict[pcDataSO].PCStateMachine.HandleClick();
        }
    }

    private void InstantiatePCs(Vector3 spawnPosition)
    {
        if (_currentTeamSO.HomeSOPCSList.Count > 0)
        {
            for (int i = 0; i < _currentTeamSO.HomeSOPCSList.Count; i++)
            {
                _currentTeamSO.HomeSOPCSList[i].PCInstance = Instantiate(
                    _currentTeamSO.HomeSOPCSList[i].PCPrefab,
                    new Vector3(3 * i, 0f, 0f) + spawnPosition,
                    Quaternion.identity);
            }

            // Set first instantiated as CurrentMenuSOPC. 
            _currentTeamSO.CurrentMenuSOPC = _currentTeamSO.HomeSOPCSList[0];
        }
        else
        {
            Debug.LogWarning("No PCs on HomeSOPCSList in CurrentTeamSO. Can't play the game without PCs. ");
        }
    }

    private void PopulateDictionary()
    {
        foreach (SOPCData pcDataSO in _currentTeamSO.HomeSOPCSList)
        {
            _pcDict.Add(pcDataSO, new PCController(pcDataSO, this));
//            _pcControllers.Add(new PCController(pcDataSO));
        }
    }
}