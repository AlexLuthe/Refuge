using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class HoverTip_r : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    GameManager_r GM;
    [SerializeField]
    string hoverTip;
    [SerializeField]
    float xOffset = 150;
    [SerializeField]
    float yOffset = -100;
    bool isOver;

    void Start() {
        GM = GameObject.Find("GameManager").GetComponent<GameManager_r>();
    }

    void Update() {
        if (isOver) {
            Vector3 newPos = new Vector3(Input.mousePosition.x + xOffset, Input.mousePosition.y + yOffset, 1);
            GM.mouseHoverTip.transform.position = newPos;
        }
    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData) {
        isOver = true;
        GM.mouseHoverTip.SetActive(true);
        GM.mouseHoverTip.GetComponentInChildren<Text>().text = hoverTip;

        //if (GM.currentScreen == GameManager_r.ScreenType.STHubMap)
        //    GM.mouseHoverTip.GetComponentInChildren<Text>().color = Color.black;
        //else
        GM.mouseHoverTip.GetComponentInChildren<Text>().color = Color.white;

        if (GetComponent<InventorySlot_r>() && GetComponent<InventorySlot_r>().item) {
            if (GetComponent<InventorySlot_r>().item.GetComponent<Item_r>().price == 0)
                hoverTip = GetComponent<InventorySlot_r>().item.GetComponent<Item_r>().invHoverTip;
            else
                hoverTip = GetComponent<InventorySlot_r>().item.GetComponent<Item_r>().shopHoverTip;
        GM.mouseHoverTip.SetActive(true);
        GM.mouseHoverTip.GetComponentInChildren<Text>().text = hoverTip;
        }
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData) {
        GM.mouseHoverTip.SetActive(false);
        isOver = false;
    }
}
