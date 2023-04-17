using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelFinishManager : MonoBehaviour
{
    private DifficultyManager difficultyManager;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private GameObject endScreenPanel;
    [SerializeField] private GameObject newDifficultyText;

    public static LevelFinishManager instance;

    private void Awake() {
        instance = this;
    }

    private void Start() {
        difficultyManager = FindObjectOfType<DifficultyManager>();
    }

    public void endRace(string endText, bool win) {
        text.text = endText;
        endScreenPanel.SetActive(true);
        if (win) {
            // unlock next level if necessary
            if (difficultyManager.difficultyOptions[difficultyManager.difficultyOptions.Length - 1] == SceneManager.GetActiveScene().buildIndex) {
                difficultyManager.unlockDifficulty();
                newDifficultyText.SetActive(true);
            }
        }
    }

    public void returnToMainMenu() {
        SceneManager.LoadScene(0);
    }
}
