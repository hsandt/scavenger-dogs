using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using CommonsHelper;

public class SwitchBlock : MonoBehaviour {

	/* Animation hashes */
	private static readonly int AnimHash_Active = Animator.StringToHash("active");
	
	/* Sibling components */
	private Animator animator;
	private Collider2D collider2d;

	/* Parameters */
	[SerializeField, Tooltip("Game Color")]
	private GameColor color = GameColor.None;
	public GameColor Color => color;

	/* State */
	private SwitchBlockState m_State;

	private void Awake () {
		animator = this.GetComponentOrFail<Animator>();
		collider2d = this.GetComponentOrFail<Collider2D>();
		
		m_State = SwitchBlockState.Ground;
		// must be done before SwitchBlockManager.Setup as it may call OnSwitchActiveColor back
		collider2d.enabled = false;
	}

	private void Start () {
		// register to manager
		// then let the SwitchBlockManager take care of initialization
		SwitchBlockManager.Instance.RegisterSwitchBlock(this);

		Setup();
	}

	private void Setup() {

	}

	private void OnEnable () {
		SwitchBlockManager.switchActiveColorEvent += OnSwitchActiveColor;
		animator.speed = 1f;
	}

	private void OnDisable () {
		SwitchBlockManager.switchActiveColorEvent -= OnSwitchActiveColor;
		animator.speed = 0f;
	}

	/// Method called on reception of the event 'active color has changed'
	/// Change the state accordingly
	private void OnSwitchActiveColor (GameColor newActiveColor) {
		// if the new active color and the previous active color are both this block's color,
		//  or both not this block's color no need to change the state
		bool wasActive = animator.GetBool(AnimHash_Active);
		bool shouldBeActive = color == newActiveColor;
		if (wasActive ^ shouldBeActive) {
			// the current state of the block is not was it should be, update it with collider and animation
			m_State = shouldBeActive ? SwitchBlockState.Ground : SwitchBlockState.Wall;
			collider2d.enabled = !shouldBeActive;
			animator.SetBool(AnimHash_Active, shouldBeActive);
		}
	}

}
