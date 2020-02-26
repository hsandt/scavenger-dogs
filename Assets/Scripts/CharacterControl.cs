using System;
using UnityEngine;
using UnityEngine.InputSystem.Processors;

public class CharacterControl : MonoBehaviour
{
    /* Owned */
    private CharacterControls m_CharacterControls;
    
    /* Parameters */
    [SerializeField, Tooltip("Dead zone for independent X/Y input")]
    private float cardinalDeadZone = 0.125f;
    
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
        // Process input so that each coordinate is 0/1 as in old school games with D-pad,
        // even when using gamepad left stick. For digital input, there is nothing to do.
        // For analog input (stick), just ceil any sufficient input coordinate to 1, independently from the other.
        Vector2 moveInput = m_CharacterControls.Player.Move.ReadValue<Vector2>();
        if (Mathf.Abs(moveInput.x) > cardinalDeadZone)
        {
            m_MoveIntention.x = Mathf.Sign(moveInput.x);
        }
        else
        {
            m_MoveIntention.x = 0f;
        }
        if (Mathf.Abs(moveInput.y) > cardinalDeadZone)
        {
            m_MoveIntention.y = Mathf.Sign(moveInput.y);
        }
        else
        {
            m_MoveIntention.y = 0f;
        }
    }
}
