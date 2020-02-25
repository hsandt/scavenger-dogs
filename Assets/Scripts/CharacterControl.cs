using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterControl : MonoBehaviour
{
    /* Owned */
    private CharacterControls m_CharacterControls;
    
    /* State */
    private Vector2 m_MoveIntentionVector;
    public Vector2 moveIntentionVector { get { return m_MoveIntentionVector; } }

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
        // update intention
        m_MoveIntentionVector = m_CharacterControls.Player.Move.ReadValue<Vector2>();
    }
}
