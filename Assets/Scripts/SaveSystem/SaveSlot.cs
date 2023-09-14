using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SaveSlot : MonoBehaviour
{
    [Header("Profile")]
    [SerializeField]
    private string _profileID = "";

    [Header("Content")]
    [SerializeField]
    private GameObject _noDataGO;
    [SerializeField]
    private GameObject _dataGO;
    [SerializeField]
    private TextMeshProUGUI _saveFileInfoText;

    [Header("Clear Data Button")]
    [SerializeField]
    private Button _clearButton;

    public bool HasData { get; private set; } = false;

    private Button _saveSlotButton;

    private void Awake()
    {
        _saveSlotButton = GetComponent<Button>();
    }

    public void SetData(GameSaveData data)
    {
        // There's no data for this profileID
        if (data == null)
        {
            HasData = false;
            _noDataGO.SetActive(true);
            _dataGO.SetActive(false);
            _clearButton.gameObject.SetActive(false);
        }
        // There is data for this profileID
        else
        {
            HasData = true;
            _noDataGO.SetActive(false);
            _dataGO.SetActive(true);
            _clearButton.gameObject.SetActive(true);

            // TODO - What to show here instead? At least total time played, last save time, and Name. 
            // Maybe show PC icons of your team or something? 
            _saveFileInfoText.text = "Data set by SaveSlot. This is placeholder text. ";
/*            percentageCompleteText.text = data.GetPercentageComplete().ToString() + "% Complete";
            currentHealthText.text = "Current Health: " + data.currentHealth.ToString();*/
        }
    }

    public string GetProfileID()
    {
        return _profileID;
    }

    public void SetInteractable(bool interactable)
    {
        _saveSlotButton.interactable = interactable;
        _clearButton.interactable = interactable;
    }
}