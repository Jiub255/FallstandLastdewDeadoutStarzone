using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SOObjData : ScriptableObject
{
	[field: SerializeField]
	public GameObject Prefab { get; set; }

	[field: SerializeField]
	public List<SOComponent> SOComponents = new List<SOComponent>();
}