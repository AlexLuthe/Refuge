using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Clinic_r : Location_r, IPointerClickHandler {

    //void IPointerClickHandler.OnPointerClick(PointerEventData eventData) {
    //    Debug.Log("The user clicked");
    //    _AudioManager.PlayClip(_AudioManager.clickSound, _AudioManager.GetChannel("SFX"));

    //    if (locationNumber == map.currentLocationNumber - 1 || locationNumber == map.currentLocationNumber + 1)
    //    {
    //        map.Travel(gameObject);
    //    }
    //}

    public override void GenerateInventory() {
        if (!generated) {
            generated = true;
            for (int i = 0; i < inventory.Length; ++i) {
                if (i < inventory.Length / 2) {
                    inventory[i].GetComponent<InventorySlot_r>().item = possibleItems[0];
                    inventory[i].GetComponent<Image>().sprite = possibleItems[0].GetComponent<Item_r>().itemSprite;
                }
                else {
                    inventory[i].GetComponent<InventorySlot_r>().item = possibleItems[1];
                    inventory[i].GetComponent<Image>().sprite = possibleItems[1].GetComponent<Item_r>().itemSprite;
                }
            }
        }
        if (changeScreen) {
            GameObject.Find("GameManager").GetComponent<GameManager_r>().ChangeScreen(destinationScreen);
            GameObject.Find("GameManager").GetComponent<GameManager_r>().conditionReportText.text = "";
        }
    }

}
