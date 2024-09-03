using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Game manager script that handles the game's logic
public class GameManager : MonoBehaviour
{
    // Singleton instance of the game manager
    public static GameManager Instance;

    // Array of random points for character spawning
    public Transform[] randomPoints;

    // Reference to the UI manager
    [HideInInspector]
    public UIManager UIManager;

    // Character prefab
    public GameObject characterPrefab;

    // Number of characters to spawn
    public int characterCount;

    private void Awake()
    {
        // Initialize the singleton instance
        Instance = this;
    }

    void Start()
    {
        // Get the UI manager instance
        UIManager = UIManager.instance;

        // Create characters
        CreateCharacter();
    }

    // Create characters
    public void CreateCharacter()
    {
        // Loop through the character count
        for (int i = 0; i < characterCount; i++)
        {
            // Instantiate a character at a random position
            Instantiate(characterPrefab, GetRandomPos(), Quaternion.identity);
        }
    }

    // Get the character HUD
    public void GetCharacterHUD(Transform character, Transform hud)
    {
        // Add the character and HUD to the UI manager's status HUD list
        UIManager.instance.statusHUD.Add((character, hud));
    }

    // Get a random position from the random points array
    public Vector3 GetRandomPos()
    {
        // Return a random position from the array
        return randomPoints[Random.Range(0, randomPoints.Length)].position;
    }
}
