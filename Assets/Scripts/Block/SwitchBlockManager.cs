using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using CommonsHelper;
using CommonsPattern;
using UnityEngine.Serialization;

public class SwitchBlockManager : SingletonManager<SwitchBlockManager> {

	/* Events */
	public delegate void SwitchActiveColorHandler(GameColor newActiveColor);
	public static event SwitchActiveColorHandler switchActiveColorEvent;
	
	/* Sibling components */
	private AudioSource audioSource;
	
	/* Parameters */

	[SerializeField, Tooltip("The switch blocks of this color will start as Ground, the others as Wall")]
	private GameColor initActiveColor = GameColor.None;

	[SerializeField, Tooltip("Minimal time interval required between two successive color change, in sec")]
	private float colorChangeTimeBreak = 1f;

#if UNITY_EDITOR
	[SerializeField, Tooltip("Should the object set itself up by itself? (for test with independent objects placed directly in the scene)")]
	private bool debug_SetupOnStart = false;
#endif

	/* State */

	/// Dictionary of registered switch blocks, per color
	private readonly Dictionary<GameColor, List<SwitchBlock>> m_SwitchBlocksDict = new Dictionary<GameColor, List<SwitchBlock>>();

	/// The current active color (designates grounded switch blocks)
	private GameColor m_ActiveColor;
	public GameColor ActiveColor => m_ActiveColor;

	/// Decreasing timer variable to prevent successive active color changes in sec (when 0, color can change)
	private float m_TimeRemainingWithoutColorSwitch;
	
	/// Set on Pause to allow sound resume
	private bool m_WasAudioSourcePlaying;

	protected override void Init () {
		audioSource = this.GetComponentOrFail<AudioSource>();

		foreach (GameColor color in GameData.colors) {
			m_SwitchBlocksDict[color] = new List<SwitchBlock>();
		}
	}

	private void Start () {
		// We will progressively stop calling Setup from Start by default, for more control.
		if (debug_SetupOnStart)
			Setup();
	}

	private void Reset () {
		Clear();
		Setup();
	}

	private void Clear () {

	}

	private void Setup () {
		// immediately switch to the initial color
		// this means there will be animations for blocks moving from the default state
		// and players will not be able to switch blocks before some time after this has been called
		// in practice, the Start message is longer than the delay between two switches so it does not matter
		Debug.Log("(SwitchBlockManager) Activating initial color");
		m_ActiveColor = GameColor.None;
		m_TimeRemainingWithoutColorSwitch = 0f;
		m_WasAudioSourcePlaying = false;
		
		SwitchActiveColor(initActiveColor);
	}

	public void RegisterSwitchBlock(SwitchBlock switchBlock) {
		if (switchBlock.Color == GameColor.None) {
			Debug.LogFormat(switchBlock, "Switch Block {0} has color None, cannot be registered by SwitchBlockManager.",
				switchBlock);
			return;
		}
		
		m_SwitchBlocksDict[switchBlock.Color].Add(switchBlock);
	}

	public void SwitchActiveColor (GameColor newActiveColor) {

		Debug.LogFormat("Switch color action triggered for: {0}", newActiveColor);

		// don't mind switching the active color if it is already the one you want
		// for the initialization we start from the None color so it will never be true
		if (m_ActiveColor == newActiveColor) {
			Debug.Log("... but the wanted color is already active!");
			return;
		}

		// don't switch to the new active color if the last color change is too recent
		if (m_TimeRemainingWithoutColorSwitch > 0) {
			Debug.Log("... but color switch is still in cooldown!");
			return;
		}

		// else, conditions are fulfilled, activate the new color

		// set the timer to prevent immediate re-change
		m_TimeRemainingWithoutColorSwitch = colorChangeTimeBreak;
		// update the active color
		m_ActiveColor = newActiveColor;

		// if there are listeners (and there should be, the switch blocks), send the switch active color event
		switchActiveColorEvent?.Invoke(newActiveColor);

		// play SFX
		audioSource.Play();
		
	}

	void FixedUpdate () {

		if (m_TimeRemainingWithoutColorSwitch > 0) {
			m_TimeRemainingWithoutColorSwitch -= Time.deltaTime;
			if (m_TimeRemainingWithoutColorSwitch < 0) {
				m_TimeRemainingWithoutColorSwitch = 0;
			}
		}

	}
	
	public void Pause () {
		if (audioSource.isPlaying) {
			audioSource.Pause();
			m_WasAudioSourcePlaying = true;
		}

		foreach (GameColor color in GameData.colors) {
			foreach (SwitchBlock switchBlock in m_SwitchBlocksDict[color]) {
				switchBlock.enabled = false;
			}
		}
	}

	public void Resume () {
		if (m_WasAudioSourcePlaying) {
			audioSource.Play();
			m_WasAudioSourcePlaying = false;
		}

		foreach (GameColor color in GameData.colors) {
			foreach (SwitchBlock switchBlock in m_SwitchBlocksDict[color]) {
				switchBlock.enabled = true;
			}
		}
	}

}
