using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using CommonsDebug;
using CommonsHelper;
using CommonsPattern;
using UnityConstants;

public class Scrap : MonoBehaviour, IPooledObject
{
	/* Parameters */
	
	[SerializeField, Tooltip("Color of the scrap. Targeted by character of same color.")]
	GameColor m_Color = GameColor.None;
	public GameColor Color => m_Color;
	
	/* IPooledObject Start */
	
	public void InitPooled() {
	}

	public bool IsInUse() {
		return gameObject.activeSelf;
	}

	public void Release() {
		gameObject.SetActive(false);
	}

	public void Spawn(Vector2 position) {
		if (IsInUse()) throw ExceptionsUtil.CreateExceptionFormat("Cannot spawn {0}, already in use.", this);

		gameObject.SetActive(true);
		transform.position = position;
	}

	/* IPooledObject End */

	void OnTriggerEnter2D (Collider2D other)
	{
		var characterPick = other.GetComponent<CharacterPick>();

		if (characterPick != null)
		{
			characterPick.Pick(this);
		}
		else
		{
			Debug.LogWarningFormat(this,
				"Other collider {0} entered Scrap trigger {1}, but has no CharacterPick component.",
				other,
				this);
		}
	}
}
