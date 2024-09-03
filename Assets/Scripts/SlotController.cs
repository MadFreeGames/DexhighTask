using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotController : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject slotPrefab;
    public int numberofSlots;

    void Start()
    {
        numberofSlots = Random.Range(0, 4);// Taking random slots
        CreateSlots();
    }
    public void CreateSlots()
    {
        for (int i = 0; i < numberofSlots; i++)
        {
            GameObject slotClone = Instantiate(slotPrefab, transform);
            slotClone.transform.GetChild(0).GetComponent<Image>().sprite = UIManager.instance.slotPowerSprites[i];
        }
    }
}