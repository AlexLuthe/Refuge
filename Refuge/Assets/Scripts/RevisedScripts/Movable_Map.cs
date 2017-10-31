using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Movable_Map : MonoBehaviour, IPointerClickHandler {

    public GameObject refugeeObj;
    public Vector2 newLocation;
    public GameManager_r GM;
    public Location_r location;
    public AudioClip clip;
    public string channel;
    bool arrived = false;

	// Use this for initialization
	void Start () {
		GM = GameObject.Find("GameManager").GetComponent<GameManager_r>();
        newLocation = refugeeObj.transform.position;
        if (clip) {
            GM._AudioManager.PlayClip(clip, GM._AudioManager.GetChannel(channel), loop: true);
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (!arrived) {
        Vector2 refTrans = refugeeObj.transform.position;
        refugeeObj.transform.position = Vector2.Lerp(refTrans, newLocation, Time.deltaTime * GM.partySpeed);
        Debug.DrawLine(refugeeObj.transform.position, newLocation, Color.red);
		if (Vector2.Distance(refugeeObj.transform.position, newLocation) < 1 && location) {
            location.Scavenge();
                arrived = true;
        }
        }
	}

    void IPointerClickHandler.OnPointerClick(PointerEventData eventData) {
        newLocation = eventData.pressPosition;
        arrived = false;
        Debug.Log("Tryna do a thang");
        Debug.Log(refugeeObj.transform.position);
    }


}
