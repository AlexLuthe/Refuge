using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Movable_Map : MonoBehaviour, IPointerClickHandler {

    public GameObject refugeeObj;
    public Vector2 newLocation;
    public GameManager_r GM;
    public Location_r location;

	// Use this for initialization
	void Start () {
		GM = GameObject.Find("GameManager").GetComponent<GameManager_r>();
        newLocation = refugeeObj.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        refugeeObj.transform.position = Vector2.Lerp(refugeeObj.transform.position, newLocation, Time.deltaTime * GM.partySpeed);
		if (Vector2.Distance(refugeeObj.transform.position, newLocation) < 1 && location) {
            location.Scavenge();
        }
	}

    void IPointerClickHandler.OnPointerClick(PointerEventData eventData) {
        newLocation = eventData.pressPosition;
        Debug.Log("Tryna do a thang");
        Debug.Log(refugeeObj.transform.position);
    }
}
