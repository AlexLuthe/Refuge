using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Travel : MonoBehaviour {

    public GameObject backGround;
    public GameObject[] backGroundObjs;
    public GameObject[] foreGroundObjs;
    public GameObject[] characters;
    public float partySpeed = 1;
    public float timer = 3;
    public int screenToSwitch;
    public int hubToSwitch;
    GameManager_r GM;
    bool camped = false;

    public Vector3 startingCoord;
    public Vector3 endCoord;

    public float completionPercent;
    public float completionRate;
	// Use this for initialization
	void Start () {
        GM = GameObject.Find("GameManager").GetComponent<GameManager_r>();
        if (!GM)
            Debug.Log("GM is gone");
	}
	
	// Update is called once per frame
	void Update () {
        /*backGround.transform.Translate(new Vector3(0.15f * partySpeed, 0));
        foreach (GameObject backGround in backGroundObjs) {
        backGround.transform.Translate(new Vector3(0.5f * partySpeed, 0));
        if (backGround.transform.position.x > Screen.width + backGround.GetComponent<RectTransform>().rect.width / 4)
            backGround.transform.position = new Vector3(0 - backGround.GetComponent<RectTransform>().rect.width / 4, backGround.transform.position.y);
        }*/

        // Move the background according to the coordinates and speeds
            // Setup %
        completionPercent += completionRate / Time.deltaTime;
        if(completionPercent < 1)
            backGround.transform.position = new Vector3(Mathf.Lerp(startingCoord.x,endCoord.x, completionPercent), Mathf.Lerp(startingCoord.y,endCoord.y, completionPercent), 0);

        foreach (GameObject foreGround in foreGroundObjs) {
            foreGround.transform.Translate(new Vector3(1 * partySpeed, 0));
            if (foreGround.transform.position.x > Screen.width + foreGround.GetComponent<RectTransform>().rect.width / 4)
                foreGround.transform.position = new Vector3(0 - foreGround.GetComponent<RectTransform>().rect.width / 4, foreGround.transform.position.y);
        }
        if (timer <= 0) {
            //Debug.Log(screenToSwitch); // 0
            if (camped) {
                camped = false;
                if (screenToSwitch == 1) {
                    //Debug.Log("Switching to hub because screenToSwitch == " + screenToSwitch);
                    GM.SwitchToHub(hubToSwitch); // This line is exectuted...
                }
                else
                    GM.ChangeScreen(screenToSwitch);
            }
            else {
                camped = true;
                GM.ChangeScreen(15);
            }
        }
        timer -= Time.deltaTime;
	}

    private void OnEnable() {
        if (!GM)
            GM = GameObject.Find("GameManager").GetComponent<GameManager_r>();
        for (int i = 0; i < GM.characters.Length; ++i) {
            if (GM.characters[i].GetComponent<Character_r>().isDead)
                characters[i].SetActive(false);
        }
        if (timer <= 0)
            timer = 3;
    }
}
