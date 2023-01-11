using UnityEngine;

[CreateAssetMenu(menuName = "PC/Selected PC SO", fileName = "New Selected PC SO")]
public class SelectedPCSO : ScriptableObject
{
	[HideInInspector]
	public GameObject selectedPCGO;
}