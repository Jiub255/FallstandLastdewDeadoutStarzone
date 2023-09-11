using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Save System/SOSaveData", fileName = "Save Data SOs SO")]
public class SOSaveData : ScriptableObject
{
	[SerializeField]
	private List<SaveableSO> _saveableSOs = new();

	public List<SaveableSO> SaveableSOs { get { return _saveableSOs; } }
}