using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_r : MonoBehaviour {

    public enum ItemType {
        ITCannedBeans = 0,
        ITDriedFruit = 1,
        ITMealRation = 2,
        ITPureWater = 3,
        ITDirtyWater = 4,
        ITHandheldPuzzle = 5,
        ITNovel = 6,
        ITDessert = 7,
        ITFirstAid = 8,
        ITMedicine = 9,
        
        ITInteravenousFluids = 10,
        ITMetronidazole = 11,
        ITPenicilin = 12,
        ITRepairFracture = 13,
        ITCureAll = 14,
        ITHeal = 15,
    };

    public ItemType itemType;
    public Sprite itemSprite;
    public float healthMod, hungerMod, thirstMod;
    public float trustGiveMod, trustDropMod;
    public float injuryChance, choleraChance, dysenteryChance, typhoidChance;
    public bool cureDiseases;
    public bool encounterOnGive, encounterOnTake, encounterOnDrop;
    public string encounterToExecute;
    public int price;
    public float spawnChance;
    public string shopHoverTip;
    public string invHoverTip;

    public GameObject character;
    public GameObject slot;
}
