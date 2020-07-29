using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScreenLobby : MonoBehaviour
{
    [Tooltip("Array of player JOINED texts (should contain 4 items)")]
    public TextMeshProUGUI[] playerJoinedTexts;

    public void Setup()
    {
        foreach (TextMeshProUGUI playerJoinedText in playerJoinedTexts)
        {
            playerJoinedText.enabled = false;
        }
    }
    
    public void ShowPlayerJoinText(int playerID)
    {
        playerJoinedTexts[playerID].enabled = true;
    }
    
    public void HidePlayerJoinText(int playerID)
    {
        playerJoinedTexts[playerID].enabled = false;
    }
}
