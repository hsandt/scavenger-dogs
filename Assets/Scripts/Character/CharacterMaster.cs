using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CharacterMaster : MonoBehaviour
{
    /* Parameters */
    [FormerlySerializedAs("m_PlayerID")] [SerializeField, Tooltip("Player ID (0-4)")]
    private int playerID = 0;
    public int PlayerId => playerID;

    [FormerlySerializedAs("m_Color")] [SerializeField, Tooltip("Character color")]
    private GameColor color = GameColor.Blue;
    public GameColor Color => color;
}