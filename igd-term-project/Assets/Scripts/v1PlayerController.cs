using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class v1PlayerController : MonoBehaviour
{
    // Variables to control car movement
    public float speed = 10f;
    public float acceleration = 5f;
    public float maxSpeed = 50f;

    private Rigidbody carRigidbody;
    private bool isAccelerating = false;

    // References to UI elements
    public TMP_InputField answerInputField;
    public TextMeshProUGUI problemText;

    private int num1;
    private int num2;
    private int correctAnswer;

    private void Start()
    {
        carRigidbody = GetComponent<Rigidbody>();
        GenerateProblem();
    }

    private void FixedUpdate()
    {
        // Check if the car is accelerating
        if (isAccelerating)
        {
            // Increase the car's speed up to the maximum speed
            if (carRigidbody.velocity.magnitude < maxSpeed)
            {
                carRigidbody.AddForce(transform.forward * acceleration);
            }
        }

        // Move the car forward at a constant speed if not accelerating
        else
        {
            carRigidbody.velocity = transform.forward * speed;
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

    private void GenerateProblem()
    {
        // Generate two random numbers and a random operator (+, -, *, /)
        num1 = Random.Range(1, 10);
        num2 = Random.Range(1, 10);
        int operatorIndex = Random.Range(0, 4);
        char op = '+';
        switch (operatorIndex)
        {
            case 0:
                correctAnswer = num1 + num2;
                break;
            case 1:
                op = '-';
                correctAnswer = num1 - num2;
                break;
            case 2:
                op = '*';
                correctAnswer = num1 * num2;
                break;
            case 3:
                op = '/';
                num1 = Random.Range(1, 10) * num2;
                correctAnswer = num1 / num2;
                break;
        }

        // Display the problem in the UI
        problemText.text = $"{num1} {op} {num2} =";
        answerInputField.text = "";
    }
}