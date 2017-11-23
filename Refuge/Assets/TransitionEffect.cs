using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionEffect : MonoBehaviour {
    public float transitionTime;
    public float currentTransitonTime = int.MaxValue;

    public GameObject halfTransition;
    public GameObject fullTransition;

    public bool transitioningForward = false;
    public bool transitioningBackward = false;

    public bool halfTransitioned = false;
    public bool fullTransitioned = false;

    public static TransitionEffect instance;

	// Use this for initialization
	void Start ()
    {
        //singleton setup
        instance = this;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Q))
            StartTransition();

        if (transitioningForward)
        {
            // go half way
            if (halfTransitioned == false)
            {
                if (fullTransitioned == false)
                {

                    if (Time.timeSinceLevelLoad >= currentTransitonTime)
                    {
                        halfTransitioned = true;
                        halfTransition.SetActive(true);
                        currentTransitonTime = Time.timeSinceLevelLoad + (transitionTime * 0.5f);
                    }
                    else if (Time.timeSinceLevelLoad <= currentTransitonTime)
                        currentTransitonTime = Time.timeSinceLevelLoad + (transitionTime * 0.5f);
                }
            }
            // go the full way
            else if (halfTransitioned == true)
            {
                if (fullTransitioned == false)
                {
                    if (Time.timeSinceLevelLoad >= currentTransitonTime)
                    {
                        fullTransitioned = true;
                        fullTransition.SetActive(true);
                        transitioningForward = false;
                        transitioningBackward = true;
                        currentTransitonTime = Time.timeSinceLevelLoad + (transitionTime * 0.5f);
                    }
                }
                else if (Time.timeSinceLevelLoad <= currentTransitonTime)
                    currentTransitonTime = Time.timeSinceLevelLoad + (transitionTime * 0.5f);
            }
        }
        else if (transitioningBackward)
        {
            // go half way
            if (fullTransitioned == true)
            {
                if (Time.timeSinceLevelLoad >= currentTransitonTime)
                {
                    fullTransitioned = false;
                    fullTransition.SetActive(false);
                    currentTransitonTime = Time.timeSinceLevelLoad + (transitionTime * 0.5f);
                }
                //else if (Time.timeSinceLevelLoad <= currentTransitonTime)
                    //currentTransitonTime = Time.timeSinceLevelLoad + (transitionTime * 0.5f);
            }
            // go the full way
            else if (halfTransitioned == true)
            { 
            if (Time.timeSinceLevelLoad >= currentTransitonTime)
            {
                halfTransitioned = false;
                halfTransition.SetActive(false);
                transitioningBackward = false;
            }
            //else if (Time.timeSinceLevelLoad <= currentTransitonTime)
                //currentTransitonTime = Time.timeSinceLevelLoad + (transitionTime * 0.5f);
            }
        }
	}

    public void StartTransition()
    {
        transitioningForward = true;
        Debug.Log("StartingTransition");
    }
}
