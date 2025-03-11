using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class demoTrials : MonoBehaviour
{
    private float pausetimer = 0.0f;
    private int allowedYes = 3;
    private int allowedNo = 7;
    private int randomFirstStim = 0; 
    private int randomSecondStim = 0;
    private int prevSecondStim = 0; 
    private int stimCounter = 0;
    private int totalStim = 10;
    private bool wasPrevYes = false; 
    private bool activatePause = false;
    private bool stimulifound = false;
    private bool showFirstStim = true; 
    private bool showSecondStim = false;

    public bool allStimGiven = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if (activatePause == false && stimulifound == false && stimCounter != totalStim)
        {
            (randomFirstStim,randomSecondStim,stimulifound,allowedYes,allowedNo,prevSecondStim,wasPrevYes) = 
                GetComponent<NBackStimMotor>().FindingTheNextTwoStimuli
                (stimulifound,allowedYes,stimCounter,totalStim,wasPrevYes,prevSecondStim,allowedNo);
        }

        if (stimulifound && activatePause == false && showFirstStim)
        {

            if (showFirstStim)
            {
                GetComponent<NBackStimMotor>().showStimuli(randomFirstStim, true);
                if (Input.anyKeyDown)
                {
                    activatePause = true;
                    showSecondStim = true;
                    showFirstStim = false;
                }
            }
        }
        else if (stimulifound && activatePause == false && showSecondStim)
        {
            if (showSecondStim)
            {
                GetComponent<NBackStimMotor>().showStimuli(randomSecondStim, true);
                if (Input.anyKeyDown)
                {
                    activatePause = true;
                    showSecondStim = false;
                    showFirstStim = true;
                    stimulifound = false;
                    stimCounter++;
                }
            }
        }
        if (activatePause)
        {
            GetComponent<NBackStimMotor>().HidAllStimulus();

            pausetimer += Time.fixedDeltaTime;
            
            if (pausetimer > 3.0)
            {
                activatePause = false;
                pausetimer = 0;
            }
        } 
        else if (stimCounter == totalStim && activatePause == false)
        {
            allStimGiven = true;
        }
    }
}
