using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlot_r : MonoBehaviour, IPointerUpHandler, IPointerDownHandler, IDragHandler, IDropHandler {

    public GameObject item;
    float lastClick;
    GameManager_r _GameManager;
    UIController_r _UIController;

	// Use this for initialization
	void Start () {
		_GameManager = GameObject.Find("GameManager").GetComponent<GameManager_r>();
        _UIController = GameObject.Find("UIController").GetComponent<UIController_r>();
        if (item) {
            GetComponent<Image>().sprite = item.GetComponent<Item_r>().itemSprite;
            item.GetComponent<Item_r>().character = transform.parent.gameObject;
            item.GetComponent<Item_r>().slot = gameObject;
            //if (item.GetComponent<Item_r>().character.GetComponent<Character_r>()) {
            //    item.GetComponent<Item_r>().price = 0;
            //    Debug.Log("Parent: " + transform.parent);
            //}
        }
	}

    void Update() {
        if (_GameManager.inventoryActive) {
            Color colour = GetComponent<Image>().color;
            colour.a = 1f;
            GetComponent<Image>().color = colour;
        }
        else {
            Color colour = GetComponent<Image>().color;
            colour.a = 0.5f;
            GetComponent<Image>().color = colour;
        }
    }

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData) {
        if (_GameManager.inventoryActive) {
            _UIController.OnClickInventory(gameObject);
            lastClick = Time.time;
        }
    }

    void IPointerUpHandler.OnPointerUp(PointerEventData eventData) {
        if (_GameManager.inventoryActive)
        if (Time.time - lastClick > 0.2f) {
            _UIController.OnClickInventory(gameObject);
            lastClick = Time.time;
        }
    }

    void IDragHandler.OnDrag(PointerEventData eventData) {
        if (_GameManager.inventoryActive) {
        if (Time.time - lastClick > 0.2f) {
            _UIController.OnClickInventory(gameObject);
        }
        lastClick = Time.time;
        }
    }

    void IDropHandler.OnDrop(PointerEventData eventData) {
        if (_GameManager.inventoryActive) {
        if (Time.time - lastClick > 0.2f) {
            _UIController.OnClickInventory(gameObject);
            lastClick = Time.time;
        }
        }
    }

}
