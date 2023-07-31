using UnityEngine;

public class UIPC : MonoBehaviour
{
    [SerializeField]
    private SOTeamData _pcSOsSO;
    [SerializeField]
    protected GameObject SlotPrefab;
    [SerializeField]
    protected Transform SlotParent;

    private void OnEnable()
    {
        PopulateInventory();
    }

    public void PopulateInventory()
    {
        ClearInventory();

        // Populates UI
        foreach (SOPCData pcSO in _pcSOsSO.HomePCs)
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