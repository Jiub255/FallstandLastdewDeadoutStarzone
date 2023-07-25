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
	private SOCurrentTeam _currentTeamSO;
	[SerializeField]
	private TextMeshProUGUI _pcName;
	[SerializeField]
	private Image _pcImage;
	[SerializeField]
	private GameObject _statTextPrefab;
	[SerializeField]
	private RectTransform _statsParent;

	// TODO - Keep reference to currentMenu SOPCData instead? 
	private int _index;
	private SOPCData _currentMenuSOPCData;

	public SOPCData CurrentMenuSOPCData { get { return _currentMenuSOPCData; } private set { _currentMenuSOPCData = value; } }

    private void OnEnable()
    {
		PCStatManager.OnStatsChanged += SetupCharacterPanel;

        SetupCharacterPanel();
    }

    private void OnDisable()
    {
        PCStatManager.OnStatsChanged -= SetupCharacterPanel;
    }

    public void NextPC()
    {
	    int currentIndex = _currentTeamSO.HomeSOPCSList.IndexOf(CurrentMenuSOPCData);

		if (currentIndex != -1)
        {
			currentIndex++;

			if (currentIndex >= _currentTeamSO.HomeSOPCSList.Count) currentIndex = 0;

			CurrentMenuSOPCData = _currentTeamSO.HomeSOPCSList[currentIndex];

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
		int currentIndex = _currentTeamSO.HomeSOPCSList.IndexOf(CurrentMenuSOPCData);

		if (currentIndex != -1)
		{
			currentIndex--;

			if (currentIndex > 0) currentIndex = _currentTeamSO.HomeSOPCSList.Count - 1;

			CurrentMenuSOPCData = _currentTeamSO.HomeSOPCSList[currentIndex];

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
		_pcName.text = CurrentMenuSOPCData.name;

		// Set character menu picture.
		_pcImage.sprite = CurrentMenuSOPCData.CharacterImage;

		// Set injury and pain. 
		StatTextBox injuryTextBox = Instantiate(_statTextPrefab, _statsParent).GetComponent<StatTextBox>();
		injuryTextBox.SetupText($"Injury: {CurrentMenuSOPCData.Injury}");

		StatTextBox painTextBox = Instantiate(_statTextPrefab, _statsParent).GetComponent<StatTextBox>();
		painTextBox.SetupText($"Pain: {CurrentMenuSOPCData.Pain}");

		// Set stats. 
		Stats stats = CurrentMenuSOPCData.Stats;
		foreach (Stat stat in stats.StatList)
        {
			StatTextBox statTextBox = Instantiate(_statTextPrefab, _statsParent).GetComponent<StatTextBox>();
            statTextBox.SetupText(stat.StatType.ToString() + ": " + stat.ModdedValue);
        }
    }

    private void ClearStatTextBoxes()
    {
        foreach (Transform child in _statsParent)
        {
			Destroy(child.gameObject);
        }
    }
}