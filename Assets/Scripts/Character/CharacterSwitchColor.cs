using UnityEngine;
using System.Collections;

using CommonsHelper;

public class CharacterSwitchColor : MonoBehaviour {

	/* Sibling components */
	
	private CharacterMaster master;
	private CharacterControl control;
	
	void Awake () {
		master = this.GetComponentOrFail<CharacterMaster>();
		control = this.GetComponentOrFail<CharacterControl>();
	}

	void FixedUpdate () {
		// TODO: add a priority system in case several players activate their color at the same time (if 3+ players)
		if (control.ConsumeSwitchColorIntention()) {
			SwitchBlockManager.Instance.SwitchActiveColor(master.Color);
		}
	}

}
