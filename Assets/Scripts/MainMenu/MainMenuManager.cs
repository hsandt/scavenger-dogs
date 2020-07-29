using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using CommonsPattern;
using UnityConstants;

public class MainMenuManager : SingletonManager<MainMenuManager>
{
    /// Button event callback
    public void StartStage1()
    {
        SceneManager.LoadScene(Scenes.Stage1);
    }
}
