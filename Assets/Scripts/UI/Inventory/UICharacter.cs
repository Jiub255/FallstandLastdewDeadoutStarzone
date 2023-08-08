using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// TODO - Have a MenuSelected property on each PC and then usable items/equipment get applied to them
// when clicked from inventory. 
public class UICharacter : MonoBehaviour
{
	/// <summary>
	/// PCManager listens, updates CurrentMenuPC. 
	/// </summary>
	public static event Action<SOPCData> OnMenuPCChanged;

	[SerializeField]
	private SOTeamData _currentTeamSO;
	[SerializeField]
	private TextMeshProUGUI _pcName;
	[SerializeField]
	private Image _pcImage;
	[SerializeField]
	private GameObject _statTextPrefab;
	[SerializeField]
	private RectTransform _statsParent;

	private SOTeamData CurrentTeamSO { get { return _currentTeamSO; } }
	private TextMeshProUGUI PCName { get { return _pcName; } }
	private Image PCImage { get { return _pcImage; } }
	private GameObject StatTextPrefab { get { return _statTextPrefab; } }
	private RectTransform StatsParent { get { return _statsParent; } }
	private SOPCData CurrentMenuSOPCData { get; set; }

    private void Awake()
    {
		if (CurrentTeamSO.HomePCs.Count > 0)
        {
			CurrentMenuSOPCData = CurrentTeamSO.HomePCs[0];
        }
        else
        {
			Debug.LogWarning("Home PCs list on SOTeamData empty.");
        }        
    }

    private void OnEnable()
    {
		PCStatManager.OnStatsChanged += SetupCharacterPanel;
		PCSelector.OnSelectedNewPC += SetNewMenuPC;

		// Shouldn't need this as long as you don't open the Character UI panel within the first frame. 
//		PCManager.OnAfterPCsInstantiated += SetupCharacterPanel;
		SetupCharacterPanel();
    }

    private void OnDisable()
    {
        PCStatManager.OnStatsChanged -= SetupCharacterPanel;
		PCSelector.OnSelectedNewPC -= SetNewMenuPC;

//		PCManager.OnAfterPCsInstantiated -= SetupCharacterPanel;
	}

	private void SetNewMenuPC(SOPCData pcDataSO)
    {
		// If selected PC changed to null, don't change menu PC. 
		CurrentMenuSOPCData = pcDataSO ? pcDataSO : CurrentMenuSOPCData;
		SetupCharacterPanel();
	}

	public void NextPC()
    {
//	    int currentIndex = CurrentTeamSO.HomePCs.IndexOf(CurrentTeamSO[CurrentMenuSOPCData]);
		int currentIndex = CurrentTeamSO./*GetIndex*/HomePCs.IndexOf(CurrentMenuSOPCData);
//		Debug.Log($"UICharacter's current index: {currentIndex}");

		if (currentIndex != -1)
        {
			currentIndex++;

			if (currentIndex >= CurrentTeamSO.HomePCs.Count) currentIndex = 0;

			SetNewMenuPC(CurrentTeamSO.HomePCs[currentIndex]);

			OnMenuPCChanged?.Invoke(CurrentMenuSOPCData);
        }
        else
        {
			Debug.LogWarning($"{CurrentMenuSOPCData.name} not found on HomeSOPCsList, index returned as -1 from GetIndex. ");
        }
    }

	public void PreviousPC()
    {
//		int currentIndex = CurrentTeamSO.HomePCs.IndexOf(CurrentTeamSO[CurrentMenuSOPCData]);
		int currentIndex = CurrentTeamSO./*GetIndex*/HomePCs.IndexOf(CurrentMenuSOPCData);

		if (currentIndex != -1)
		{
			currentIndex--;

			if (currentIndex < 0) currentIndex = CurrentTeamSO.HomePCs.Count - 1;

			SetNewMenuPC(CurrentTeamSO.HomePCs[currentIndex]);

			OnMenuPCChanged?.Invoke(CurrentMenuSOPCData);
		}
		else
		{
			Debug.LogWarning($"{CurrentMenuSOPCData.name} not found on HomeSOPCsList, index returned as -1 from GetIndex. ");
		}
	}

	private void SetupCharacterPanel()
    {
		ClearStatTextBoxes();

		Debug.Log($"PCName = null: {PCName == null}");
		Debug.Log($"CurrentMenuSOPCData = null: {CurrentMenuSOPCData == null}");

		// Set character name. 
		PCName.text = CurrentMenuSOPCData.name;

		// Set character menu picture.
		PCImage.sprite = CurrentMenuSOPCData.CharacterImage;

		// Set injury and pain. 
		StatTextBox injuryTextBox = Instantiate(StatTextPrefab, StatsParent).GetComponent<StatTextBox>();
		injuryTextBox.SetupText($"Injury: {CurrentMenuSOPCData.Injury}");

		StatTextBox painTextBox = Instantiate(StatTextPrefab, StatsParent).GetComponent<StatTextBox>();
		painTextBox.SetupText($"Pain: {CurrentMenuSOPCData.Pain}");

		// Set stats. 
		Stats stats = CurrentMenuSOPCData.Stats;
		foreach (Stat stat in stats.StatList)
        {
			StatTextBox statTextBox = Instantiate(StatTextPrefab, StatsParent).GetComponent<StatTextBox>();
            statTextBox.SetupText(stat.StatType.ToString() + ": " + stat.ModdedValue);
        }
    }

    private void ClearStatTextBoxes()
    {
        foreach (Transform child in StatsParent)
        {
			Destroy(child.gameObject);
        }
    }
}