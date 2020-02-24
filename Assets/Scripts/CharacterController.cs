using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterController : MonoBehaviour
{
    /* intentions (better in Controller or Mind) */
    private Vector2 m_MoveIntentionVector;
    public Vector2 moveIntentionVector { get { return m_MoveIntentionVector; } }

    private InputActionsCharacter m_InputActionsCharacter;

    private void Awake()
    {
        m_InputActionsCharacter = new InputActionsCharacter();
    }

    private void OnDestroy()
    {
        m_InputActionsCharacter.Dispose();
    }
    
    public void OnEnable()
    {
        m_InputActionsCharacter.Enable();
    }

    public void OnDisable()
    {
        m_InputActionsCharacter.Disable();
    }

    void Update()
    {
        // update intention
        m_MoveIntentionVector = m_InputActionsCharacter.Player.Move.ReadValue<Vector2>();
    }
}
