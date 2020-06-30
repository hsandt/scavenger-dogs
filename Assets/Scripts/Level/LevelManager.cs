//#define DEBUG_LEVEL_MANAGER

using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

using CommonsPattern;
using UnityConstants;

public class LevelManager : SingletonManager<LevelManager> {

	protected LevelManager () {} // guarantee this will be always a singleton only - can't use the constructor!

	/* resources */
	// prefab dictionary for keys (loaded on Awake)
	private Dictionary<GameColor, GameObject> keyPrefabDict = new Dictionary<GameColor, GameObject>();

	// array that store stage scene values
	int[] stageSceneIndices = new int[] {Scenes.SampleScene};

	// debug settings
	[SerializeField]
	bool generateOnStart = true;  // generate level on start
	[SerializeField]
	bool generateFromScene = true;  // generate level from scene instead of TXT file
	[SerializeField]
	bool spawnScrapsOnStart = true;

	[SerializeField]
	ScrapPoolManager scrapPoolManager = null;  // scrap multi-pool manager (only LevelManager has direct access to it)

	// [SerializeField]
	// Transform itemParent;  // transform of object tagged ItemParent

	// level information

	// track number of keys to take per color
	public Dictionary<GameColor, int> requiredNbScrapsPerColorDict = new Dictionary<GameColor, int>();

	// key spawn positions
	Dictionary<GameColor, List<Transform>> scrapSpawnPoints = new Dictionary<GameColor, List<Transform>>();

	// Use this for initialization
	void Awake () {
		SetInstanceOrSelfDestruct(this);

		foreach (GameColor gameColor in GameData.colors) {
			LevelManager.Instance.requiredNbScrapsPerColorDict[gameColor] = 0;
			scrapSpawnPoints[gameColor] = new List<Transform>();
		}

	}

	void Start () {
		if (spawnScrapsOnStart) {
			// spawn all keys
			LevelManager.Instance.SpawnAllScraps();
		}
	}

	/* Generate a game level with PCG */
	public void GenerateLevel (int stageIdx) {
		/* add PCG here */
		// build the grid
		// if (generateOnStart) {
			if (generateFromScene) {
				SceneManager.LoadScene(stageSceneIndices[stageIdx], LoadSceneMode.Additive);
				// TODO: also update grid, by reading the tilemap content
				// grid.BuildWithStagePrefab(stagePrefab);
			}
		// }
	}

	/* Clear the game level */
	public void ClearLevel () {
		// clear the grid
		if (generateOnStart) {
			if (generateFromScene) {
				// TODO: equivalent of clear for objects?
			}
		}
	}

	/// Register all scrap spawn points under transform
	public void RegisterAllScrapSpawnPoints(Transform eventParent) {
		ScrapSpawnPoint[] scrapSpawnPoints = eventParent.GetComponentsInChildren<ScrapSpawnPoint>();
		foreach (var scrapSpawnPoint in scrapSpawnPoints) {
			RegisterKeySpawnPoint(scrapSpawnPoint.transform, scrapSpawnPoint.color);
		}
	}

	/// Register a new key spawn point
	public void RegisterKeySpawnPoint(Transform spawnPoint, GameColor color) {
		if (color == GameColor.None) {
			Debug.LogWarning("Cannot register KeySpawnPoint with Color: None.", spawnPoint.gameObject);
			return;
		}
#if DEBUG_LEVEL_MANAGER
		Debug.LogFormat("Registering {0} for scrap {1}", spawnPoint, color);
#endif
		scrapSpawnPoints[color].Add(spawnPoint);
	}

	// REFACTOR: make private when you are sure the Grid is not spawning one by one anymore
	/// Spawn a scrap with the provided information
	/// <param name="position">spawn position</param>
	/// <param name="size">Scrap size</param>
	/// <param name="color">Scrap color</param>
	public Scrap SpawnAndRegisterScrap (Vector2 position, GameColor color) {
		Scrap scrap = scrapPoolManager.SpawnScrap(position, color);
		if (scrap == null) {
			// pool starvation
			return null;
		}
		// increment number of keys to pick for that color
		LevelManager.Instance.requiredNbScrapsPerColorDict[color] ++;
#if DEBUG_LEVEL_MANAGER
		Debug.LogFormat("Registering key {0} #{1}", color, LevelManager.Instance.requiredNbScrapsPerColorDict[color]);
#endif
		return scrap;
	}

	public void SpawnAllScraps() {
		// IMPROVE: instead of always playing with the same avatars/colors in order, allow playing with specific colors since their powers will differ
		//	in later versions
		// int playerNb = SessionManager.Instance.playerNb;
#if DEBUG_LEVEL_MANAGER
		Debug.Log("SpawnAllScraps");
#endif
		int playerNb = 4;
		for (int i = 0; i < playerNb; ++i) {
			int playerNo = i + 1;
			GameColor color = (GameColor) playerNo;
			var scrapType = color;
			int scrapSpawnPointsNb = scrapSpawnPoints[scrapType].Count;
			for (int j = 0; j < scrapSpawnPointsNb; ++j) {
				SpawnAndRegisterScrap(scrapSpawnPoints[scrapType][j].position, color);
			}
		}
	}

	/// Return true if no scraps active in the scene (all stored in a container)
	public bool CheckIfNoScrapsLeft () {
		return !scrapPoolManager.AnyInUse();
	}

}
