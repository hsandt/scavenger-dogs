using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

using CommonsHelper;

public class PlayerEntity : MonoBehaviour
{
    private void Start()
    {
        int playerIndex = PlayerInputManager.instance.playerCount - 1;
        Debug.LogFormat("Player {0} joined, notifying Lobby", playerIndex + 1);
        
        MainMenu.Instance.NotifyLobbyOfPlayerJoining(playerIndex);
    }
}
