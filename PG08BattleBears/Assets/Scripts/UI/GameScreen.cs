using UnityEngine;
using System.Collections;

public class GameScreen : UIScreen {

	// Use this for initialization
	void Start () {
	
	}

    void OnEnable() {
        Time.timeScale = 1;
    }

    // Update is called once per frame
    void Update () {
        Cursor.lockState = CursorLockMode.Locked;
        if (Input.GetKeyDown(KeyCode.Escape)) {
            UIManager.instance.Show<ExitPopUp>();
        }
	}
}
