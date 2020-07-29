using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
using System.Linq;

using CommonsPattern;

public class SystemManager : SingletonManager<SystemManager> {

	protected SystemManager () {} // guarantee this will be always a singleton only - can't use the constructor!

	// object references
	
	void Awake () {
		SetInstanceOrSelfDestruct(this);
	}

	void Start ()
	{
	}

	// Input action callback
	private void OnPause()
	{
		// open/close the pause menu
		if (!UIManager.Instance.IsPauseMenuActive()) {
			// prevent pausing game during StartSession coroutine
			// TODO: replace coroutine with regular Update() + timer and
			// allow opening the menu anytime; just pause the session as other objects
			if (!SessionManager.Instance.IsStartingSession) {
				PauseGame();
			}
			else {
				Debug.Log("Tried to open menu but session is starting!");
			}
		}
		else {
			ResumeGame();
		}
	}
	
	// Input action callback
	private void OnDecrementPlayerNb()
	{
//		SessionManager.Instance.m_NbPlayersJoin --;
//		if (SessionManager.Instance.m_NbPlayersJoin < 2)
//			SessionManager.Instance.m_NbPlayersJoin = 4;
	}
	
	// Input action callback
	private void OnIncrementPlayerNb()
	{
//		SessionManager.Instance.playerNb ++;
//		if (SessionManager.Instance.playerNb > 4)
//			SessionManager.Instance.playerNb = 2;
	}
	
	// Input action callback
	private void OnReset()
	{
		// TODO: use enum game phases instead
		if (!SessionManager.Instance.IsStartingSession) {
			// normally changes are in FixedUpdate, but for a debug or a complete change it should be fine
			SessionManager.Instance.Reset();
		}
		else {
			Debug.Log("SystemManager: received Reset input but SessionManager is already reseting.");
		}
	}
	
	public void Quit () {
		Application.Quit();
	}

	public void PauseGame () {
		PauseGamePhysics();
		UIManager.Instance.OpenPauseMenu();
	}

	public void ResumeGame () {
		ResumeGamePhysics();
		UIManager.Instance.ClosePauseMenu();
	}

	public void RestartSession() {
		// FIXME: only resume objects that will not be destroyed in the reset
		// we could also divide the methods in more parts, call the clearing parts of Reset(),
		// then resume remaining objects (typ. Managers), finally call generating parts of Reset()
		ResumeGamePhysics();  // required if we want to use SwitchBlockManager and SwitchBlocks,
		// since switch blocks cannot reset otherwise (for now we rebuild everything anyway)
		// TODO: on Reset, reset SBM timer
		UIManager.Instance.ClosePauseMenu();
		SessionManager.Instance.Reset();

		// FIXME: blocks still wrong when restarting game from menu
		// FIXME: restarting before avatars spawn cause incorrect initialization (control both menu cursor and character)
		// either prevent opening the menu before spawning or ensure spawned avatars are frozen
		// or prevent delayed avatar spawning (that is why coroutines are more difficult than time counters)
		// actually need to stop counter so almost impossible; if you use a coroutine, prevent any user action!
	}

	/// Pause all in-game actions
	void PauseGamePhysics () {

		// TODO: Register all IPausable objects (that are active in the scene)
		// and iterate on them following Update Pattern

		// freeze all present avatars via the Master Avatar component
		CharacterManager.Instance.PauseAllCharacters();

		// pause switch block manager, it will also pause all switch blocks
		SwitchBlockManager.Instance.Pause();
		// SwitchBlockManager.Instance.enabled = false;

		// FIXME? I do not pause GFX such as key pick since they have no effect on the mechanics
		// and feedback importance is minor

		// optional: pause BGM
		MusicManager.Instance.enabled = false;

	}

	/// Resume all in-game actions
	void ResumeGamePhysics () {

		// unfreeze all avatars
		CharacterManager.Instance.ResumeAllCharacters();

		// resume switch block manager (also resume all switch blocks)
		SwitchBlockManager.Instance.Resume();
		// SwitchBlockManager.Instance.enabled = true;

		MusicManager.Instance.enabled = true;
	}
}
