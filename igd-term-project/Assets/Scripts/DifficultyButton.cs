using UnityEngine;
using UnityEngine.UI;

public class DifficultyButton : MonoBehaviour
{
    public int difficulty;
    public Color selectedColor;
    public Color deselectedColor;
    public bool isSelected = false;

    private void Start()
    {
        if (isSelected) selectDifficulty();
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
                button.GetComponent<Image>().color = deselectedColor;
            } 
        }
        MenuManager.instance.setGameStartScene(difficulty);
    }
}