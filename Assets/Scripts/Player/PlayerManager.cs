using System.Collections;
using System.Collections.Generic;
using CommonsHelper;
using UnityEngine;

using CommonsPattern;
using UnityEngine.InputSystem;

public class PlayerManager : SingletonManager<PlayerManager>
{
    /* Sibling components */
    
    private PlayerInputManager playerInputManager;

    protected override void Init()
    {
        playerInputManager = this.GetComponentOrFail<PlayerInputManager>();
    }

    public void EnableJoining()
    {
        playerInputManager.EnableJoining();
    }

    public void DisableJoining()
    {
        playerInputManager.DisableJoining();

        // for now, just destroy all players who joined so far...
        // we don't need to remember them if we leave and enter lobby again
        // although we could, using .enabled = false then true and updating
        // the join widgets based on players who previously joined
        foreach (var player in PlayerInput.all)
        {
            Destroy(player.gameObject);
        }
    }

    /// PlayerInputManager message callback
    private void OnPlayerJoined()
    {
        Debug.Log("player joined msg received");
    }
}
