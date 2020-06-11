using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

using CommonsHelper;

public class CharacterControl : MonoBehaviour
{
    /* External references */
    
    /// Controlling player. Must be set from Player side after Character Prefab instantiation.
    private Player player;
    
    /* State */
    
    private Vector2 m_MoveIntention = Vector2.zero;
    public Vector2 MoveIntention => m_MoveIntention;
    
    private bool m_SwitchColorIntention;
    public bool SwitchColorIntention => m_SwitchColorIntention;

    /// Player can control character only when this is true
    private bool m_CanControl;
    
    /// Timer for control stop
    private float m_TimeRemainingWithoutControl;
    
    private void Start()
    {
        Setup();
    }

    private void Setup()
    {
        m_MoveIntention = Vector2.zero;
        m_SwitchColorIntention = false;
        m_CanControl = true;
        m_TimeRemainingWithoutControl = 0f;
    }

    private void OnEnable()
    {
        SubscribeInputEvent();
    }

    private void OnDisable()
    {
        UnsubscribeInputEvent();
    }
    
    public void SetPlayer(Player newPlayer)
    {
        // clean up event binding for any previous player
        UnsubscribeInputEvent();

        player = newPlayer;
        
        // bind control event for new player
        SubscribeInputEvent();
    }
    
    private void SubscribeInputEvent()
    {
        if (player != null)
        {
            player.onMoveInputEvent += PlayerOnMoveInputEvent;
            player.onSwitchColorInputEvent += PlayerOnSwitchColorInputEvent;
        }
    }
    
    private void UnsubscribeInputEvent()
    {
        if (player != null)
        {
            player.onMoveInputEvent -= PlayerOnMoveInputEvent;
            player.onSwitchColorInputEvent -= PlayerOnSwitchColorInputEvent;
        }
    }
    
    private void PlayerOnMoveInputEvent(InputValue value)
    {
        // Just use the input value, as we set up Binary Cardinal Processor for gamepad stick input in Controls asset,
        // so that each coordinate is 0/1 as in old school games with D-pad, even when using the left stick to move.
        m_MoveIntention = value.Get<Vector2>();
    }
    
    private void PlayerOnSwitchColorInputEvent(InputValue value)
    {
        Debug.Assert(value.isPressed, "SwitchColor input action sent with false value. " +
                            "Make sure that CharacterControls Input Actions" +
                            "do not set SwitchColor Interactions to Press and Release");
        // considering the assertion above, we can safely always set flag to true for a sticky intention
        // (until consumption)
        m_SwitchColorIntention = true;
    }

    public bool ConsumeSwitchColorIntention()
    {
        return ControlUtil.ConsumeBool(ref m_SwitchColorIntention);
    }
    
    /// Enable control (only call when this component is enabled to avoid double SubscribeInputEvent)
    public void StartControl() {
        if (m_CanControl)
        {
            Debug.LogAssertionFormat(this, "Player can already control {0}, cannot Start Control again.", this);
            return;
        }
        
        m_CanControl = true; // set control variable to enable input processing

        SubscribeInputEvent();
    }

    /// Disable control until StartControl is called
    public void StopControl() {
        if (!m_CanControl)
        {
            Debug.LogAssertionFormat(this, "Player cannot control {0} yet, cannot Stop Control again.", this);
            return;
        }
        
        // call the duration version of this method, with 0 to mean "permanent stop"
        StopControlForDuration(0);
    }

    /// Temporarily disable control for the given duration
    /// a null or negative duration stands for a permanent stop, as there will be no further countdown
    /// (only call when this component is enabled to avoid double UnsubscribeInputEvent)
    public void StopControlForDuration(float duration) {
        // reset all intentions in case the motor is still active
        m_MoveIntention = Vector2.zero;
        m_SwitchColorIntention = false;
        // enabled = false; // disable the script to stop updating the velocity
        
        // unsubscribe from all input events to prevent further control
        UnsubscribeInputEvent();

        // Debug.Log("disabling control in StopControl");
        m_CanControl = false; // set control variable to disable input processing
        m_TimeRemainingWithoutControl = duration;
    }
}