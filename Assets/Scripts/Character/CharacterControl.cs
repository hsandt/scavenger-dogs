using System;
using UnityEngine;
using UnityEngine.InputSystem;

using CommonsHelper;

public class CharacterControl : MonoBehaviour
{
    /* Sibling components */
    private CharacterMaster characterMaster;
    
    /* Owned */
    private CharacterControls m_CharacterControls;
    
    /* State */
    private Vector2 m_MoveIntention = Vector2.zero;
    public Vector2 MoveIntention => m_MoveIntention;

    private void OnDestroy()
    {
        m_CharacterControls.Dispose();
    }

    private void Awake()
    {
        characterMaster = this.GetComponentOrFail<CharacterMaster>();

        m_CharacterControls = new CharacterControls();

        if (characterMaster.playerId < Gamepad.all.Count)
        {
            Gamepad gamepad = Gamepad.all[characterMaster.playerId];
            m_CharacterControls.devices = new[] {gamepad};
        }
        else
        {
            // Fix this later by handling InputSystem.onDeviceChange, PlayerInput.DeviceLostEvent, PlayerInput.DeviceRegainedEvent
            m_CharacterControls.devices = new InputDevice[] {};
            Debug.LogWarningFormat("Gamepad {0} not plugged during CharacterControl.Awake. Character {1} won't be controllable.",
                characterMaster.playerId, 
                characterMaster.color);
        }
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
