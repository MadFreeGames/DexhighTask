using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Health bar script that handles the player's health
public class HealthBar : MonoBehaviour
{
    // Horizontal layout group for the health bar
    public HorizontalLayoutGroup horizontalLayoutGroup;

    // RectTransform for the health bar
    public RectTransform healthBar;

    // Transform for the group lines
    public Transform groupLines;

    // Image for the relative health
    public Image relativeHealth;

    // Slider for the health
    public Slider healthSldier;

    // Coroutine for the mana change
    private Coroutine manaChangeCoroutine;

    // Coroutine for the relative mana change
    private Coroutine relativeManaChangeCoroutine;

    // Maximum health
    public int maxHealth;

    // Maximum block health (100 health for each block)
    public int maxBlockHealth;

    // Block count according to total health
    private int blockCount;

    // Lines space
    private float linesSpace;

    // Current health
    private float currentHealth;

    // Smooth time for the health change
    public float smoothTime = 0.5f;

    // Remainder
    float remainder;

    void Start()
    {
        // Initialize the current health to a random value between 100 and maxHealth
        currentHealth = Random.Range(100, maxHealth);

        // Calculate the remainder
        remainder = currentHealth % maxBlockHealth;

        // Calculate the block count
        blockCount = ((int)(currentHealth)) / maxBlockHealth;

        // Set the maximum value of the health slider to the current health
        healthSldier.maxValue = currentHealth;

        // Set the maximum health to the current health
        maxHealth = ((int)(currentHealth));

        // Find the remainder space
        FindRemainderSpace();

        // Activate the group lines
        ActiveGroupLines();

        // Load the health
        LoadHealth(currentHealth);

        // Set the relative health to full
        relativeHealth.fillAmount = 1f;
    }

    // Find the remainder space
    float width;

    void FindRemainderSpace()
    {
        // Calculate the percentage of the remainder
        float part = remainder;
        float whole = maxHealth;
        float percentage = (part / whole) * 100;

        // Calculate the width of the health bar
        width = healthBar.rect.width;

        // Calculate the result
        float number = width;
        float widthpercentage = percentage;
        float result = (number / 100) * widthpercentage;

        // Calculate the width
        width -= result;
    }

    // Load the health
    public void LoadHealth(float _health)
    {
        healthSldier.value = _health;
    }

    // Activate the group lines
    private void ActiveGroupLines()
    {
        linesSpace = width / (blockCount);
        horizontalLayoutGroup.spacing = linesSpace;
        groupLines.GetChild(0).gameObject.name = "Line " + 1;
        for (int i = 0; i < blockCount; i++)
        {
            GameObject line = Instantiate(groupLines.GetChild(0).gameObject, groupLines);
            line.name = "Line " + (i + 2);
        }
    }

    // Increase the health
    public void IncreaseHealth(float healthRegenRate)
    {
        float targetMana = Mathf.Min(currentHealth + healthRegenRate, maxHealth);
        StartManaChange(targetMana);
        StartRelativeManaChange(targetMana);
    }

    // Decrease the health
    public void DecreaseHealth(float healthUsageRate)
    {
        float targetMana = Mathf.Max(currentHealth - healthUsageRate, 0f);
        StartManaChange(targetMana);
        StartRelativeManaChange(targetMana);
    }

    // Start the mana change coroutine
    private void StartManaChange(float targetMana)
    {
        if (manaChangeCoroutine != null)
        {
            StopCoroutine(manaChangeCoroutine);
        }
        manaChangeCoroutine = StartCoroutine(SmoothManaChange(targetMana));
    }

    // Start the relative mana change coroutine
    private void StartRelativeManaChange(float targetMana)
    {
        if (relativeManaChangeCoroutine != null)
        {
            StopCoroutine(relativeManaChangeCoroutine);
        }
        relativeManaChangeCoroutine = StartCoroutine(SmoothRelativeManaChange(targetMana));
    }

    // Smooth mana change coroutine
    private IEnumerator SmoothManaChange(float targetMana)
    {
        float velocity = 0f;

        while (!Mathf.Approximately(currentHealth, targetMana))
        {
            currentHealth = Mathf.SmoothDamp(currentHealth, targetMana, ref velocity, smoothTime);

            healthSldier.value = currentHealth;

            yield return null;
        }

        currentHealth = targetMana;

        healthSldier.value = currentHealth;
    }

    // Smooth relative mana change coroutine
    private IEnumerator SmoothRelativeManaChange(float targetMana)
    {
        float velocity = 0f;
        float currentRelativeMana = relativeHealth.fillAmount * maxHealth;
        while (!Mathf.Approximately(currentRelativeMana, targetMana))
        {
            currentRelativeMana = Mathf.SmoothDamp(currentRelativeMana, targetMana, ref velocity, smoothTime * 1.2f);
            relativeHealth.fillAmount = currentRelativeMana / maxHealth;
            yield return null;
        }
        relativeHealth.fillAmount = targetMana / maxHealth;
    }
}
