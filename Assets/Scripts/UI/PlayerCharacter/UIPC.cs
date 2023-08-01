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
        _teamDataSO.OnHomeSOPCListChanged += PopulateInventory; 

        PopulateInventory();
    }

    private void OnDisable()
    {
        _teamDataSO.OnHomeSOPCListChanged -= PopulateInventory;
    }

    public void PopulateInventory()
    {
        ClearInventory();

        // Populates UI
        foreach (SOPCData pcSO in _teamDataSO.HomePCs)
        {
            PCSlot slot = Instantiate(SlotPrefab, SlotParent).GetComponent<PCSlot>();

            slot.SetupSlot(pcSO);
        }
    }

    private void ClearInventory()
    {
        foreach (Transform child in SlotParent)
        {
            Destroy(child.gameObject);
        }
    }
}