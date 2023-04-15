using UnityEngine;
using System;

public class DifficultyManager : MonoBehaviour
{
    public int[] difficultyOptions; // 0=easy, 1=medium, 2=hard, 3=einstein

    private void Start() {
        if (PlayerPrefs.HasKey("Difficulties")) {
            // The key exists in PlayerPrefs
            updateDifficultyOptions();
        } else {
            // The key does not exist in PlayerPrefs, set default
            difficultyOptions[0] = 0;
        }
    }

    private void updateDifficultyOptions() {
        string difficutiesString = PlayerPrefs.GetString("Difficulties");
        string[] difficultiesStringArray = difficutiesString.Split(',');
        difficultyOptions = new int[difficultiesStringArray.Length];

        for (int i = 0; i < difficultiesStringArray.Length; i++) {
            difficultyOptions[i] = int.Parse(difficultiesStringArray[i]);
        }

    }

    // Sets difficulty as completed so it will unlock button
    public void unlockDifficulty() {
        if (difficultyOptions.Length >= 4) return;
        // add new difficulty to options
        int newDifficulty = difficultyOptions[difficultyOptions.Length - 1] + 1;
        // stringify the array
        string newDifficultiesString = string.Join(",", difficultyOptions) + "," + newDifficulty.ToString();
        // set playerprefs with updated options
        PlayerPrefs.SetString("Difficulties", newDifficultiesString);
        // update difficultyOptions array
        updateDifficultyOptions();
    }

    public void resetDifficulties() {
        difficultyOptions = new int[1] { 0 };
        PlayerPrefs.DeleteKey("Difficulties");
    }
}
