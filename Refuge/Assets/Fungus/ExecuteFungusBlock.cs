using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

public class ExecuteFungusBlock : MonoBehaviour {
    private Flowchart flowchartInScene;
    public GameObject flowchartGameObject;


    public string fungusBlockToExecute;

	// Use this for initialization
	void Start () {
        flowchartInScene = FindObjectOfType<Flowchart>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnClick()
    {
        flowchartGameObject.SetActive(true);
        flowchartInScene.ExecuteBlock(fungusBlockToExecute);

        //GameManager.Instance.ChangeScreen()
    }
}
