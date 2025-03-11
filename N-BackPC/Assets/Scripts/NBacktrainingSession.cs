using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class NBacktrainingSession : MonoBehaviour
{
    private float timer = 0.0f;
    private float grandClock = 0.0f;
    private float pausetimer = 0.0f;
    private int allowedYes = 6;
    private int allowedNo = 14;
    private int randomFirstStim = 0;
    private int randomSecondStim = 0;
    private int prevSecondStim = 0;
    private int stimCounter = 0;
    private int totalStim = 20;
    private bool wasPrevYes = false;
    private bool activatePause = false;
    private bool stimulifound = false;
    private bool showFirstStim = true;
    private bool showSecondStim = false;
    private string subjectId = "";
    private bool sendfeedback = true;
    private bool recordReaction = true; 

    public bool allStimGiven = false;
    public int blockNo = 0; 

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.fixedDeltaTime;
        grandClock += Time.fixedDeltaTime;

        if (activatePause == false && stimulifound == false && stimCounter != totalStim)
        {
            (randomFirstStim, randomSecondStim, stimulifound, allowedYes, allowedNo, prevSecondStim, wasPrevYes) =
                GetComponent<NBackStimMotor>().FindingTheNextTwoStimuli
                (stimulifound, allowedYes, stimCounter, totalStim, wasPrevYes, prevSecondStim, allowedNo);
        }

        if (timer < 0.5f && stimulifound && activatePause == false && showFirstStim)
        {
            GetComponent<NBackStimMotor>().showStimuli(randomFirstStim, true);
            activatePause = true;
            showSecondStim = true;
            showFirstStim = false;
        }
        else if (timer < 0.5f && stimulifound && activatePause == false && showSecondStim)
        {
            GetComponent<NBackStimMotor>().showStimuli(randomSecondStim, true);
            activatePause = true;
            showSecondStim = false;
            showFirstStim = true;
            stimulifound = false;
            stimCounter++;
        }

        if (timer > 0.5f && timer < 3.5f && activatePause)
        {
            GetComponent<NBackStimMotor>().HidAllStimulus();
        }

        if (timer < 3.5f && stimulifound == false && showSecondStim == false && recordReaction)
        {
            if (blockNo == 1 || blockNo == 0)
            {
                var reactiongiven = GetComponent<NBackReactionCapture>().RecordReaction(randomFirstStim, randomSecondStim, timer);
                if (reactiongiven)
                {
                    recordReaction = false;
                }
            }
            if (blockNo == 2)
            {
                var reactiongiven = GetComponent<NBackReactionCaptureBlock2>().RecordReaction(randomFirstStim, randomSecondStim, timer);
                if (reactiongiven)
                {
                    recordReaction = false;
                }
            }
        }

        if (timer > 3.5f && activatePause && stimCounter != totalStim)
        {
            activatePause = false;
            timer = 0.0f; 
            recordReaction = true;
        }
        else if (timer > 3.5f && activatePause && stimCounter == totalStim)
        {
            activatePause = false;
            timer = 4.0f;
        }

        else if (stimCounter == totalStim && timer > 3.9f && sendfeedback)
        {
            if (blockNo == 1 || blockNo == 0)
            {
                GetComponent<NBackReactionCapture>().CalculateReactionMatrics(totalStim);
                GetComponent<NBackReactionCapture>().SaveAndPublishReactions(subjectId, blockNo);
                allStimGiven = true;
                sendfeedback = false;
            }
            if (blockNo == 2)
            {
                GetComponent<NBackReactionCaptureBlock2>().CalculateReactionMatrics(totalStim);
                GetComponent<NBackReactionCaptureBlock2>().SaveAndPublishReactions(subjectId, blockNo);
                allStimGiven = true;
                sendfeedback = false;
            }

        }
    }
    public void setSubjectID(string subId)
    {
        subjectId = subId;
    }

    public void ResetTraining()
    {
        stimCounter = 0;
        allowedNo = 14;
        allowedYes = 6;
        stimulifound = false;
        sendfeedback = true;
        showFirstStim = true;
        showSecondStim = false;
        timer = 0.0f;
        allStimGiven = false;
        recordReaction = true; 
    }
}
