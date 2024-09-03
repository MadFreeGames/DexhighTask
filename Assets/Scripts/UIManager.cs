using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// UI manager script that handles the game's UI
public class UIManager : MonoBehaviour
{
    // Singleton instance of the UI manager
    public static UIManager instance;

    // Reference to the joystick
    public VariableJoystick joystick;

    // List of character and HUD pairs for the status HUD
    public List<(Transform character, Transform hud)> statusHUD = new();

    // Offset for the HUD position
    public Vector3 offcet;

    // List of sprites for the slot powers
    public List<Sprite> slotPowerSprites = new();

    private void Awake()
    {
        // Initialize the singleton instance
        if (!instance)
            instance = this;
    }

    private void Update()
    {
        // Loop through the status HUD list
        foreach (var item in statusHUD)
        {
            // Convert the character's world position to screen position
            Vector3 screenPos = Camera.main.WorldToScreenPoint(item.character.position);

            // Set the HUD position to the screen position with an offset
            item.hud.position = screenPos + offcet;
        }
    }
}
