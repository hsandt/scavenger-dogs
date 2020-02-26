using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using CommonsHelper;

[RequireComponent(typeof(Rigidbody2D))]
public class CharacterMotor : MonoBehaviour
{
	/* Sibling components */
	private Rigidbody2D rigidbody2d;
	private CharacterControl characterControl;
	
	/* Parameters */
	[SerializeField, Tooltip("Character speed")]
	private float speed = 1f;

	void Awake ()
	{
		rigidbody2d = this.GetComponentOrFail<Rigidbody2D>();
		characterControl = this.GetComponentOrFail<CharacterControl>();
	}

	void FixedUpdate ()
	{
		// we assume move intention coordinates are 0/1 are in old school games using D-pad
		rigidbody2d.velocity = speed * characterControl.moveIntention;
	}
}
