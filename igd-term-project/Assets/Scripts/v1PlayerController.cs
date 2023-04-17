using System.Security.AccessControl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using System;

public class v1PlayerController : MonoBehaviour
{
    // Variables to control car movement
    public float speed = 10f;
    public float acceleration = 5f;
    public float maxSpeed = 50f;
    public float finishLineXPosition = 25f; 
    public Animator carAnimator; 
    // public Animator finishAnimator; 
    public Button startButton; 
    public GameObject SpeechBubble;
    
    private Rigidbody carRigidbody;
    private bool isAccelerating = false;
    private bool hasStarted = false; 
    public bool hasFinished = false;
    private Vector3 middleScreenPosition; 

    // References to UI elements
    public TMP_InputField answerInputField;
    public TextMeshProUGUI problemText;

    private int difficulty;
    private int[] operands;
    private int opMax;
    private int correctAnswer;

    private LevelFinishManager levelFinishManager;
    public static v1PlayerController instance;
    private v1NPCScript npc;

    private void Awake() {
        instance = this;
    }

    private void Start()
    {
        difficulty = MenuManager.instance.getDifficulty();
        operands = new int[5];
        opMax = 10;
        levelFinishManager = LevelFinishManager.instance;
        npc = v1NPCScript.instance;
        carRigidbody = GetComponent<Rigidbody>();
        GenerateProblem();
        
        startButton.onClick.AddListener(StartGame); 
        middleScreenPosition = new Vector3(transform.position.x, transform.position.y, 0f); 
    }

    private void FixedUpdate()
    {
        if (npc.hasFinished) {
            SetFinished();
        }

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
            SetFinished();
            // open panel for game end that indicates win/loss
            levelFinishManager.endRace("YOU WIN!", true);
        }
    }
    
    private void Update(){
        //Check if the user has pressed enter
        if(Input.GetKeyDown(KeyCode.Return)){
            CheckAnswer();
        }
    }

    private void SetFinished() {
        carRigidbody.velocity = Vector3.zero;
        // finishAnimator.SetTrigger("Finish"); // Trigger animation when the object reaches the finish line
        hasStarted = false;
        hasFinished = true;
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
            operands[i] = UnityEngine.Random.Range(-opMax, opMax);
        }

        // Weed out some negatives
        for (int i = 0; i < 5; i++)
        {
            if (operands[i] < 0)
            {
                if (1 == UnityEngine.Random.Range(0, 100) % 4)
                {
                    operands[i] = operands[i];
                }
                else
                {
                    operands[i] = -operands[i];
                }
            }
        }


        correctAnswer = 1;
        bool open = false;
        // First term inside parenthesis?
            // First term negative and need wrapped?
        string problemString = "";
        if (1 == UnityEngine.Random.Range(0, 100) % 10)
        {
            if (operands[0] < 0) 
            {
                problemString = $"(({operands[0]}) ";
                open = true;
            }
            else
            {
                problemString = $"({operands[0]} ";
                open = true;
            }
        }
        else
        {
            if (operands[0] < 0)
            {
                problemString = $"({operands[0]}) ";
            }
            else
            {
                problemString = $"{operands[0]} ";
            }
        }

        for (int i = 0; i < difficulty; i++)
        {
            // Which operator are we using this time?
            int operatorIndex = UnityEngine.Random.Range(0, 3);

            // Wrap negative operands in parenthesis
                // Open or close parenthesis as needed
            string operand;
            if (operands[i + 1] < 0)
            {
                if (1 == UnityEngine.Random.Range(0, 100) % 10)
                {
                    if (open)
                    {
                        open = false;
                        operand = $"({operands[i + 1]})) ";
                    }
                    else
                    {
                        open = true;
                        operand = $"(({operands[i + 1]}) ";

                    }
                }
                else
                {
                    operand = $"({operands[i + 1]}) ";

                }
            }
            else
            {
                if (1 == UnityEngine.Random.Range(0, 100) % 10)
                {
                    if (open)
                    {
                        open = false;
                        operand = $"{operands[i + 1]}) ";
                    }
                    else
                    {
                        open = true;
                        operand = $"({operands[i + 1]} ";

                    }
                }
                else
                {
                    operand = $"{operands[i + 1]} ";

                }
            }

            // append the operator to the front of the string
            switch (operatorIndex)
            {
                case 0:
                    problemString += $"+ {operand}";
                    break;
                case 1:
                    problemString += $"- {operand}";
                    break;
                case 2:
                    problemString += $"* {operand}";
                    break;
                }
        }

        if (open)
        {
            problemString += ") ";
        }
        correctAnswer = EvaluateExpression(problemString);
        problemString += "= ";
        Debug.Log(correctAnswer);
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

    public static int EvaluateExpression(string expression)
    {
        // Remove all spaces from the expression
        expression = expression.Replace(" ", "");

        var result = 0;
        var operatorType = '+';
        var number = "";
        var stack = new Stack<int>();
        var lastOperatorStack = new Stack<char>();
        bool negging = false;
        char lastChar = ' ';

        foreach (var ch in expression)
        {

            if (lastChar == '(')
            {
                negging = true;
            } 
            else
            {
                negging = false;
            }
            lastChar = ch;
            if (Char.IsDigit(ch) || (ch == '-' && negging))
            {
                number += ch;
            }
            else if (ch == '+' || ch == '-' || ch == '*')
            {
                if (number.Length > 0)
                {
                    result = ApplyOperator(result, Int32.Parse(number), operatorType);
                    number = "";
                }

                operatorType = ch;
            }
            else if (ch == '(')
            {
                stack.Push(result);
                lastOperatorStack.Push(operatorType);
                result = 0;
                operatorType = '+';
            }
            else if (ch == ')')
            {
                if (number.Length > 0)
                {
                    result = ApplyOperator(result, Int32.Parse(number), operatorType);
                    number = "";
                }

                var lastOperator = lastOperatorStack.Pop();
                var lastResult = stack.Pop();

                result = ApplyOperator(lastResult, result, lastOperator);
            }
            else
            {
                throw new ArgumentException($"Invalid character: {ch}");
            }
        }

        if (number.Length > 0)
        {
            result = ApplyOperator(result, Int32.Parse(number), operatorType);
        }

        return result;
    }

    private static int ApplyOperator(int leftOperand, int rightOperand, char operatorType)
    {
        switch (operatorType)
        {
            case '+':
                return leftOperand + rightOperand;
            case '-':
                return leftOperand - rightOperand;
            case '*':
                return leftOperand * rightOperand;
            default:
                throw new ArgumentException($"Invalid operator: {operatorType}");
        }
    }
}