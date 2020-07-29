using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using CommonsPattern;
using UnityConstants;

public class MainMenu : SingletonManager<MainMenu>
{
    [Tooltip("Screen Title Menu")] public GameObject screenTitleMenu;
    [Tooltip("Mode Party")] public GameObject modeParty;
    [Tooltip("Screen Lobby")] public ScreenLobby screenLobby;

    private void Start()
    {
        screenTitleMenu.SetActive(true);
        modeParty.SetActive(false);
        screenLobby.gameObject.SetActive(false);
    }
    
    public void EnterPartyMode()
    {
        screenTitleMenu.SetActive(false);
        modeParty.SetActive(true);
        screenLobby.gameObject.SetActive(true);
        screenLobby.Setup();
        PlayerManager.Instance.EnableJoining();
    }
    
    public void ExitPartyMode()
    {
        screenTitleMenu.SetActive(true);
        modeParty.SetActive(false);
        screenLobby.gameObject.SetActive(false);
        PlayerManager.Instance.DisableJoining();
    }

    public void NotifyLobbyOfPlayerJoining(int playerIndex)
    {
        screenLobby.ShowPlayerJoinText(playerIndex);
    }
    
    /// Button event callback
    public void StartStage1()
    {
        SceneManager.LoadScene(Scenes.Stage1);
    }
}
