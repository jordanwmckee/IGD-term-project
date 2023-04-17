using System.Security.AccessControl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class v1PlayerController : MonoBehaviour
{
    // Variables to control car movement
    public float speed = 10f;
    public float acceleration = 5f;
    public float maxSpeed = 50f;
    public float finishLineXPosition = 25f; 
    public Animator carAnimator; 
    public Animator finishAnimator; 
    public Button startButton; 
    public GameObject SpeechBubble;
    
    private Rigidbody carRigidbody;
    private bool isAccelerating = false;
    private bool hasStarted = false; 
    private Vector3 middleScreenPosition; 

    // References to UI elements
    public TMP_InputField answerInputField;
    public TextMeshProUGUI problemText;

    private int difficulty;
    private int[] operands;
    private int opMax;
    private int correctAnswer;

    private void Start()
    {
        difficulty = MenuManager.instance.getDifficulty();
        operands = new int[5];
        opMax = 10;

        carRigidbody = GetComponent<Rigidbody>();
        GenerateProblem();
        
        startButton.onClick.AddListener(StartGame); 
        middleScreenPosition = new Vector3(transform.position.x, transform.position.y, 0f); 
    }

    private void FixedUpdate()
    {
        // Check if the car is accelerating
        if (hasStarted) { 
            if (isAccelerating)
            {
                // Increase the car's speed up to the maximum speed
                if (carRigidbody.velocity.magnitude < maxSpeed)
                {
                    carRigidbody.AddForce(transform.right * acceleration); // Changed direction of force from forward to right
                }
            }

            // Move the car forward at a constant speed if not accelerating
            else
            {
                carRigidbody.velocity = transform.right * speed; // Changed direction of velocity from forward to right
            }
            
            // Follow the player once they are in the middle of the screen
            if(transform.position.x > middleScreenPosition.x){
                Vector3 cameraPos = new Vector3(transform.position.x, 0, -10f); 
                Camera.main.transform.position = cameraPos; 
            }
        }
        
        // Check if the object has reached the finish line
        if (transform.position.x > finishLineXPosition) {
            carRigidbody.velocity = Vector3.zero;
            finishAnimator.SetTrigger("Finish"); // Trigger animation when the object reaches the finish line
            hasStarted = false;
        }
    }
    
    private void Update(){
        //Check if the user has pressed enter
        if(Input.GetKeyDown(KeyCode.Return)){
            CheckAnswer();
        }
    }

    public void CheckAnswer()
    {
        // Check if the answer is correct
        int answer;
        if (int.TryParse(answerInputField.text, out answer)){
            // Set the car to accelerate if the answer is correct
            if (answer == correctAnswer){
                isAccelerating = true;
            }
            else {
                isAccelerating = false;
            }

            // Generate a new problem
            GenerateProblem();
        }
    }

    /*
     * Pseudocoding this shit out!
     * So basically, when generateProblem() is called, the first operand is always generated
     * then depending on difficulty, [1-4] operands will be generated to follow.
     * 
     * 
     * 
     * 
     */

    private void GenerateProblem()
    {
        for (int i = 0; i < 5; i++)
        {
            operands[i] = Random.Range(-opMax, opMax);
        }


        correctAnswer = 1;
        bool open = false;
        string problemString = "";
        if (1 == Random.Range(0, 100) % 2)
        {
            problemString = $"({operands[0]} ";
            open = true;
        }
        else problemString = $"{operands[0]} ";

        for (int i = 0; i < difficulty; i++)
        {
            int operatorIndex = Random.Range(0, 4);
            switch (operatorIndex)
            {
                case 0:
                    if (1 == Random.Range(0, 100) % 2)
                    {
                        if (open)
                        {
                            open = false;
                            problemString += $"+ {operands[i + 1]} ) ";
                        } else
                        {
                            open = true;
                            problemString += $"+ ( {operands[i + 1]} ";
                        }
                    } 
                    else
                    {
                        problemString += $"+ {operands[i + 1]} ";
                    }
                    // correctAnswer += operands[i+1];
                    break;
                case 1:
                    if (1 == Random.Range(0, 100) % 2)
                    {
                        if (open)
                        {
                            open = false;
                            problemString += $"+ {operands[i + 1]} ) ";
                        }
                        else
                        {
                            open = true;
                            problemString += $"+ ( {operands[i + 1]} ";
                        }
                    }
                    else
                    {
                        problemString += $"+ {operands[i + 1]} ";
                    }

                    // correctAnswer -= operands[i+1];
                    break;
                case 2:
                    if (1 == Random.Range(0, 100) % 2)
                    {
                        if (open)
                        {
                            open = false;
                            problemString += $"+ {operands[i + 1]} ) ";
                        }
                        else
                        {
                            open = true;
                            problemString += $"+ ( {operands[i + 1]} ";
                        }
                    }
                    else
                    {
                        problemString += $"+ {operands[i + 1]} ";
                    }

                    // correctAnswer *= operands[i+1];
                    break;
                case 3:
                    if (1 == Random.Range(0, 100) % 2)
                    {
                        if (open)
                        {
                            open = false; 
                            problemString += $"+ {operands[i + 1]} ) ";
                        }
                        else
                        {
                            open = true;
                            problemString += $"+ ( {operands[i + 1]} ";
                        }
                    }
                    else
                    {
                        problemString += $"+ {operands[i + 1]} ";
                    }

                    // operands[0] = Random.Range(1, 10) * operands[1];
                    // correctAnswer = operands[0] / operands[1];
                    break;
            }
        }

        if (open)
        {
            problemString += ") = ";
        }
        else
        {
            problemString += "= ";
        }
        if (hasStarted){ 
            problemText.text = problemString;
            answerInputField.ActivateInputField();
        }
    }
    
    public void StartGame(){ 
        carAnimator.Play("Miamianimation");
        SpeechBubble.gameObject.SetActive(false);
        startButton.gameObject.SetActive(false);
        hasStarted = true;
        GenerateProblem();
    }
}