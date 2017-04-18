using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScreen : UIScreen
{

	public void OnStartGameButton ()
    {
        UIManager.instance.Show<GameScreen>();
        SceneManager.LoadScene("GameScene", LoadSceneMode.Additive);
    }
}
