using UnityEngine;

public class UIPC : MonoBehaviour
{
    [SerializeField]
    private SOListSOPC _pcSOsSO;
    [SerializeField]
    protected GameObject SlotPrefab;
    [SerializeField]
    protected Transform SlotParent;

    private void OnEnable()
    {
        UIManager.OnOpenedMenu += PopulateInventory;
    }

    private void OnDisable()
    {
        UIManager.OnOpenedMenu -= PopulateInventory;
    }

    private void Start()
    {
        PopulateInventory();
    }

    public void PopulateInventory()
    {
        ClearInventory();

        // Populates UI
        foreach (SOPCData pcSO in _pcSOsSO.HomeSOPCSList)
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