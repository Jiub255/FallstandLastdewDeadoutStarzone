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

	/// <summary>
	/// Just using to get GameData for now, probably going to add it to the GameManager eventually and have it pass that down in the constructor. 
	/// </summary>
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

	private SOPCData _currentMenuSOPCData;

	/// <summary>
	/// Just using to get GameData for now, probably going to add it to the GameManager eventually and have it pass that down in the constructor. 
	/// </summary>
	private SOTeamData CurrentTeamSO { get { return _currentTeamSO; } }
	private TextMeshProUGUI PCName { get { return _pcName; } }
	private Image PCImage { get { return _pcImage; } }
	private GameObject StatTextPrefab { get { return _statTextPrefab; } }
	private RectTransform StatsParent { get { return _statsParent; } }
	private SOPCData CurrentMenuSOPCData { get { return _currentMenuSOPCData; } set { _currentMenuSOPCData = value; } }

    private void OnEnable()
    {
		PCStatManager.OnStatsChanged += SetupCharacterPanel;
		PCSelector.OnSelectedNewPC += (pcDataSO) => CurrentMenuSOPCData = pcDataSO;

        SetupCharacterPanel();
    }

    private void OnDisable()
    {
        PCStatManager.OnStatsChanged -= SetupCharacterPanel;
		PCSelector.OnSelectedNewPC -= (pcDataSO) => CurrentMenuSOPCData = pcDataSO;
	}

	public void NextPC()
    {
	    int currentIndex = CurrentTeamSO.HomePCs.IndexOf(CurrentMenuSOPCData);

		if (currentIndex != -1)
        {
			currentIndex++;

			if (currentIndex >= CurrentTeamSO.HomePCs.Count) currentIndex = 0;

			CurrentMenuSOPCData = CurrentTeamSO.HomePCs[currentIndex];

			OnMenuPCChanged?.Invoke(CurrentMenuSOPCData);

			SetupCharacterPanel();
        }
        else
        {
			Debug.LogWarning($"{CurrentMenuSOPCData.name} not found on HomeSOPCsList, index returned as -1 from GetIndex. ");
        }
    }

	public void PreviousPC()
    {
		int currentIndex = CurrentTeamSO.HomePCs.IndexOf(CurrentMenuSOPCData);

		if (currentIndex != -1)
		{
			currentIndex--;

			if (currentIndex > 0) currentIndex = CurrentTeamSO.HomePCs.Count - 1;

			CurrentMenuSOPCData = CurrentTeamSO.HomePCs[currentIndex];

			OnMenuPCChanged?.Invoke(CurrentMenuSOPCData);

			SetupCharacterPanel();
		}
		else
		{
			Debug.LogWarning($"{CurrentMenuSOPCData.name} not found on HomeSOPCsList, index returned as -1 from GetIndex. ");
		}
	}

	private void SetupCharacterPanel()
    {
		ClearStatTextBoxes();

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