using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Fungus;

public class Location_r : MonoBehaviour, IPointerClickHandler {

    public bool changeScreen = false;
    public int destinationScreen;

    public string FungusBlockToExecute;
    public bool executeEncounterWithFungus;

    public enum LocationType {
        LTClinic = 0,
        LTAbandonedShop = 1,
        LTMarketplace = 2,
        LTIntermediary = 3,
    }

    public LocationType type;
    public GameObject inventoryUI;
    public GameObject[] inventory;
    public GameObject[] possibleItems;
    public int locationNumber = 0;
    public int distance;
    float encounterChance;
    public string description;
    public Map_r map;
    public Movable_Map mMap;
    bool worldMap = true;
    public bool generated = false;
    public List<GameObject> possibleLocations = new List<GameObject>();
    public AudioManager _AudioManager;
    public int hubArea = 0;

    public bool travelReady = false;

    public void Start() {
        if (!map && !mMap) {
        if (worldMap)
            map = GameObject.FindGameObjectWithTag("ScreenWorldMap").GetComponent<Map_r>();
        else //if (transform.parent.GetComponent<Movable_Map>())
            mMap = transform.parent.GetComponent<Movable_Map>();
        if (!mMap && transform.parent.GetComponent<Map_r>())
            map = transform.parent.GetComponent<Map_r>();
        else if (!mMap && !map)
            Debug.Log("Map could not be found");
        }
        _AudioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
    }

    void IPointerClickHandler.OnPointerClick(PointerEventData eventData) {
        Debug.Log("The user clicked");
        _AudioManager.PlayClip(_AudioManager.clickSound, _AudioManager.GetChannel("SFX"));
        if (map)
            map.Travel(gameObject);
        if (mMap) {
            Debug.Log("Old Location: " + mMap.newLocation);
            mMap.newLocation = transform.position;
            mMap.location = this;
            Debug.Log("New Location: " + mMap.newLocation);
        }

    }

    public virtual void GenerateInventory() {
        if (inventory.Length > 0) {
        if (!generated) {
                Debug.Log(inventory.Length);
            generated = true;
            foreach (GameObject slot in inventory) {
                    Debug.Log(slot);
                    // No Empty RND Slots
                    float rnd = Random.Range(0f, 1f);
                GameObject selectedObj = possibleItems[Random.Range(0, possibleItems.Length)];
                foreach (GameObject item in possibleItems)
                    if (selectedObj) {
                        if (item.GetComponent<Item_r>().spawnChance > rnd && item.GetComponent<Item_r>().spawnChance - rnd > selectedObj.GetComponent<Item_r>().spawnChance - rnd) {
                            selectedObj = item;
                        }
                    }
                slot.GetComponent<InventorySlot_r>().item = selectedObj;
                slot.GetComponent<Image>().sprite = selectedObj.GetComponent<Item_r>().itemSprite;
            }
            if (Random.Range(0f, 1f) > 0.75f) {
                inventory[inventory.Length - 1].GetComponent<InventorySlot_r>().item = null;
                inventory[inventory.Length - 1].GetComponent<Image>().sprite = GameObject.Find("UIController").GetComponent<UIController_r>().emtpyInv;
            }
        }
        }

        if(changeScreen)
        {
            Debug.Log("Tried to change screen");
            if(executeEncounterWithFungus)
            {
                FindObjectOfType<Flowchart>().ExecuteBlock(FungusBlockToExecute);
                GameObject.Find("GameManager").GetComponent<GameManager_r>().ChangeScreen(2);
                Debug.Log("Switched screen to a fungus block");
            }
            else
            {
                GameObject.Find("GameManager").GetComponent<GameManager_r>().SwitchToHub(hubArea);
                GameObject.Find("GameManager").GetComponent<GameManager_r>().ChangeScreen(destinationScreen);
                GameObject.Find("GameManager").GetComponent<GameManager_r>().conditionReportText.text = "";
            }
        }
    }

    public void Scavenge() {
        GenerateInventory();
        inventoryUI.SetActive(true);
    }

    public void StartEncounter() {
        GameObject.Find("GameManager").GetComponent<GameManager_r>().ChangeScreen(GameManager_r.ScreenType.STEncounter);
    }

	// Update is called once per frame
	void Update () {
        //if (locationNumber == map.currentLocationNumber - 1 || locationNumber == map.currentLocationNumber + 1)
        //{
        //    if (travelReady == true)
        //    {
        //        map.Travel(gameObject);
        //        travelReady = false;
        //    }
        //}
	}
}
