using UnityEngine;
using System.Collections;

public class ScrapSpawnPoint : MonoBehaviour {

	[SerializeField]
	GameColor m_Color = GameColor.None;
	// color of the key to spawn at the position of this object
	public GameColor color { get { return m_Color; } }

	void Awake (){
		// Debug.Log("ScrapSpawnPoint AWAKE");
	}

	void Start () {
		// Debug.Log("ScrapSpawnPoint START");
		// comment out if the level manager registers the scraps by itself
		LevelManager.Instance.RegisterKeySpawnPoint(transform, m_Color);
	}

}
