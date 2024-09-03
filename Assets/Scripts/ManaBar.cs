using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManaBar : MonoBehaviour
{
    // UI elements for the mana bar
    public Slider manaSlider;          // Slider that represents the mana level
    public Image relativeMana;         // Background image for the relative mana

    // Mana properties
    public float maxMana = 100f;       // Maximum mana value
    public float currentMana;          // Current mana value

    public float smoothTime = 0.5f;    // Time for smooth mana transition

    // Coroutines to manage mana change smoothly
    private Coroutine manaChangeCoroutine;           // Coroutine for the main mana change
    private Coroutine relativeManaChangeCoroutine;   // Coroutine for the relative mana change

    // Initialize the mana values and UI elements
    void Start()
    {
        manaSlider.maxValue = maxMana;       
        LoadMana(maxMana);                  
        currentMana = maxMana;               
        relativeMana.fillAmount = 1f;        
    }

    // Load a specific amount of mana into the slider
    public void LoadMana(float _mana)
    {
        manaSlider.value = _mana;
    }

    // Increase mana by a specific amount and update both the main and relative mana bars
    public void IncreaseMana(float manaRegenRate)
    {
        float targetMana = Mathf.Min(currentMana + manaRegenRate, maxMana); 
        StartManaChange(targetMana);                                         
        StartRelativeManaChange(targetMana);                                
    }

    // Decrease mana by a specific amount and update both the main and relative mana bars
    public void DecreaseMana(float manaUsageRate)
    {
        float targetMana = Mathf.Max(currentMana - manaUsageRate, 0f);      
        StartManaChange(targetMana);                                         
        StartRelativeManaChange(targetMana);                                 
    }

    // Start the coroutine for changing the main mana smoothly
    private void StartManaChange(float targetMana)
    {
        if (manaChangeCoroutine != null)
        {
            StopCoroutine(manaChangeCoroutine);     
        }
        manaChangeCoroutine = StartCoroutine(SmoothManaChange(targetMana)); 
    }

    // Start the coroutine for changing the relative mana smoothly
    private void StartRelativeManaChange(float targetMana)
    {
        if (relativeManaChangeCoroutine != null)
        {
            StopCoroutine(relativeManaChangeCoroutine);   
        }
        relativeManaChangeCoroutine = StartCoroutine(SmoothRelativeManaChange(targetMana)); 
    }

    // Coroutine for smoothly changing the main mana value
    private IEnumerator SmoothManaChange(float targetMana)
    {
        float velocity = 0f;  // Variable to control the speed of SmoothDamp
        while (!Mathf.Approximately(currentMana, targetMana))  
        {
            currentMana = Mathf.SmoothDamp(currentMana, targetMana, ref velocity, smoothTime); 
            manaSlider.value = currentMana;  
            yield return null;  
        }
        currentMana = targetMana;  
        manaSlider.value = currentMana;  
    }

    // Coroutine for smoothly changing the relative mana image
    private IEnumerator SmoothRelativeManaChange(float targetMana)
    {
        float velocity = 0f;  
        float currentRelativeMana = relativeMana.fillAmount * maxMana; 

        while (!Mathf.Approximately(currentRelativeMana, targetMana))  
        {
            currentRelativeMana = Mathf.SmoothDamp(currentRelativeMana, targetMana, ref velocity, smoothTime * 1.5f);
            relativeMana.fillAmount = currentRelativeMana / maxMana;  
            yield return null;  
        }
        relativeMana.fillAmount = targetMana / maxMana;  
    }
}
