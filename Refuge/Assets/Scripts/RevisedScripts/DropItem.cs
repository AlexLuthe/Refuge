﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Fungus;

public class DropItem : MonoBehaviour, IPointerUpHandler {

    GameManager_r _GameManager;
    GameObject confirmDropText;
    public GameObject confirmDropDialogue;

    //public GameObject[] dialogueObjects; 

	// Use this for initialization
	void Start () {
		_GameManager = GameObject.Find("GameManager").GetComponent<GameManager_r>();
        if (!confirmDropDialogue)
            confirmDropDialogue = GameObject.Find("ItemConfirmationPanel");
        confirmDropDialogue.SetActive(false);
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
                else {
                    confirmDropDialogue.SetActive(true);
                    //foreach(GameObject dialogueObj in dialogueObjects)
                    //{
                    //    Debug.Log("Activating: " + dialogueObj.name);
                    //    dialogueObj.SetActive(true);
                    //}
                }
            }
        }
    }

    public void DestroyItem() {
        //GameObject[] dialogueObjects = GameObject.FindGameObjectsWithTag("ItemConfirmationObjects");
        //foreach(GameObject dialogueObj in dialogueObjects)
        //{
        //    Debug.Log("Deactivating: " + dialogueObj.name);
        //    dialogueObj.SetActive(false);
        //}
        confirmDropDialogue.SetActive(false);
        _GameManager.carryingItem.GetComponent<Item_r>().character.GetComponent<Character_r>().AddTrust(_GameManager.carryingItem.GetComponent<Item_r>().trustDropMod);
        Destroy(_GameManager.carryingItem.GetComponent<Image>());
        Destroy(_GameManager.carryingItem.gameObject);
        _GameManager.carryingItem = null;
    }

    public void ReturnItem() {
        //GameObject[] dialogueObjects = GameObject.FindGameObjectsWithTag("ItemConfirmationObjects");
        //foreach(GameObject dialogueObj in dialogueObjects)
        //{
        //    Debug.Log("Deactivating: " + dialogueObj.name);
        //    dialogueObj.SetActive(false);
        //}
        confirmDropDialogue.SetActive(false);
        _GameManager.carryingItem.GetComponent<Item_r>().slot.GetComponent<InventorySlot_r>().item = _GameManager.carryingItem;
        _GameManager.carryingItem.GetComponent<Item_r>().slot.GetComponent<Image>().sprite = _GameManager.carryingItem.GetComponent<Item_r>().itemSprite;
        if (_GameManager.carryingItem.GetComponent<Image>()) {
            Destroy(_GameManager.carryingItem.GetComponent<Image>());
            _GameManager.carryingItem.gameObject.SetActive(false);
        }
        _GameManager.carryingItem = null;
    }
}
