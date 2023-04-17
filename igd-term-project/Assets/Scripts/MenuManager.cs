using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    // build number of scene to start when start button is pressed
    [SerializeField] private int gameStartScene;
    public static MenuManager instance;

    private void Awake() {
        instance = this;
    }

    public void StartGame() {
        SceneManager.LoadScene(gameStartScene);
    }

    public void QuitGame() {
        Application.Quit();
    }

    public void setGameStartScene(int scene) {
        gameStartScene = scene;
    }    

    public int getDifficulty()
    {
        return gameStartScene;
    }
}
