using System;
using CommonsHelper;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterControl : MonoBehaviour
{
    /* External references */
    /// Controlling player. Must be set from Player side after Character Prefab instantiation.
    private Player player;
    
    /* Sibling components */
    private CharacterMaster characterMaster;
    
    /* State */
    private Vector2 m_MoveIntention = Vector2.zero;
    public Vector2 MoveIntention => m_MoveIntention;

    private void Awake()
    {
        characterMaster = this.GetComponentOrFail<CharacterMaster>();
    }
    
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
        if (player != null)
        {
            player.onMoveInputEvent += PlayerOnOnMoveInputEvent;
        }
    }

    private void OnDisable()
    {
        if (player != null)
        {
            player.onMoveInputEvent -= PlayerOnOnMoveInputEvent;
        }
    }
    
    public void SetPlayer(Player newPlayer)
    {
        // clean up event binding for any previous player
        if (player != null)
        {
            player.onMoveInputEvent -= PlayerOnOnMoveInputEvent;
        }

        player = newPlayer;
        
        // bind control event for new player
        if (newPlayer != null)
        {
            newPlayer.onMoveInputEvent += PlayerOnOnMoveInputEvent;
        }
    }

    private void PlayerOnOnMoveInputEvent(InputValue value)
    {
        // Just use the input value, as we set up Binary Cardinal Processor for gamepad stick input in Controls asset,
        // so that each coordinate is 0/1 as in old school games with D-pad, even when using the left stick to move.
        m_MoveIntention = value.Get<Vector2>();
    }
}