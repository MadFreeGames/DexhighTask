using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// Character script that handles movement, animation, and status HUD
public class Character : MonoBehaviour
{
    // Reference to the status controller
    private StatusController statusController;

    // Status HUD prefab
    public GameObject statusHUD;

    // Radius within which the character moves
    public float moveRadius = 10f;

    // Reference to the NavMesh Agent component
    private NavMeshAgent agent;

    // Reference to the Animator component
    private Animator animator;

    // Rotation speed
    public float rotationSpeed = 720f;

    // Random position for movement
    private Vector3 randPos;

    // Particle systems for health gain and loss
    public ParticleSystem healthGain;
    public ParticleSystem healthLoss;

    private void Start()
    {
        CreatestatusHUD();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        randPos = GameManager.Instance.GetRandomPos();
    }

    void Update()
    {
        if (!agent.pathPending && agent.remainingDistance < 0.1f)
        {
            randPos = GameManager.Instance.GetRandomPos();
            RotateEnemy(randPos);
        }
        agent.SetDestination(randPos);
        if (agent.velocity.magnitude > 0.1f)
        {
            animator.SetBool("isWalking", true);
        }
        else
        {
            animator.SetBool("isWalking", false);
        }
    }

    // Create the status HUD
    public void CreatestatusHUD()
    {
        GameObject statusHUDClone = Instantiate(statusHUD, GameManager.Instance.UIManager.transform);
        GameManager.Instance.GetCharacterHUD(transform, statusHUDClone.transform);
        statusController = statusHUDClone.GetComponent<StatusController>();
    }

    // Rotate the character towards a direction
    void RotateEnemy(Vector3 direction)
    {
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    // Handle trigger enter events
    public void OnTriggerEnter(Collider other)
    {
        randPos = GameManager.Instance.GetRandomPos();
        if (other.CompareTag("Enemy") || other.CompareTag("Player"))
        {
            if (statusController)
            {
                statusController.healthBar.DecreaseHealth(Random.Range(50, 100));
                statusController.manaBar.DecreaseMana(Random.Range(10, 50));
                healthLoss.Play();
            }
        }

        if (other.CompareTag("GainTrigger"))
        {
            if (statusController)
            {
                statusController.healthBar.IncreaseHealth(Random.Range(50, 100));
                statusController.manaBar.IncreaseMana(Random.Range(10, 50));
                healthGain.Play();
            }
        }
    }
}
