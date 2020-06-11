using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using CommonsHelper;
using CommonsPattern;

public class CharacterMaster : MasterBehaviour
{
    /* Sibling components */
//    Animator animator;
    CharacterControl controller;
    CharacterMotor motor;
    
    /* Parameters */
    [SerializeField, Tooltip("Player ID (0-4)")]
    private int playerID = 0;
    public int PlayerId => playerID;

    [SerializeField, Tooltip("Character color")]
    private GameColor color = GameColor.Blue;
    public GameColor Color => color;
    
    void Awake () {
//        animator = this.GetComponentOrFail<Animator>();
        controller = this.GetComponentOrFail<CharacterControl>();
        motor = this.GetComponentOrFail<CharacterMotor>();
    }
    
    /* MasterBehaviour Start */
    
    /// Pause control and motion
    public override void Pause () {
        controller.enabled = false;
        motor.enabled = false;
//        animator.speed = 0f;
    }

    /// Resume control and motion
    public override void Resume () {
        controller.enabled = true;
        motor.enabled = true;
//        animator.speed = 1f;
    }
    
    /* MasterBehaviour End */
    
    /// Return true if active in the scene
    public bool IsPresent() {
        return gameObject.activeSelf;
    }
}