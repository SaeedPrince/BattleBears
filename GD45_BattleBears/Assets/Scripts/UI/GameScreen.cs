using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScreen : UIScreen
{

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        //This will lock and hide the cursor
        Cursor.lockState = CursorLockMode.Locked;

        if (Input.GetKeyDown(KeyCode.Q))
        {
            UIManager.instance.Show<ExitPopup>();
            Cursor.lockState = CursorLockMode.None;
        }
	}
}
