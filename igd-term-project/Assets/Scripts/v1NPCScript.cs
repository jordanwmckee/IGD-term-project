using System.Security.AccessControl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine;

public class v1NPCScript : MonoBehaviour
{
    public float speed = 20f; // Speed of the NPC car
    public float finishLineXPosition = 25f; 
    public float reactionTime = 1.5f; // Time it takes for the NPC to react to the player's movement
    public float offset = 3f; // Offset from the center of the road
    public float waitTimer = 3f; // Timer to move across the screen
    private bool hasStarted = false;
    public bool hasFinished = false;
    private Rigidbody npcRigidbody;
    // public Animator animator;

    private LevelFinishManager levelFinishManager;
    public static v1NPCScript instance;
    private v1PlayerController player;

    private void Awake() {
        Debug.Log("NPC is Awake!");
        instance = this;
    }

    private void Start()
    {
        levelFinishManager = LevelFinishManager.instance;
        player = v1PlayerController.instance;
        npcRigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (player.hasFinished) {
            SetFinished();
        }
        if (hasStarted)
        {
            // Move the NPC forward at a constant speed
            npcRigidbody.velocity = new Vector3(speed, 0f, 0f);

        }

        if (transform.position.x > finishLineXPosition) 
        {
            SetFinished();
            levelFinishManager.endRace("YOU LOSE...", false);
        }
    }

    private void SetFinished() {
        npcRigidbody.velocity = Vector3.zero;
        // animator.SetTrigger("NPCFinish"); // Trigger animation when the object reaches the finish line
        hasFinished = true;
    }

    public void StartRace()
    {
        hasStarted = true;
    }
}