using UnityEngine;

[CreateAssetMenu(fileName = "New Selected PC SO", menuName = "Scriptable Object/Player Characters/Selected PC SO")]
public class SelectedPCSO : ScriptableObject
{
	[HideInInspector]
	public GameObject SelectedPCGO;
}