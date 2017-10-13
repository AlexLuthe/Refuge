using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PromptMenu : MonoBehaviour {
	public Text headerContent;
	public Text bodyContent;

	public bool PromptToggle;

	public GameObject Prompt;

	public static PromptMenu instance;

	public void Awake()
	{
		instance = this;
	}
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

	public void DisplayPrompt(string header, string body)
	{
		Debug.Log("Trying to display the Prompt");
		// Set the text and activate the promot
		headerContent.text = header;
		bodyContent.text = body;

		PromptToggle = true;

		TogglePrompt();
	}

	public void TogglePrompt()
	{
		if(PromptToggle)
			Prompt.SetActive(false);
		else
			Prompt.SetActive(true);
	}
}
