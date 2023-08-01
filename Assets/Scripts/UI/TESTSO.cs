using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class TESTSO : ScriptableObject
{
	[SerializeField]
	private GameObject _testGO;

	public GameObject testGO { get { return _testGO; } }
}