using System;
using UnityEngine;
using UnityEngine.InputSystem.Processors;

public class CharacterControl : MonoBehaviour
{
    /* Owned */
    private CharacterControls m_CharacterControls;
    
    /* State */
    private Vector2 m_MoveIntention;
    public Vector2 moveIntention { get { return m_MoveIntention; } }

    private void Awake()
    {
        m_CharacterControls = new CharacterControls();
    }

    private void OnDestroy()
    {
        m_CharacterControls.Dispose();
    }
    
    public void OnEnable()
    {
        m_CharacterControls.Enable();
    }

    public void OnDisable()
    {
        m_CharacterControls.Disable();
    }

    void Update()    
    {
        // Just use the input value, as we set up Binary Cardinal Processor for gamepad stick input in Controls asset,
        // so that each coordinate is 0/1 as in old school games with D-pad, even when using the left stick to move.
        m_MoveIntention = m_CharacterControls.Player.Move.ReadValue<Vector2>();
    }
}
