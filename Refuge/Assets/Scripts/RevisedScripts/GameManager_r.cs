using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager_r : MonoBehaviour {
    
    public enum ScreenType {
        STWorldMap = 0,
        STHubMap = 1,
        STEncounter = 2,
        STLocation = 3,
        STMarket = 4,
        STPause = 5,
        STOptions = 6,
        STCredits = 7,
        STClinic = 8,
        STEnMap1 = 9, // Heshem Night
        STEnMap2 = 10, // Heshem Day
        STEnMap3 = 11, // World Night
        STDebug = 12,
        STScavenge = 13,
        STTravel = 14,
    };

    public GameObject charUI;
    public GameObject[] characters;
    public GameObject mouseHoverTip;
    public GameObject carryingItem;
    public GameObject moneyGUI;
    public Text conditionReportText;
    public float reportActiveTime = 1f;
    public ScreenType currentScreen, prevScreen;
    Dictionary<ScreenType, GameObject> screens = new Dictionary<ScreenType, GameObject>();
    public List<GameObject> hubs = new List<GameObject>();
    GameObject currentHub;

    public int partyMoney;
    public float partySpeed = 1f;

    float hoverTimer = 0;
    public bool inCoRoutine = false;
    public AudioManager _AudioManager;
    bool musicStarted = false;

    public GameObject endGameCanvas;
    public GameObject gameCanvas;

    // Singleton
    private static GameManager_r _Instance;
    public static GameManager_r Instance {
        get {
            if (_Instance == null)
                _Instance = new GameManager_r();
            return _Instance;
        }
    }

    public IEnumerator HasGottenHealthCondition()
    {
        
        //inCoRoutine = true;
        conditionReportText.gameObject.SetActive(true);
        yield return new WaitForSeconds(reportActiveTime);
        conditionReportText.text = "";
        conditionReportText.gameObject.SetActive(false);
        //inCoRoutine = false;

        StopCoroutine(HasGottenHealthCondition());
        
    }

    // Use this for initialization
    void Start () {
        // Find each screen object
        screens.Add(ScreenType.STHubMap, GameObject.FindGameObjectWithTag("ScreenHubMap")); // Char UI
        screens.Add(ScreenType.STWorldMap, GameObject.FindGameObjectWithTag("ScreenWorldMap")); // Char UI
        screens.Add(ScreenType.STEncounter, GameObject.FindGameObjectWithTag("ScreenEncounter"));
        screens.Add(ScreenType.STLocation, GameObject.FindGameObjectWithTag("ScreenLocation"));
        screens.Add(ScreenType.STMarket, GameObject.FindGameObjectWithTag("ScreenMarket"));
        screens.Add(ScreenType.STPause, GameObject.FindGameObjectWithTag("ScreenPause"));
        screens.Add(ScreenType.STOptions, GameObject.FindGameObjectWithTag("ScreenOptions"));
        screens.Add(ScreenType.STCredits, GameObject.FindGameObjectWithTag("ScreenCredits"));
        screens.Add(ScreenType.STClinic, GameObject.FindGameObjectWithTag("ScreenClinic"));
        screens.Add(ScreenType.STEnMap1, GameObject.FindGameObjectWithTag("ScreenHeshemNight"));
        screens.Add(ScreenType.STEnMap2, GameObject.FindGameObjectWithTag("ScreenHeshemDay"));
        screens.Add(ScreenType.STEnMap3, GameObject.FindGameObjectWithTag("ScreenWorldNight"));
        screens.Add(ScreenType.STDebug, GameObject.FindGameObjectWithTag("ScreenDebug"));
        screens.Add(ScreenType.STScavenge, GameObject.FindGameObjectWithTag("ScreenScavenge"));
        screens.Add(ScreenType.STTravel, GameObject.FindGameObjectWithTag("ScreenTravel"));
        currentHub = hubs[0];

        if (!screens[ScreenType.STEnMap3])
            Debug.Log("World Night Map has gone walkabouts");
        
        for (int index = 0; index < screens.Count; ++index) 
            if (screens[(ScreenType)index])
                screens[(ScreenType)index].SetActive(false);
        ChangeScreen(ScreenType.STPause);
        foreach (GameObject chara in characters) {
            chara.GetComponent<Character_r>().AddHealth(1);
            chara.GetComponent<Character_r>().AddHunger(1);
            chara.GetComponent<Character_r>().AddThirst(1);
        }

        _AudioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
    }

    void Update() {
        if (carryingItem) {
                if (!carryingItem.GetComponent<Image>()) {
                    carryingItem.AddComponent<Image>();
                    carryingItem.GetComponent<Image>().sprite = carryingItem.GetComponent<Item_r>().itemSprite;
                    carryingItem.transform.SetParent(GameObject.Find("Canvas").transform);
                    carryingItem.gameObject.transform.SetSiblingIndex(carryingItem.gameObject.transform.GetSiblingIndex());
                    carryingItem.GetComponent<Image>().raycastTarget = false;
                }
            carryingItem.transform.position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, -5f);
            carryingItem.SetActive(true);
            Vector3 newPos = carryingItem.transform.position;
            newPos.z = -5f;
            carryingItem.transform.position = newPos;
        }
        if (!musicStarted) {
            _AudioManager.PlayClip(_AudioManager.BGM, _AudioManager.GetChannel("Music"), 1, true);
            musicStarted = true;
        }
    }

    public void ChangeScreen(ScreenType newScreen) {
        //if (_AudioManager)
           // _AudioManager.PlayClip(_AudioManager.clickSound, _AudioManager.GetChannel("SFX"));
        Destroy(carryingItem);

        prevScreen = currentScreen;
        screens[currentScreen].SetActive(false);
        screens[newScreen].SetActive(true);
        currentScreen = newScreen;
        mouseHoverTip.SetActive(false);

        // UI Requirements
        if (charUI)
        if (currentScreen == ScreenType.STHubMap || currentScreen == ScreenType.STWorldMap || currentScreen == ScreenType.STClinic || currentScreen == ScreenType.STMarket || currentScreen == ScreenType.STEnMap1 || currentScreen == ScreenType.STEnMap2 || currentScreen == ScreenType.STEnMap3)
            charUI.SetActive(true);
        else
            charUI.SetActive(false);
        if (newScreen == ScreenType.STEnMap1 || newScreen == ScreenType.STEnMap2 || newScreen == ScreenType.STEnMap3)
            screens[ScreenType.STEncounter].SetActive(true);

    }

    public void ChangeScreen(int iNewScreen) {
        if (_AudioManager && _AudioManager.GetChannel("SFX") != null && _AudioManager.clickSound)
            _AudioManager.PlayClip(_AudioManager.clickSound, _AudioManager.GetChannel("SFX"));
        Destroy(carryingItem);

        ScreenType newScreen = (ScreenType)iNewScreen;
        prevScreen = currentScreen;
        screens[currentScreen].SetActive(false);
        screens[newScreen].SetActive(true);
        currentScreen = newScreen;
		mouseHoverTip.SetActive(false);
        // UI Requirements
        if (charUI)
            if (currentScreen == ScreenType.STHubMap || currentScreen == ScreenType.STWorldMap || currentScreen == ScreenType.STClinic || currentScreen == ScreenType.STMarket || currentScreen == ScreenType.STEnMap1 || currentScreen == ScreenType.STEnMap2 || currentScreen == ScreenType.STEnMap3)
                charUI.SetActive(true);
            else
                charUI.SetActive(false);
        if (iNewScreen == 9 || iNewScreen == 10 || iNewScreen == 11)
            screens[ScreenType.STEncounter].SetActive(true);
    }

    public void ChangeToPrevScreen() {
        ChangeScreen(prevScreen);
    }

    public void SwitchToHub(int HubIndex) {
        ChangeScreen(ScreenType.STHubMap);
        foreach (GameObject hub in hubs) {
            hub.SetActive(false);
        }
        currentHub = hubs[HubIndex];
        currentHub.SetActive(true);
    }

    public GameObject WealthiestChar(Item_r.ItemType itemType) {
        GameObject chara = new GameObject();
        int maxItemCount = 0;
        foreach (GameObject character in characters) {
            int itemCount = 0;
            foreach (GameObject item in character.GetComponent<Character_r>().inventory) {
                if (item.GetComponent<Item_r>().itemType == itemType)
                    ++itemCount;
            }
            if (itemCount > maxItemCount) {
                maxItemCount = itemCount;
                chara = character;
            }
        }
        return chara;
    }

    public void AddMoney(int modifier) { partyMoney += modifier; if (moneyGUI) moneyGUI.GetComponent<Text>().text = "Money: " + partyMoney; }
    public int GetMoney() { return partyMoney; }
}
