using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MenuScreen : UIScreen {

    public void OnStartButton() {
        SceneManager.LoadScene("GameScene", LoadSceneMode.Additive);
        //UIManager.instance.Show(typeof(GameScreen));
        UIManager.instance.Show<GameScreen>();
    }

}
