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
        if (item)
            GetComponent<Image>().sprite = item.GetComponent<Item_r>().itemSprite;
	}

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData) {
        _UIController.OnClickInventory(gameObject);
        lastClick = Time.time;
    }

    void IPointerUpHandler.OnPointerUp(PointerEventData eventData) {
        if (Time.time - lastClick > 0.2f) {
            _UIController.OnClickInventory(gameObject);
            lastClick = Time.time;
        }
    }

    void IDragHandler.OnDrag(PointerEventData eventData) {
        if (Time.time - lastClick > 0.2f) {
            _UIController.OnClickInventory(gameObject);
        }
        lastClick = Time.time;
    }

    void IDropHandler.OnDrop(PointerEventData eventData) {
        if (Time.time - lastClick > 0.2f) {
            _UIController.OnClickInventory(gameObject);
            lastClick = Time.time;
        }
    }

}
