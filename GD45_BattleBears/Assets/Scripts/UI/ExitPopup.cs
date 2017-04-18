using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitPopup : UIScreen {

    //OnEnable is called when the object gets enabled/activated.
    //Start is only called once when the object gets instantiated
    void OnEnable ()
    {
        Time.timeScale = 0;
    }

    void OnDisable ()
    {
        Time.timeScale = 1;
    }

	public void OnYesButton ()
    {
        UIManager.instance.Show<MenuScreen>();
        SceneManager.UnloadSceneAsync("GameScene");
    }

    public void OnNoButton ()
    {
        UIManager.instance.Show<GameScreen>();
    }
}
