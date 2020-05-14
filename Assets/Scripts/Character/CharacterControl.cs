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
    
    private bool m_SwitchColorIntention = false;
    public bool SwitchColorIntention => m_SwitchColorIntention;

    private void Start()
    {
        Setup();
    }

    private void Setup()
    {
        m_MoveIntention = Vector2.zero;
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
}