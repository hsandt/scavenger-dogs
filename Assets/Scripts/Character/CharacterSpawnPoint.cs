using UnityEngine;
using System.Collections;

public class CharacterSpawnPoint : MonoBehaviour {

	[SerializeField]
	GameColor color = GameColor.None;  // color of the avatar to spawn at the position of this object

	void Awake () {
		// Debug.Log("Awake SpawnPoint");
	}

	void OnEnable () {
		// Debug.Log("OnEnable SpawnPoint");
	}

	void Start () {
		// Debug.Log("Start SpawnPoint");
		CharacterManager.Instance.RegisterSpawnPoint(transform, color);
	}

}
