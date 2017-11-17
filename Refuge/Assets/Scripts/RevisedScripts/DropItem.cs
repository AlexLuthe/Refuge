using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DropItem : MonoBehaviour, IPointerUpHandler {

    GameManager_r _GameManager;

	// Use this for initialization
	void Start () {
		_GameManager = GameObject.Find("GameManager").GetComponent<GameManager_r>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void IPointerUpHandler.OnPointerUp(PointerEventData eventData) {
        Debug.Log("Drop Called");
        if (_GameManager.carryingItem) {
            if (_GameManager.carryingItem.GetComponent<Image>()) {
                Destroy(_GameManager.carryingItem.GetComponent<Image>());
                _GameManager.carryingItem.gameObject.SetActive(false);
                //Destroy(_GameManager.carryingItem.gameObject);
            }
        }
    }
}
