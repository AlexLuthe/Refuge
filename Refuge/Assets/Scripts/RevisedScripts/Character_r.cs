using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Character_r : MonoBehaviour
{
    public string charName, bio;
    public GameObject[] inventory;
    public float health, hunger, thirst; // Normalized
    public bool injured, cholera, dysentery, typhoid;
    public bool isDead = false;
    public int trust = 50;

    public Sprite sprite;
    GameObject UISprite; // Button
    public GameObject UIHealth, UIHunger, UIThirst, UITrust; // Sliders

    public void AddHealth(float modifier) { health += modifier; health = Mathf.Clamp01(health); UIHealth.GetComponent<Slider>().value = health; }
    public float GetHealth() { return health; }
    public void AddThirst(float modifier) { thirst += modifier; thirst = Mathf.Clamp01(thirst); UIThirst.GetComponent<Slider>().value = thirst; }
    public float GetThirst() { return thirst; }
    public void AddTrust(int modifier) { trust += modifier; UITrust.GetComponent<Slider>().value = trust; }


    public void AddHunger(float modifier)
    {
        hunger += modifier; hunger = Mathf.Clamp01(hunger); UIHunger.GetComponent<Slider>().value = hunger;

        if (childOne != null || childTwo != null)
        {
            if (hunger >= 0.75)
            {
                AddTrust(-5);
            }
        }

        if (parentOne != null)
        {
            parentOne.GetComponent<Character_r>().AddTrust(15);

            if (parentTwo != null)
            {
                parentTwo.GetComponent<Character_r>().AddTrust(15);
            }
        }

        AddTrust(10);
    }

    public float GetHunger() { return hunger; }

    public GameObject injurySprite;
    public GameObject choleraSprite;
    public GameObject dysenterySprite;
    public GameObject typhoidSprite;
    [Space(1)]

    [Header("Family")]
    public GameObject significantOther = null;
    public GameObject parentOne = null;
    public GameObject parentTwo = null;
    public GameObject childOne = null;
    public GameObject childTwo = null;
    
    void Update()
    {
        if(health <= 0)
        {
            isDead = true;
            OnDeath();
        }

        if(trust <= 0)
        {
            OnNoTrust();
        }

        if(trust >= 100)
        {
            trust = 100;
        }
    }

    public void UseItem () {
        GameManager_r GM = GameObject.Find("GameManager").GetComponent<GameManager_r>();
        Item_r item = GM.carryingItem.GetComponent<Item_r>();
        AddHealth (item.healthMod);
        AddThirst (item.thirstMod);
        AddHunger (item.hungerMod);
        Destroy(item.gameObject);
        GameObject.Find("GameManager").GetComponent<GameManager_r>().carryingItem = null;

        if (Random.Range(0f, 1f) < item.injuryChance)
        {
            injured = true;
            GM.conditionReportText.text = charName + " has gotten injured! ";
            StartCoroutine(GM.HasGottenHealthCondition());
            injurySprite.SetActive(true);
        }

        if (Random.Range(0f, 1f) < item.choleraChance)
        {
            cholera = true;
            GM.conditionReportText.text = charName + " has gotten cholera! ";
            StartCoroutine(GM.HasGottenHealthCondition());
            choleraSprite.SetActive(true);
        }

        if (Random.Range(0f, 1f) < item.dysenteryChance)
        {
            dysentery = true;
            GM.conditionReportText.text = charName + " has gotten dysentery! ";
            StartCoroutine(GM.HasGottenHealthCondition());
            dysenterySprite.SetActive(true);
        }

        if (Random.Range(0f, 1f) < item.typhoidChance)
        {
            typhoid = true;
            GM.conditionReportText.text = charName + " has gotten typhoid! ";
            StartCoroutine(GM.HasGottenHealthCondition());
            typhoidSprite.SetActive(true);
        }

            if (item.itemType == Item_r.ItemType.ITCureAll)
        {
            typhoid = false;
            dysentery = false;
            cholera = false;
            injured = false;
            injurySprite.SetActive(true);
            choleraSprite.SetActive(true);
            dysenterySprite.SetActive(true);
            typhoidSprite.SetActive(true);
        }

        if (item.itemType == Item_r.ItemType.ITHeal)
            AddHealth(1);
    }

    public void AddItem (GameObject item) {
        for (int index = 0; index < inventory.Length; ++index) {
            if (!inventory[index].GetComponent<InventorySlot_r>().item) {
                inventory[index].GetComponent<InventorySlot_r>().item = item;
                inventory[index].GetComponent<Image>().sprite = item.GetComponent<Item_r>().itemSprite;
            }
        }
    }

    public void OnDeath()
    {
        GameManager_r GM = GameObject.Find("GameManager").GetComponent<GameManager_r>();
        GM.conditionReportText.text = charName + " Has Died!";
        StartCoroutine(GM.HasGottenHealthCondition());
        Debug.Log(charName + " has died");
        Destroy(gameObject);
    }

    public void OnNoTrust()
    {
        GameManager_r GM = GameObject.Find("GameManager").GetComponent<GameManager_r>();
        GM.conditionReportText.text = charName + " Has Left!";
        StartCoroutine(GM.HasGottenHealthCondition());
        Debug.Log(charName + " has left");
        Destroy(gameObject);
    }
}

