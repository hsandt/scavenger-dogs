using UnityEngine;
using System.Collections;

using CommonsHelper;

public class CharacterSwitchColor : MonoBehaviour {

	/* Sibling components */
	
	private CharacterMaster characterMaster;
	private CharacterControl characterControl;
	
	void Awake () {
		characterMaster = this.GetComponentOrFail<CharacterMaster>();
		characterControl = this.GetComponentOrFail<CharacterControl>();
	}

	void FixedUpdate () {
		// TODO: add a priority system in case several players activate their color at the same time (if 3+ players)
		if (characterControl.ConsumeSwitchColorIntention()) {
			SwitchBlockManager.Instance.SwitchActiveColor(characterMaster.Color);
		}
	}

}
