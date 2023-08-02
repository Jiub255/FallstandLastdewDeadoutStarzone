using UnityEngine;

public class UIPC : MonoBehaviour
{
    [SerializeField]
    private SOTeamData _teamDataSO;
    [SerializeField]
    protected GameObject SlotPrefab;
    [SerializeField]
    protected Transform SlotParent;

    private void OnEnable()
    {
        PCManager.OnAfterPCsInstantiated += SetupPCSlots;
        _teamDataSO.OnHomeSOPCListChanged += SetupPCSlots;
    }

    private void OnDisable()
    {
        PCManager.OnAfterPCsInstantiated -= SetupPCSlots;
        _teamDataSO.OnHomeSOPCListChanged -= SetupPCSlots;
    }

    /// <summary>
    /// Call this right after instantiating PCs. 
    /// </summary>
    public void SetupPCSlots()
    {
        ClearPCSlots();

        // Populates UI
        foreach (SOPCData pcSO in _teamDataSO.HomePCs)
        {
            PCSlot slot = Instantiate(SlotPrefab, SlotParent).GetComponent<PCSlot>();

            slot.SetupSlot(pcSO);
        }
    }

    private void ClearPCSlots()
    {
        foreach (Transform child in SlotParent)
        {
            Destroy(child.gameObject);
        }
    }
}