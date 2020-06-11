using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using CommonsHelper;
using CommonsPattern;
using UnityConstants;

public class CharacterManager : SingletonManager<CharacterManager> {

	protected CharacterManager () {} // guarantee this will be always a singleton only - can't use the constructor!

	/* resources */
	private Dictionary<GameColor, GameObject> avatarPrefabDict;

	// parameters

	// DEBUG
	/// Set this to false to stop generating a pool of avatars
	/// You won't be able to spawn avatars, only do this to reduce setup time when debugging with
	/// characters already in the scene
	public bool createCharacters = true;

	// pool of heterogeneous characters
	CharacterMaster[] m_Characters = new CharacterMaster[4];
	// public CharacterMaster[] createdCharacters { get { return m_Characters; } }

	// derivated counter
	private int remainingCharacterNb;
	public int RemainingCharacterNb {
		get {
			return remainingCharacterNb;
		}
	}

	Transform[] spawnPoints = new Transform[4];

	// Use this for initialization
	void Awake () {
		SetInstanceOrSelfDestruct(this);

		// load and instantiate avatar prefab for each color (if too costly, link in inspector instead)
		avatarPrefabDict = new Dictionary<GameColor, GameObject>();
		for (int playerNo = 1; playerNo <= 4; ++playerNo) {
			GameColor color = (GameColor) playerNo;

			// load prefab
			string colorName = color.ToString();  // alternative to players' info dict
			avatarPrefabDict[color] = ResourcesUtil.LoadOrFail<GameObject>("Character/Character_" + colorName);

			// instantiate inactive (SEO: better in Start?)
			if (createCharacters) {
				GameObject avatarInstance = avatarPrefabDict[color].InstantiateUnder(Locator.FindWithTag(Tags.CharacterParent).transform);
				m_Characters[playerNo - 1] = avatarInstance.GetComponentOrFail<CharacterMaster>();
				avatarInstance.SetActive(false);
			}
		}

		remainingCharacterNb = 0;
		// the number of players may not be known at the time of Awake so put this somewhere else if needed
	}

	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}

	public void RegisterSpawnPoint(Transform spawnPoint, GameColor color) {
		if (color == GameColor.None) {
			Debug.LogWarning("Cannot register SpawnPoint with Color: None.", spawnPoint.gameObject);
			return;
		}
		int idx = (int) color - 1;
		spawnPoints[idx] = spawnPoint;
	}

	/// Return spawn point for color of value idx + 1
	public Transform GetSpawnPoint(int idx) {
		return spawnPoints[idx];  // may be null if not all spawn points present on stage (esp. in debug)
	}

	/// <summary>Spawn all registered avatars</summary>
	/// <param name="active">If true, the avatars is spawned active</param>
	/// <param name="allowControl">If true, the avatars are immediately controllable</param>
	public GameObject[] SpawnAllCharacters (bool allowControl = true) {
		GameObject[] spawnedCharacterObjects = new GameObject[4];
		for (int i = 0; i < 4; ++i) {
			int playerNo = i + 1;
			spawnedCharacterObjects[i] = SpawnCharacter(playerNo, allowControl);
		}
		return spawnedCharacterObjects;  // or return only actual avatars spawned
	}

	/// <summary>Spawn avatar if spawn point is registered</summary>
	/// <param name="playerNo">Number of the avatar to spawn</param>
	/// <param name="active">If true, the avatar is spawned active</param>
	/// <param name="allowControl">If true, the avatar is immediately controllable</param>
	public GameObject SpawnCharacter (int playerNo, bool allowControl = true) {

		Transform spawnTr = spawnPoints[playerNo - 1];
		if (spawnTr == null) throw ExceptionsUtil.CreateExceptionFormat("No spawn point registered for player {0}.", playerNo);

		// activate stored avatar instance and move to spawn position
		CharacterMaster avatar = m_Characters[playerNo - 1];
		avatar.gameObject.SetActive(true);
		avatar.transform.position = spawnTr.position;

		// stop control if needed (prefab should have CharacterControl active by default)
		if (!allowControl) {
			avatar.GetComponentOrFail<CharacterControl>().StopControl();
		}

		// update derivated counter of avatars registered and active in the scene
		remainingCharacterNb ++;

		// return the spawned avatar as a game object
		return avatar.gameObject;
	}

	/// Enable control for all avatars
	public void StartControlAllCharacters() {
		foreach (var avatar in m_Characters) {
			if (avatar.IsPresent()) avatar.GetComponentOrFail<CharacterControl>().StartControl();
		}
	}

	//// Despawn avatar by player number
	public void DespawnCharacter (int playerID) {
		Debug.LogFormat("Despawning avatar #{0}", playerID);

		// deactivate the actual game object
		m_Characters[playerID].gameObject.SetActive(false);

		// update avatar counter
		remainingCharacterNb --;
	}

	public void DespawnAllCharacters () {
		Debug.Log("DespawnAllCharacters");
		// don't use remainingCharacterNb for the loop condition since it will change during the iterations
		for (int playerNo = 1; playerNo <= 4; playerNo++) {
			var avatar = m_Characters[playerNo - 1];
			if (!avatar.IsPresent()) continue;
			Debug.Log("DespawnAllCharacters: " + avatar + " found, removing it");
			DespawnCharacter(playerNo);
		}
	}

	/// Pause all characters present
	public void PauseAllCharacters () {
		foreach (CharacterMaster characterMaster in m_Characters.Where(av => av.IsPresent())) {
			characterMaster.Pause();
		}
	}

	/// Resume physics for all characters present
	public void ResumeAllCharacters () {
		foreach (CharacterMaster characterMaster in m_Characters.Where(av => av.IsPresent())) {
			characterMaster.Resume();
		}
	}

	/// DEBUG: register character as if spawned
	public void RegisterCharacter (CharacterMaster characterMaster) {
		m_Characters[characterMaster.PlayerId - 1] = characterMaster;
		remainingCharacterNb ++;  // do not forget!
	}

}
