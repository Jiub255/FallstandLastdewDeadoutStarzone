using System;
using UnityEngine;
using UnityEngine.InputSystem;

// TODO - Put this functionality on PCManager instead? Had it here so it wouldn't be on individual state machines, 
// so it might be better there. 

/// <summary>
/// Must be constructed after PCs have been instantiated in scene. <br/> 
/// TODO - Better way to do this? Seems unsafe and hard to keep track of. 
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
/// <summary>
/// Need to wait to call this until after PCs have been instantiated, because it subscribes to events called by instantiated MBs on PCs. 
/// </summary>
/// <param name="teamDataSO"></param>
    public PCSelector(SOTeamData teamDataSO)
    {
        TeamDataSO = teamDataSO;

        if (teamDataSO.HomePCs.Count > 0)
        {
            PCLayerMask = teamDataSO.HomePCs[0].PCSharedDataSO.PCLayerMask;
        }
        else
        {
            Debug.LogWarning("No PCs on SOTeamData.HomeSOPCSList. Can't play game without PCs. ");
        }

        MousePositionAction = S.I.IM.PC.Camera.MousePosition;

        // SOPCData click events get subscribed to/unsubscribed from as PCs get added/removed from home team list. 
        // TODO - Do the same for scavenging team list? 
        teamDataSO.OnBeforeAddPCToHomeList += (pcDataSO) => pcDataSO.OnClickPCIcon += () => HandleClick(pcDataSO.PCInstance);
        teamDataSO.OnBeforeRemovePCFromHomeList += (pcDataSO) => pcDataSO.OnClickPCIcon -= () => HandleClick(pcDataSO.PCInstance);
        foreach (SOPCData pcDataSO in teamDataSO.HomePCs)
        {
            pcDataSO.OnClickPCIcon += () => HandleClick(pcDataSO.PCInstance);
        }
//        SOPCData.OnClickPCIcon += HandleClick;

        PCIdleState.OnPCDeselected += () => ChangePC(null);
    }

    public void OnDisable()
    {
        TeamDataSO.OnBeforeAddPCToHomeList -= (pcDataSO) => pcDataSO.OnClickPCIcon += () => HandleClick(pcDataSO.PCInstance);
        TeamDataSO.OnBeforeRemovePCFromHomeList -= (pcDataSO) => pcDataSO.OnClickPCIcon -= () => HandleClick(pcDataSO.PCInstance);
        foreach (SOPCData pcDataSO in TeamDataSO.HomePCs)
        {
            pcDataSO.OnClickPCIcon -= () => HandleClick(pcDataSO.PCInstance);
        }
//        SOPCData.OnClickPCIcon -= HandleClick;

        PCIdleState.OnPCDeselected -= () => ChangePC(null);
    }

    /// <summary>
    /// Returns true if PC clicked, false if not. <br/>
    /// Also runs HandleClick if PC was clicked, which selects PC and centers camera too if double clicked. 
    /// </summary>
    /// <returns></returns>
    public bool CheckIfPCClicked(/*InputAction.CallbackContext context*/)
    {
        Debug.Log($"CheckIfPCClicked called."); 
        // TODO - Change max distance to max zoom or something that makes sense? Probably not a big deal. 
        // Only raycast to PC layer. 
        RaycastHit[] hits = Physics.RaycastAll(
            Camera.main.ScreenPointToRay(MousePositionAction.ReadValue<Vector2>()),
            1000,
            PCLayerMask);
//        Debug.Log($"hit: {hits[0].point}");

        // If there were any hits, they must have been PCs. 
        if (hits.Length > 0)
        {
            // TODO - Handle click on the closest one to camera, not the first one necessarily? Or are they the same? 
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
        Debug.Log("HandleClick called from PCSelector. ");
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
    /// Central place for selecting/deselecting/changing PCs. <br/>
    /// Changes SelectedPC in Current Team SO. Also changes CurrentMenuPC so that when you open the menu, it's on the 
    /// most recently selected character. 
    /// </summary>
    /// <param name="clickedPCInstance"></param>
    private void ChangePC(GameObject PCInstance)
    {
        // TODO - Could do this better with selected bool I think. If clicked pc has Selected == true, then do nothing, otherwise
        // Select this PC and deselect other. Maybe deselect all first then select this one? This is the problem between having a bool
        // Or having a selectedPC field. With the bool, multiple could theoretically be selected, but only want at most one at a time to be. 
        foreach (SOPCData pcDataSO in TeamDataSO.HomePCs)
        {
            // Deselect all PCs first, 
            pcDataSO.Selected = false;
            pcDataSO.SelectedPCIcon.ActivateIcon(false);

            // Then only select the one that was clicked. 
            if (pcDataSO.PCInstance == PCInstance)
            {
                pcDataSO.Selected = true;
                pcDataSO.SelectedPCIcon.ActivateIcon(true);

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

        if (PCInstance == null)
        {
            OnSelectedNewPC?.Invoke(null);
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