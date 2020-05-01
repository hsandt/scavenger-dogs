using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterControl : MonoBehaviour
{
    /* Sibling components */
    private CharacterMaster characterMaster;
    
    /* State */
    private Vector2 m_MoveIntention = Vector2.zero;
    public Vector2 MoveIntention => m_MoveIntention;

    private void Start()
    {
        Setup();
    }

    private void Setup()
    {
        m_MoveIntention = Vector2.zero;
    }

    // Input Action callback
    private void OnMove(InputValue value)
    {
        // Just use the input value, as we set up Binary Cardinal Processor for gamepad stick input in Controls asset,
        // so that each coordinate is 0/1 as in old school games with D-pad, even when using the left stick to move.
        m_MoveIntention = value.Get<Vector2>();
    }
}