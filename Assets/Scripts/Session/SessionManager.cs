using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using CommonsDebug;
using CommonsHelper;
using CommonsPattern;

public class SessionManager : SingletonManager<SessionManager> {

	/* Parameters */
	
	[Header("Text")]
	
	[SerializeField, Tooltip("Session start message")]
	private string sessionStartMsg = "START";

	[Header("Timing")]
	
	[SerializeField, Tooltip("Delay between level generation and avatar spawning (s)")]
	float spawnDelay = 1.0f;
	
	[SerializeField, Tooltip("Delay until start message display (s)")]
	public float startMsgDelay = 3.0f; // d
	
	[SerializeField, Tooltip("Start message duration (s)")]
	public float startMsgDuration = 3.0f; // start message duration

	
	/* State */

	/// Current state
	private SessionManagerState m_State;
	
	/// Is the session starting? Useful to lock other behaviors.
	private bool m_IsStartingSession;
	public bool IsStartingSession => m_IsStartingSession;
	
	/// Number of players who are joining the session
	private int m_NbPlayersJoin = 0;
	
	/// Number of players ready to start the session
	private int m_NbPlayersReady = 0;

	
	protected override void Init()
	{
		base.Init();

		m_State = SessionManagerState.GenerateLevel;
		m_IsStartingSession = false;
	}

	// Use RCS methods; if you prefer, use a Session object with RCS, controlled
	// by a SessionManager

	public void Reset ()
	{
		// OPTIMIZE: when reloading the same level, just reset everything (avatars should
		// have a reset location and keys method)
		Clear();
		Setup();
	}

	/// Clear, in order: avatars, blocks, level
	public void Clear ()
	{
		CharacterManager.Instance.DespawnAllCharacters(); // only useful for debug reset
		SwitchBlockManager.Instance.Clear(); // nothing for now
		LevelManager.Instance.ClearLevel();
	}

	public void Setup ()
	{
	}

	private void Update()
	{
		switch (m_State)
		{
			case SessionManagerState.GenerateLevel:
				m_State = SessionManagerState.Join;
				OnEnterJoinPhase();
				break;
			case SessionManagerState.Join:
//				UpdateJoinPhase();
				break;
			case SessionManagerState.Party:
//				UpdatePartyPhase();
				break;
		}
	}

	private void OnEnterJoinPhase()
	{
		Debug.Log("SessionManager Join Phase ENTER");

		// immediately retrieve event parent by tag in loaded level, and register scrap spawn events
		// now objects are spawned after some delay, so self-registration should be enough
		// LevelManager.Instance.RegisterAllScrapSpawnPoints(ParentLocator.eventParent);

		// spawn all scraps
		LevelManager.Instance.SpawnAllScraps();
		// on the built level, initialize correct block states
		SwitchBlockManager.Instance.Setup();

		// activate player join UI (input + view)
		// IMPROVE: only show message for plugged controllers, if we can detect them
		for (int i = 1; i <= 4; ++i) {
			Debug.Log(string.Format("PlayerJoinUI {0}", i));
		}

		DebugScreen.PrintVar<int>("join", 0);
		DebugScreen.PrintVar<int>("ready", 0);
	}

	private void OnExitJoinPhase()
	{
	}

	private void OnEnterPartyPhase()
	{
		DebugScreen.Print(1, "Party starting");

		// VISUAL: display the START message on GUI layer (prefab also on GUI layer just in case)
		UIManager.Instance.ShowMessage("START");

		// AUDIO: play the in-game BGM
		MusicManager.Instance.Play();

		// After some time, enable the avatars to move
		CharacterManager.Instance.StartControlAllCharacters();
	}

	IEnumerator DelayHideStartMessage() {
		// Debug.LogWarning("Wait 1 second...");
		yield return new WaitForSeconds(1);
		// Debug.LogWarning("Hide!");
		UIManager.Instance.HideMessage("START");
	}

	/// Let player join
	public void NotifyPlayerJoin () {
		++m_NbPlayersJoin;
		DebugScreen.PrintVar<int>("join", m_NbPlayersJoin);

		// if needed, hide Start Party message
		UIManager.Instance.HideSelectableMessage(sessionStartMsg);

	}

	public void NotifyPlayerStopJoin () {
		// TODO: despawn (in practice, do not destroy but deactivate)
		--m_NbPlayersJoin;
		DebugScreen.PrintVar<int>("join", m_NbPlayersJoin);
	}

	public void NotifyPlayerReady () {
		++m_NbPlayersReady;
		DebugScreen.PrintVar<int>("ready", m_NbPlayersReady);

		if (AreAllJoiningPlayersReady()) {
			UIManager.Instance.ShowAndSelectMessage(sessionStartMsg);
		}
	}

	public void NotifyPlayerStopReady () {
		--m_NbPlayersReady;
		DebugScreen.PrintVar<int>("ready", m_NbPlayersReady);

		UIManager.Instance.HideSelectableMessage(sessionStartMsg);
	}

	/// Return true if there is at least one joining player, and all joining players are ready
	public bool AreAllJoiningPlayersReady () {
		return m_NbPlayersJoin > 0 && m_NbPlayersJoin == m_NbPlayersReady;
	}

}
