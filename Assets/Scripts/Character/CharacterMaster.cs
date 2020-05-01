using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMaster : MonoBehaviour
{
    /* Parameters */
    [SerializeField, Tooltip("Player ID (0-4)")]
    private int m_PlayerID = 0;
    public int playerId => m_PlayerID;

    [SerializeField, Tooltip("Character color")]
    private GameColor m_Color = GameColor.Blue;
    public GameColor color => m_Color;
}