using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// TODO - Have a MenuSelected property on each PC and then usable items/equipment get applied to them
// when clicked from inventory. 
public class UICharacter : MonoBehaviour
{
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

	private int _index;

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
		_index++;
		if (_index >= _currentTeamSO.HomeSOPCSList.Count) _index = 0;

		_currentTeamSO.CurrentMenuSOPC = _currentTeamSO.HomeSOPCSList[_index];

		SetupCharacterPanel();
    }

	public void PreviousPC()
    {
		_index--;
		if (_index < 0) _index = _currentTeamSO.HomeSOPCSList.Count - 1;

		_currentTeamSO.CurrentMenuSOPC = _currentTeamSO.HomeSOPCSList[_index];
		
		SetupCharacterPanel();
	}

	private void SetupCharacterPanel()
    {
		ClearStatTextBoxes();

		// Set character name. 
		_pcName.text = _currentTeamSO.CurrentMenuSOPC.name;

		// Set character menu picture.
		_pcImage.sprite = _currentTeamSO.CurrentMenuSOPC.CharacterImage;

		// Set injury and pain. 
		StatTextBox injuryTextBox = Instantiate(_statTextPrefab, _statsParent).GetComponent<StatTextBox>();
		injuryTextBox.SetupText($"Injury: {_currentTeamSO.CurrentMenuSOPC.Injury}");

		StatTextBox painTextBox = Instantiate(_statTextPrefab, _statsParent).GetComponent<StatTextBox>();
		painTextBox.SetupText($"Pain: {_currentTeamSO.CurrentMenuSOPC.Pain}");

		// Set stats. 
		Stats stats = _currentTeamSO.CurrentMenuSOPC.Stats;
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