using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using CommonsHelper;
using UnityConstants;

public class CharacterPick : MonoBehaviour {

	/* External references */

	[Tooltip("prefab reference for the pick_up GFX")]
	public GameObject pickUpGFXPrefab;
	
	/* Sibling components */

	private CharacterMaster master;

	/* dynamic attributes */
	private int nbKeys;

	public int NbKeys => nbKeys;

	// adapted getter
	public bool HasAllKeys () {
		return nbKeys >= LevelManager.Instance.requiredNbScrapsPerColorDict[master.Color];
	}

	// Use this for initialization
	void Awake () {
		master = this.GetComponentOrFail<CharacterMaster>();

		Debug.Assert(pickUpGFXPrefab, "pickUpGFXPrefab not set");
	}

	void Start () {
		Reset();
	}
	
	void Reset () {
		nbKeys = 0;
	}

	// Pick up the key given as argument
	// 	in this method we do all the work related to picking up a key
	//	we could also have split destroying the key and increasing the key counter between the key itself
	//	and the avatar, but in case we need to do more (pick up animation, etc.), it's better to centralize
	//	the work in the avatar
	public void Pick (Scrap scrap) {

		// check if the key color is this avatar's color (the only check)	
		if (scrap.Color == master.Color) {

			Debug.Log(this + " picks up " + scrap);

			// increase the key count
			IncrementNbKeys();

			/* alternative:
			change the key's sprite animation to the GFX and deactivate its trigger
			behaviour; then only, destroy the key object */
			
			// destroy the key
			Destroy(scrap.gameObject);

			/* VISUAL */
			// add the pick_up GFX (you can still use key until its destruction)
			//	where the key was
			// TODO: pool will handle this
			var pickUpGFX = pickUpGFXPrefab.InstantiateUnderAtOn(
				null,
				scrap.transform.localPosition,
				Layers.UI
			);
			// the color of the GFX must match the one of the avatar/key
			/* alternatives:
			a) create one prefab per color
			b) prepare one animation state per color and play the right one at GFX creation
			c) prepare a gray-scaled animation and change the sprite renderer color ""
			chosen: c)
			Optimization note: c) could be optimized by storing inactive anim game objects
			on the side of the game, and clone them when needed (similarly to prefabs) */
			// TODO: same with blue, yellow... use true gray rather than white-blue
			var spriteRenderer = pickUpGFX.GetComponentOrFail<SpriteRenderer>();
			switch (master.Color)
			{
				case GameColor.Blue:
				spriteRenderer.color = Color.white;
				break;
				case GameColor.Red:
				spriteRenderer.color = Color.red;
				break;
				case GameColor.Yellow:
				spriteRenderer.color = Color.yellow;
				break;
				case GameColor.Green:
				spriteRenderer.color = Color.green;
				break;
			}

		}

	}

	// increment the number of keys in hand
	void IncrementNbKeys () {
		nbKeys ++;
	}

}
