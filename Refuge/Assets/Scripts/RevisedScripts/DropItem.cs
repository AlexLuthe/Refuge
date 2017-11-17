using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Fungus;

public class DropItem : MonoBehaviour, IPointerUpHandler {

    GameManager_r _GameManager;
    GameObject confirmDropDialogue;

	// Use this for initialization
	void Start () {
		_GameManager = GameObject.Find("GameManager").GetComponent<GameManager_r>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void IPointerUpHandler.OnPointerUp(PointerEventData eventData) {
        if (_GameManager.carryingItem) {
            if (_GameManager.carryingItem.GetComponent<Image>()) {
                if (_GameManager.carryingItem.GetComponent<Item_r>().encounterOnDrop) {
                    _GameManager.ChangeScreen(GameManager_r.ScreenType.STEncounter);
                    GameObject.Find("LevelScripting").GetComponent<Flowchart>().ExecuteBlock(_GameManager.carryingItem.GetComponent<Item_r>().encounterToExecute);
                }
                else
                    confirmDropDialogue.SetActive(true);
            }
        }
    }

    void DestroyItem() {
        confirmDropDialogue.SetActive(false);
        _GameManager.carryingItem.GetComponent<Item_r>().character.GetComponent<Character_r>().AddTrust(_GameManager.carryingItem.GetComponent<Item_r>().trustDropMod);
        Destroy(_GameManager.carryingItem.GetComponent<Image>());
        Destroy(_GameManager.carryingItem.gameObject);
        _GameManager.carryingItem = null;
    }

    void ReturnItem() {
        confirmDropDialogue.SetActive(false);
        if (_GameManager.carryingItem.GetComponent<Image>()) {
            Destroy(_GameManager.carryingItem.GetComponent<Image>());
            Destroy(_GameManager.carryingItem.gameObject);
        }
        _GameManager.carryingItem.GetComponent<Item_r>().slot.GetComponent<InventorySlot_r>().item = _GameManager.carryingItem;
        _GameManager.carryingItem = null;
    }
}
