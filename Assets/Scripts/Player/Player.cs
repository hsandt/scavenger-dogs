using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

using CommonsHelper;

public class Player : MonoBehaviour
{
    [Tooltip("Characters Data")]
    public CharactersData charactersData;
    
    public delegate void OnMoveInputDelegate(InputValue value);
    public event OnMoveInputDelegate onMoveInputEvent;

    void Start()
    {
        int playerIndex = PlayerInputManager.instance.playerCount - 1;
        Debug.LogFormat("Player {0} joined, spawning corresponding character", playerIndex + 1);
        
        // spawn character and associate this Player
        GameObject character = Instantiate(charactersData.characterPrefabs[playerIndex], Vector3.zero, Quaternion.identity);
        var characterControl = character.GetComponentOrFail<CharacterControl>();
        characterControl.SetPlayer(this);
    }
    
    /// Input Action callback (via Message)
    private void OnMove(InputValue value)
    {
        onMoveInputEvent?.Invoke(value);
    }
}
