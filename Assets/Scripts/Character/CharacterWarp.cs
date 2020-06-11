using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityConstants;

using CommonsDebug;
using CommonsHelper;

public class CharacterWarp : MonoBehaviour {

	/* internal script references */
	private CharacterMaster master;
	private CharacterControl control;

	void Awake () {
		master = this.GetComponentOrFail<CharacterMaster>();
		control = this.GetComponentOrFail<CharacterControl>();
	}

	void Start () {
		if (CharacterManager.Instance == null) throw new UnityException("No instance of CharacterManager, needed for " + this);
	}

	// Update is called once per frame
	void Update () {

	}

	/* Warp the avatar out of the scene */

	/* alternatives:
	a) Warp() is public, the warper calls Warp(), Warp() calls the Character Manager
	b) Warp() is private, the warper sets the avatar's intention to warp, FixedUpdate() converts
		intention to action Warp(), Warp() calls the Character Manager
	c) there is no Warp() here and the warper directly calls the Character Manager
	chosen: a) (allows to add some extra such as StopMotion or some animation)
	*/
	public void Warp () {
		DebugScreen.Print(0, "{0} warps.", this);

		control.StopControl();
		// motor.StopMotion();
		/* add some warping view here, should last around 1 second */
		// yield return new WaitForSeconds(1.0f);
		CharacterManager.Instance.DespawnCharacter(master.PlayerId);

		// if the last avatar has been removed, end the session
		if (CharacterManager.Instance.RemainingCharacterNb == 0) {
			SceneManager.LoadScene(Scenes.ScoreScreen);
			// SessionManager.Instance.Reset();
		}
	}

}
