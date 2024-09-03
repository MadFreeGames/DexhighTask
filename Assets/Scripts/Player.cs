using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// Player script that handles movement, animation, and status
public class Player : MonoBehaviour
{
    // Reference to the status controller component
    private StatusController statusController;

    // Status HUD prefab
    public GameObject statusHUD;

    // Reference to the joystick component
    private Joystick joystick;

    // Speed of the player
    public float speed = 5f;

    // NavMesh Agent component reference
    private NavMeshAgent agent;

    // Animator component reference
    private Animator animator;

    // Rotation speed
    public float rotationSpeed = 720f;

    // Particle systems for health gain and loss
    public ParticleSystem healthGain;
    public ParticleSystem healthLoss;

    private void Start()
    {
        // Get references to the NavMesh Agent and Animator components
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        // Get the joystick component from the UIManager
        joystick = UIManager.instance.joystick;

        // Create the status HUD
        CreatestatusHUD();
    }

    // Create the status HUD
    public void CreatestatusHUD()
    {
        GameObject statusHUDClone = Instantiate(statusHUD, UIManager.instance.transform);
        UIManager.instance.statusHUD.Add((transform, statusHUDClone.transform));
        statusController = statusHUDClone.GetComponent<StatusController>();
    }

    void Update()
    {
        HandleMovement();
    }

    // Handle player movement
    void HandleMovement()
    {
        float horizontal = joystick.Horizontal;
        float vertical = joystick.Vertical;

        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude > 0.3f)
        {
            MovePlayer(direction);
            RotatePlayer(direction);
            animator.SetBool("isWalking", true);
        }
        else
        {
            agent.velocity = Vector3.zero;
            agent.ResetPath();
            animator.SetBool("isWalking", false);
        }
    }

    // Move the player in a direction
    void MovePlayer(Vector3 direction)
    {
        Vector3 moveTarget = transform.position + direction * speed * Time.deltaTime;
        agent.SetDestination(moveTarget);
    }

    // Rotate the player to face a direction
    void RotatePlayer(Vector3 direction)
    {
        Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    // Handle trigger enter events
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (statusController)
            {
                statusController.healthBar.DecreaseHealth(50);
                statusController.manaBar.DecreaseMana(10);
                healthLoss.Play();
            }
        }
        if (other.CompareTag("GainTrigger"))
        {
            if (statusController)
            {
                statusController.healthBar.IncreaseHealth(50);
                statusController.manaBar.IncreaseMana(10);
                healthGain.Play();
            }
        }
    }
}
