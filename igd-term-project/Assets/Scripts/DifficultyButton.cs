using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;

public class DifficultyButton : MonoBehaviour
{
    public int difficulty;
    public Color selectedColor;
    public Color deselectedColor;
    public Color inactiveColor;
    [SerializeField] private Image locked;
    public bool isSelected = false;
    DifficultyManager difficultyManager;
    private bool isEasy;

    private void Start()
    {
        if (difficulty == 1) isEasy = true;
        difficultyManager = FindObjectOfType<DifficultyManager>();

        // toggle inactive unless unlocked
        checkUnlocked();

        // select button if default is set to selected
        if (isSelected) 
            selectDifficulty();
    }

    private void Update() {
        checkUnlocked();
    }

    private void checkUnlocked() {
        if (difficultyManager.difficultyOptions.Contains(difficulty) && GetComponent<Image>().color == inactiveColor) {
            setActive();
        } else if (!difficultyManager.difficultyOptions.Contains(difficulty)) {
            if (!isEasy)
                setInactive();
        }
    }

    public void OnClick() {
        selectDifficulty();
    }

    private void selectDifficulty() {
        isSelected = true;
        // set background color to active
        GetComponent<Image>().color = selectedColor;

        // unselect all other difficulty buttons
        foreach (DifficultyButton button in transform.parent.GetComponentsInChildren<DifficultyButton>()) {
            if (button != this) {
                button.isSelected = false;
                if (button.GetComponent<Button>().interactable == true) {
                    button.GetComponent<Image>().color = deselectedColor;
                }
            } 
        }
        MenuManager.instance.setGameStartScene(difficulty);
    }

    public void setActive() {
        GetComponent<Image>().color = deselectedColor;
        GetComponent<Button>().interactable = true;
        if (!isEasy) locked.gameObject.SetActive(false);
    }

    public void setInactive() {
        GetComponent<Image>().color = inactiveColor;
        GetComponent<Button>().interactable = false;
        if (!isEasy) locked.gameObject.SetActive(true);
    }
}