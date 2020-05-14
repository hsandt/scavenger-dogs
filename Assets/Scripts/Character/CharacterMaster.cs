using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMaster : MonoBehaviour
{
    /* Parameters */
    [SerializeField, Tooltip("Player ID (0-4)")]
    private int playerID = 0;
    public int PlayerId => playerID;

    [SerializeField, Tooltip("Character color")]
    private GameColor color = GameColor.Blue;
    public GameColor Color => color;
}