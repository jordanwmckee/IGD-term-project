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
    private Rigidbody npcRigidbody;
    public Animator animator;

    private void Start()
    {
        npcRigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (hasStarted)
        {
                // Move the NPC forward at a constant speed
                npcRigidbody.velocity = new Vector3(speed, 0f, 0f);

        }

        if (transform.position.x > finishLineXPosition) 
        {
            animator.SetTrigger("NPCFinish"); // Trigger animation when the object reaches the finish line
        }
    }

    public void StartRace()
    {
        hasStarted = true;
    }
}