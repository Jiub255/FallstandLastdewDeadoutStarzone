using System;
using UnityEngine;
using UnityEngine.InputSystem;

// TODO - Put this functionality on PCManager instead? Had it here so it wouldn't be on individual state machines, 
// so it might be better there. 

/// <summary>
/// How to handle click? <br/>
/// Put PCSelector here, maybe as a plain C# class?
/// Subscribe click event to that, and also run HandleClick in selected PC if there is one? 
/// </summary>
public class PCSelector/* : MonoBehaviour*/
{
    /// <summary>
    /// CameraMoveRotate listens, centers on PC. 
    /// </summary>
    public static event Action<Transform> OnDoubleClickPC;
    /// <summary>
    /// Transparentizer listens, updates its _currentPC Transform. <br/>
    /// PCManager listens, updates its _currentSelectedPC and _currentMenuPC SOPCDatas. 
    /// </summary>
    public static event Action<SOPCData> OnSelectedNewPC;

    private SOTeamData TeamDataSO { get; }
    private LayerMask PCLayerMask { get; }
    private float DoubleClickTimeLimit { get; } = 0.5f;
    private float LastClickTime { get; set; }
    private int FirstClickedObjectID { get; set; }
    private InputAction MousePositionAction { get; }

//    private void Start()
    public PCSelector(SOTeamData teamDataSO)
    {
        TeamDataSO = teamDataSO;

        if (teamDataSO.HomeSOPCSList.Count > 0)
        {
            PCLayerMask = teamDataSO.HomeSOPCSList[0].PCSharedDataSO.PCLayerMask;
        }
        else
        {
            Debug.LogWarning("No PCs on SOTeamData.HomeSOPCSList. Can't play game without PCs. ");
        }

        MousePositionAction = S.I.IM.PC.Camera.MousePosition;

        SOPCData.OnSelectPC += HandleClick;
        PCIdleState.OnPCDeselected += () => ChangePC(null);
    }

    public void OnDisable()
    {
        SOPCData.OnSelectPC -= HandleClick;
        PCIdleState.OnPCDeselected -= () => ChangePC(null);
    }

    /// <summary>
    /// Returns true if PC clicked, false if not. <br/>
    /// Also runs HandleClick if PC was clicked, which selects PC and centers camera too if double clicked. 
    /// </summary>
    /// <returns></returns>
    public bool CheckIfPCClicked(/*InputAction.CallbackContext context*/)
    {
        // Only raycast to PC layer. 
        RaycastHit[] hits = Physics.RaycastAll(
            Camera.main.ScreenPointToRay(MousePositionAction.ReadValue<Vector2>()),
            1000,
            PCLayerMask);

        // If there were any hits, they must have been PCs. 
        if (hits.Length > 0)
        {
            // This handles double/single clicking. 
            HandleClick(hits[0].transform.gameObject);
            return true;
        }

        return false;
    }

    /// <summary>
    /// Selects PC if single click, selects and centers camera on PC if double click. 
    /// </summary>
    private void HandleClick(GameObject pcInstance)
    {
        float currentClickTime = Time.realtimeSinceStartup;

        // Double click 
        if ((currentClickTime - LastClickTime) < DoubleClickTimeLimit)
        {
            Debug.Log("Double click");
            // If the second click was on the same PC as first click, center on that PC. 
            if (pcInstance.GetInstanceID() == FirstClickedObjectID)
            {
                // CameraMoveRotate listens, centers on PC. 
                OnDoubleClickPC?.Invoke(pcInstance.transform);
            }
            // If second click was on a different PC, treat it like a single click. 
            else
            {
                // Select PC. 
                ChangePC(pcInstance); 

                // Get reference to first object clicked, to compare on second click. 
                FirstClickedObjectID = pcInstance.GetInstanceID(); 
            }
        }
        // Single Click 
        else
        {
            Debug.Log("Single click");
            // Select PC. 
            ChangePC(pcInstance); 

            // Get reference to first object clicked, to compare on second click. 
            FirstClickedObjectID = pcInstance.GetInstanceID(); 
        }

        LastClickTime = currentClickTime;
    }
    /// <summary>
    /// Changes SelectedPC in Current Team SO. Also changes CurrentMenuPC so when you open the menu, it's on the 
    /// most recently selected character. 
    /// </summary>
    /// <param name="clickedPCInstance"></param>
    private void ChangePC(GameObject PCInstance)
    {
        // TODO - Could do this better with selected bool I think. If clicked pc has Selected == true, then do nothing, otherwise
        // Select this PC and deselect other. Maybe deselect all first then select this one? This is the problem between having a bool
        // Or having a selectedPC field. With the bool, multiple could theoretically be selected, but only want at most one at a time to be. 
        foreach (SOPCData pcDataSO in TeamDataSO.HomeSOPCSList)
        {
            // Deselect all PCs first, 
            pcDataSO.Selected = false;

            // Then only select the one that was clicked. 
            if (pcDataSO.PCInstance == PCInstance)
            {
                pcDataSO.Selected = true;

                // TODO - Keep these properties? Or just send an event and let scripts that need this data update their own properties? 
                // Seems more performant the event way, instead of constantly getting these references from the SO. 
                // Only really needed in a few scripts anyways: Tranparentizer need SelectedPC for its transform,
                // and PCManager needs CurrentMenuSOPC for dealing with item events. 
                // The change character buttons in the inventory menus can send a change CurrentMenuPC event too. So it can be changed in menu, not
                // just from clicking on PC or PC icon. 
                OnSelectedNewPC?.Invoke(pcDataSO);

/*                _currentTeamSO.SelectedPC = PCInstance;
                _currentTeamSO.CurrentMenuSOPC = pcDataSO;*/
            }
        }

/*        // If clicked PC is NOT your selected PC, or selected PC is null, 
        if (_currentTeamSO.SelectedPC == null || _currentTeamSO.SelectedPC != clickedPCInstance)
        {
            // If there is a currently selected PC, set its Selected to false. 
            if (_currentTeamSO.SelectedPC != null)
            {
                _currentTeamSO.SelectedPC.GetComponent<PCStateMachine>().SetSelected(false);
            }

            // Set clicked PC's Selected to true.
            if (clickedPCInstance != null)
            {
                clickedPCInstance.GetComponent<PCStateMachine>().SetSelected(true);
            }
         
            _currentTeamSO.SelectedPC = clickedPCInstance;
            // Also set PC as current menu PC so you always see your most recently selected character when you open the inventory. 
            if (clickedPCInstance != null)
            {
                // TODO - How to get to PCSOData from instance with new system? 
                // Had circular references before with SO -> instance -> statemachine -> SO. Need to fix anyway. 
                // Have a monobehaviour on PCInstance that sends event to CurrentTeamSO through PCManager or something? 
                _currentTeamSO.CurrentMenuSOPC = clickedPCInstance.GetComponent<PCStateMachine>().PCSO;
            }
        }*/
    }
}