using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

using CommonsHelper;

public class Player : MonoBehaviour
{
    [Tooltip("Characters Data to spawn corresponding character on player join")]
    public CharactersData charactersData;
    
    // Delegates and events for input actions
    
    public delegate void OnMoveInputDelegate(InputValue value);
    public event OnMoveInputDelegate onMoveInputEvent;
    
    public delegate void OnSwitchColorInputDelegate(InputValue value);
    public event OnSwitchColorInputDelegate onSwitchColorInputEvent;
    
    private void Start()
    {
        int playerIndex = PlayerInputManager.instance.playerCount - 1;
        Debug.LogFormat("Player {0} joined, spawning corresponding character", playerIndex + 1);
        
        // spawn character and associate this Player
        GameObject character = Instantiate(charactersData.characterPrefabs[playerIndex], Vector3.zero, Quaternion.identity);
        var characterControl = character.GetComponentOrFail<CharacterControl>();
        characterControl.SetPlayer(this);
    }
    
    /// PlayerInput action message callback for Move
    private void OnMove(InputValue value)
    {
        onMoveInputEvent?.Invoke(value);
    }
    
    /// PlayerInput action message callback for SwitchColor
    private void OnSwitchColor(InputValue value)
    {
        onSwitchColorInputEvent?.Invoke(value);
    }
}
