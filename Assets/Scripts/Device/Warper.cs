using UnityEngine;
using System.Collections;

using CommonsHelper;
using UnityConstants;

public class Warper : MonoBehaviour {

	Animator animator;

	void Awake () {
		animator = this.GetComponentOrFail<Animator>();
	}

	void Start () {
		WarperManager.Instance.RegisterWarper(this);
		Disappear();
	}

	// Update is called once per frame
	void Update () {

	}

	void OnTriggerEnter2D (Collider2D other) {
		// warp collider if it is a player character
		if (!other.CompareTag(Tags.Player)) return;

		// warp character
		var warp = other.GetComponentOrFail<CharacterWarp>();
		warp.Warp();
	}

	private void Disappear()
	{
//		animator.SetBool("Visible", false);
		gameObject.SetActive(false);
	}

	public void Appear () {
//		animator.SetBool("Visible", true);
		gameObject.SetActive(true);
	}

	public bool HasAppeared()
	{
		return gameObject.activeSelf;
	}

}
