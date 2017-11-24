using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Fungus;

public class UIController_r : MonoBehaviour {

    GameManager_r _GameManager;
    AudioManager _AudioManager;
    public Sprite emtpyInv;

	// Use this for initialization
	void Start () {
		_GameManager = GameObject.Find("GameManager").GetComponent<GameManager_r>();
        _AudioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
	}

    public void OnClickInventory(GameObject slot) {
        _AudioManager.PlayClip(_AudioManager.clickSound, _AudioManager.GetChannel("SFX"));
        if (slot.GetComponent<InventorySlot_r>().item && !_GameManager.carryingItem && _GameManager.partyMoney >= slot.GetComponent<InventorySlot_r>().item.GetComponent<Item_r>().price) {
            Debug.Log(slot.GetComponent<InventorySlot_r>().item.GetComponent<Item_r>().price +  " <= " + _GameManager.partyMoney);
            _GameManager.carryingItem = slot.GetComponent<InventorySlot_r>().item;
            slot.GetComponent<Image>().sprite = emtpyInv;
            slot.GetComponent<InventorySlot_r>().item = null;
            _GameManager.carryingItem = Instantiate(_GameManager.carryingItem);
            _GameManager.carryingItem.transform.SetParent(GameObject.Find("GameCanvas").transform, false);
            _GameManager.carryingItem.transform.SetAsLastSibling();
            _GameManager.carryingItem.GetComponent<Item_r>().slot = slot;
            _GameManager.carryingItem.GetComponent<Item_r>().character = slot.transform.parent.gameObject;
            if (_GameManager.carryingItem.GetComponent<Item_r>().encounterOnTake) {
                _GameManager.ChangeScreen(GameManager_r.ScreenType.STEncounter);
                GameObject.Find("LevelScripting").GetComponent<Flowchart>().ExecuteBlock(_GameManager.carryingItem.GetComponent<Item_r>().encounterToExecute);
            }
        }
        else if (!slot.GetComponent<InventorySlot_r>().item && _GameManager.carryingItem) {
            if (_GameManager.carryingItem.GetComponent<Image>()) {
                Destroy(_GameManager.carryingItem.GetComponent<Image>());
                _GameManager.carryingItem.gameObject.SetActive(false);
                //Destroy(_GameManager.carryingItem.gameObject);
            }
            slot.GetComponent<InventorySlot_r>().item = _GameManager.carryingItem;
            slot.GetComponent<InventorySlot_r>().item.GetComponent<Item_r>().character = slot.transform.parent.gameObject;
            slot.GetComponent<InventorySlot_r>().item.GetComponent<Item_r>().slot = slot;
            slot.transform.parent.GetComponent<Character_r>().AddTrust(slot.GetComponent<InventorySlot_r>().item.GetComponent<Item_r>().trustGiveMod);
            if (slot.GetComponent<InventorySlot_r>().item.GetComponent<Item_r>().price != 0) {
            bool anotherSlot = true;
            List<GameObject> maps = new List<GameObject>();
            maps.AddRange(GameObject.FindGameObjectsWithTag("ScreenWorldMap"));
            maps.AddRange(GameObject.FindGameObjectsWithTag("ScreenHubMap"));
            maps.AddRange(GameObject.FindGameObjectsWithTag("Map"));
            foreach (GameObject obj in maps)
                if (obj.GetComponent<Map_r>()) {
                    foreach (GameObject loc in obj.GetComponent<Map_r>().locations)
                        foreach (GameObject invSlot in loc.GetComponent<Location_r>().inventory)
                            if (invSlot == slot)
                                anotherSlot = false;
                }
                else if (obj.GetComponent<Movable_Map>())
                     foreach (GameObject loc in obj.GetComponent<Movable_Map>().locations)
                        foreach (GameObject invSlot in loc.GetComponent<Location_r>().inventory)
                            if (invSlot == slot)
                                anotherSlot = false;

            if (anotherSlot) {
                _GameManager.AddMoney(-slot.GetComponent<InventorySlot_r>().item.GetComponent<Item_r>().price);
                slot.GetComponent<InventorySlot_r>().item.GetComponent<Item_r>().price = 0;
            }
            }

            slot.GetComponent<Image>().sprite = slot.GetComponent<InventorySlot_r>().item.GetComponent<Item_r>().itemSprite;
            _GameManager.carryingItem = null;
            if (slot.GetComponent<InventorySlot_r>().item.GetComponent<Item_r>().encounterOnGive) {
                _GameManager.ChangeScreen(GameManager_r.ScreenType.STEncounter);
                GameObject.Find("LevelScripting").GetComponent<Flowchart>().ExecuteBlock(slot.GetComponent<InventorySlot_r>().item.GetComponent<Item_r>().encounterToExecute);
            }
        }
    }

    public void BackToMap() {
        if (_GameManager.currentScreen == GameManager_r.ScreenType.STHubMap)
            _GameManager.ChangeScreen(GameManager_r.ScreenType.STWorldMap);
        else {
            _GameManager.SwitchToHub(_GameManager.iCurrentHub);
        }
    }

    public void ChangeSFXVolume(float vol) {
        GameObject.Find("AudioManager").GetComponent<AudioManager>().GetChannel("SFX").volume = vol;
    }

    public void ChangeMusicVolume(float vol) {
        GameObject.Find("AudioManager").GetComponent<AudioManager>().GetChannel("Music").volume = vol;
    }

    public void ChangeAmbientVolume(float vol) {
        GameObject.Find("AudioManager").GetComponent<AudioManager>().GetChannel("Ambient").volume = vol;
    }

    public void ChangeMasterVolume(float vol) {
        GameObject.Find("AudioManager").GetComponent<AudioManager>().masterVolume = vol;
    }

    public void ClosePanel(GameObject panel) {
        panel.SetActive(false);
    }

}
