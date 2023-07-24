using System;
using System.Collections.Generic;
using UnityEngine;

// TODO - Have this be a centralized place to control which character is selected. It's all over the place right now. 
// In PCStateMachine, PCSelector, Transparentizer, probably other places. 
// Do like SOListSOPC with events for every change? Or just have scripts that need it reference this? Events seems better. 
public class PCManager : MonoBehaviour
{
    /// <summary>
    /// Any non-MonoBehaviour that gets this PCManager in its constructor can subscribe to this to unsubscribe from its own events. 
    /// </summary>
    public event Action OnDisabled;

    [SerializeField]
	private SOCurrentTeam _currentTeamSO;

    private Dictionary<SOPCData, PCController> _pcSOsAndControllers = new();

    private void OnEnable()
    {
        SpawnPoint.OnSceneStart += InitializeScene;
    }

    private void OnDisable()
    {
        SpawnPoint.OnSceneStart -= InitializeScene;
        OnDisabled?.Invoke();
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
            _pcSOsAndControllers[_currentTeamSO.CurrentMenuSOPC].EquipmentManager.Equip(item);
        };
        SOEquipmentItem.OnUnequip += (item) =>
        {
            _pcSOsAndControllers[_currentTeamSO.CurrentMenuSOPC].EquipmentManager.Unequip(item);
        };

        // Usable items 
        SORelievePain.OnRelievePainEffect += (amount, duration) =>
        {
            _pcSOsAndControllers[_currentTeamSO.CurrentMenuSOPC].PainInjuryManager.TemporarilyRelievePain(amount, duration);
        };
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
            _pcSOsAndControllers.Add(pcDataSO, new PCController(pcDataSO, this));
//            _pcControllers.Add(new PCController(pcDataSO));
        }
    }
}