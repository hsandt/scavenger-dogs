using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// Characters data
[CreateAssetMenu(fileName = "CharactersData", menuName = "Data/Characters Data", order = 2)]
public class CharactersData : ScriptableObject
{
	[Tooltip("Array of character Prefabs (one per color)")]
	public GameObject[] characterPrefabs;
}
