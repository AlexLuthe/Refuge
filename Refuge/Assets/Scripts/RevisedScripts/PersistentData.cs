using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

struct CharPacket {
    public float health;
    public float hunger;
    public float thirst;
    public int[] inventory;
    public bool injured;
    public bool cholera;
    public bool dysentery;
    public bool typhoid;
}

struct DataPacket {
    public string fungusBlock;
    public int currentScreen; // ID
    public int currentArea; // Map ID
    public int currentLoc; // Location ID within Map
    public int totalLoc; // Total number of locations
    //public List<List<int>> locationInventory;
    public List<int[]> locationInventory;
    //public int[][] locationInventory;
    public CharPacket[] chars;
    public int money;
}

public class PersistentData : MonoBehaviour {

    void SaveCSV (DataPacket data) {
        string unwrittenData;
        unwrittenData = string.Format("{0},{1},{2},{3},{4},", data.fungusBlock, data.currentScreen.ToString(), data.currentArea.ToString(), data.currentLoc.ToString(), data.totalLoc.ToString());
        // Location Inventories
        for (int i = 0; i < data.totalLoc; ++i) {
            for (int a = 0; a < 4; ++a) {
                unwrittenData += data.locationInventory[i][a] + ",";
            }
        }
        // Characters
        for (int i = 0; i < 4; ++i) {
            unwrittenData += string.Format("{0},{1},{2},", data.chars[i].health.ToString(), data.chars[i].hunger.ToString(), data.chars[i].thirst.ToString());
            for (int a = 0; a < 4; ++a) {
                unwrittenData += data.chars[i].inventory[a].ToString() + ",";
            }
            unwrittenData += string.Format("{0},{1},{2},{3},", data.chars[i].injured.ToString(), data.chars[i].cholera.ToString(), data.chars[i].dysentery.ToString(), data.chars[i].typhoid.ToString());
        }
        unwrittenData += data.money.ToString() + ",";
        Debug.Log("Saving...");
        TextWriter sw = new StreamWriter(Application.dataPath + "/Saves/sav.csv");
        sw.WriteLine(unwrittenData);
        sw.Close();
        //TextWriter sw = new StreamWriter("");
        //string thing1 = "_test_";
        //float thing2 = 5.52f;
        //sw.WriteLine("{0},{1}", thing1, thing2.ToString());
    }

    DataPacket LoadCSV () {
        DataPacket data = new DataPacket();
        string[] passedData;
        StreamReader sr = new StreamReader(Application.dataPath + "/Saves/sav.csv");
        passedData = sr.ReadToEnd().Split(',');
        data.fungusBlock = passedData[0];
        data.currentScreen = int.Parse(passedData[1]);
        data.currentArea = int.Parse(passedData[2]);
        data.currentLoc = int.Parse(passedData[3]);
        data.totalLoc = int.Parse(passedData[4]);
        int currentIndex = 5;
        // Location Inventories
        data.locationInventory = new List<int[]>();
        for (int i = 0; i < data.totalLoc; ++i) {
            data.locationInventory.Add(new int[4]);
            for (int a = 0; a < 4; ++a) {
                data.locationInventory[i][a] = int.Parse(passedData[currentIndex]);
                ++currentIndex;
            }
        }
        data.chars = new CharPacket[4];
        for (int i = 0; i < 4; ++i) {
            data.chars[i].health = float.Parse(passedData[currentIndex]);
            data.chars[i].hunger = float.Parse(passedData[currentIndex + 1]);
            data.chars[i].thirst = float.Parse(passedData[currentIndex + 2]);
            currentIndex += 3;
            data.chars[i].inventory = new int[4];
            for (int a = 0; a < 4; ++a) {
                data.chars[i].inventory[a] = int.Parse(passedData[currentIndex]);
                ++currentIndex;
            }
            data.chars[i].injured = bool.Parse(passedData[currentIndex]);
            data.chars[i].cholera = bool.Parse(passedData[currentIndex + 1]);
            data.chars[i].dysentery = bool.Parse(passedData[currentIndex + 2]);
            data.chars[i].typhoid = bool.Parse(passedData[currentIndex + 3]);
            currentIndex += 4;
        }
        data.money = int.Parse(passedData[currentIndex]);
        sr.Close();
        return data;
    }

    string EchoDataPacket(DataPacket data) {
        string dataString = "";
        dataString = string.Format("{0},{1},{2},{3},{4},", data.fungusBlock, data.currentScreen.ToString(), data.currentArea.ToString(), data.currentLoc.ToString(), data.totalLoc.ToString());
        // Location Inventories
        for (int i = 0; i < data.totalLoc; ++i) {
            for (int a = 0; a < 4; ++a) {
                dataString += data.locationInventory[i][a] + ",";
            }
        }
        // Characters
        for (int i = 0; i < 4; ++i) {
            dataString += string.Format("{0},{1},{2},", data.chars[i].health.ToString(), data.chars[i].hunger.ToString(), data.chars[i].thirst.ToString());
            for (int a = 0; a < 4; ++a) {
                dataString += data.chars[i].inventory[a].ToString() + ",";
            }
            dataString += string.Format("{0},{1},{2},{3},", data.chars[i].injured.ToString(), data.chars[i].cholera.ToString(), data.chars[i].dysentery.ToString(), data.chars[i].typhoid.ToString());
        }
        dataString += data.money.ToString() + ",";
        Debug.Log(dataString);
        return dataString;
    }

    void ApplyPacket(DataPacket data) {
        GameManager_r GM = FindObjectOfType<GameManager_r>();
        ExecuteFungusBlock ex = new ExecuteFungusBlock();
        ex.fungusBlockToExecute = data.fungusBlock;
        GM.ChangeScreen(data.currentScreen);
    }

	// Use this for initialization
	void Start () {
        // Create Dummy Packet to test saving and loading
		DataPacket initData = new DataPacket();
        initData.fungusBlock = "1-5-7";
        initData.currentScreen = 5;
        initData.currentLoc = 3;
        initData.totalLoc = 2;
        initData.locationInventory = new List<int[]>();
        for (int i = 0; i < 2; ++i) {
            initData.locationInventory.Add(new int[4]);
            for (int a = 0; a < 4; ++a) {
                initData.locationInventory[i][a] = i + a; // This line breaks
            }
        }

        initData.chars = new CharPacket[4];
        for (int i = 0; i < 4; ++i) {
            initData.chars[i].health = 0.25f * i;
            initData.chars[i].hunger = 0.5f;
            initData.chars[i].thirst = 0.15f * i;
            initData.chars[i].inventory = new int[4];
            for (int a = 0; a < 4; ++a) {
                initData.chars[i].inventory[a] = 2 + a + i;
            }
            initData.chars[i].injured = true;
            initData.chars[i].cholera = false;
            initData.chars[i].dysentery = false;
            initData.chars[i].typhoid = true;
        }
        initData.money = 5;
        Debug.Log("Built a data");
        SaveCSV(initData);
        Debug.Log("Saved a data");
        DataPacket loadedData = LoadCSV();
        Debug.Log("Loaded a data");
        EchoDataPacket(initData);
        string str = EchoDataPacket(loadedData);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
