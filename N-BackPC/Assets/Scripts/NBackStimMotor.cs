using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NBackStimMotor : MonoBehaviour
{
    public GameObject bike;
    public GameObject car;
    public GameObject cloud;
    public GameObject curtlery;
    public GameObject eye;
    public GameObject globe;
    public GameObject headset;
    public GameObject key;
    public GameObject node;
    public GameObject plane; 

    public void HidAllStimulus()
    {
        bike.SetActive(false);
        car.SetActive(false);
        cloud.SetActive(false);
        curtlery.SetActive(false);
        eye.SetActive(false);
        globe.SetActive(false);
        headset.SetActive(false);
        key.SetActive(false);
        node.SetActive(false);
        plane.SetActive(false);
    }

    public void showStimuli(int StimNumber, bool state)
    {
        switch (StimNumber)
        {
            case 1:
                bike.SetActive(state);
                break;
            case 2:
                car.SetActive(state);
                break;
            case 3:
                cloud.SetActive(state);
                break;
            case 4:
                curtlery.SetActive(state);
                break;
            case 5:
                eye.SetActive(state);
                break;
            case 6:
                globe.SetActive(state);
                break;
            case 7:
                headset.SetActive(state);
                break;
            case 8:
                key.SetActive(state);
                break;
            case 9:
                node.SetActive(state);
                break;
            case 10:
                plane.SetActive(state);
                break;
            default:
                break;
        }
    }

    public (int firstStim, int secondStim, bool found, 
        int yes, int no, int prevStim, bool wasPrevYes) 
        FindingTheNextTwoStimuli( bool stimulifound, int allowedYes, 
        int stimCounter, int totalStim,
        bool wasPrevYes, int prevSecondStim, int allowedNo)
    {
        while (stimulifound == false)
        {
            var stim = DetermineNewStim();
            int randomFirstStim = stim.Item1;
            int randomSecondStim = stim.Item2;

            if (randomFirstStim == randomSecondStim && allowedYes != 0 && stimCounter != totalStim)
            {
                if (wasPrevYes && randomFirstStim != prevSecondStim)
                {
                    allowedYes--;
                    prevSecondStim = randomSecondStim;
                    wasPrevYes = true;
                    stimulifound = true;
                    return (randomFirstStim, randomSecondStim, stimulifound, allowedYes, allowedNo, prevSecondStim, wasPrevYes);
                }
                else if (wasPrevYes == false)
                {
                    allowedYes--;
                    prevSecondStim = randomSecondStim;
                    wasPrevYes = true;
                    stimulifound = true;
                    return (randomFirstStim, randomSecondStim, stimulifound, allowedYes, allowedNo, prevSecondStim, wasPrevYes);
                }
            }
            else if (randomFirstStim != randomSecondStim && allowedNo != 0 && stimCounter != totalStim)
            {
                if (wasPrevYes && randomFirstStim != prevSecondStim)
                {
                    allowedNo--;
                    wasPrevYes = false;
                    stimulifound = true;
                    return (randomFirstStim, randomSecondStim, stimulifound, allowedYes, allowedNo, prevSecondStim, wasPrevYes);
                }
                else if (wasPrevYes == false)
                {
                    allowedNo--;
                    wasPrevYes = false;
                    stimulifound = true;
                    return (randomFirstStim, randomSecondStim, stimulifound, allowedYes, allowedNo, prevSecondStim, wasPrevYes);
                }
            }
        }
        return (11, 11, stimulifound, allowedYes, allowedNo, prevSecondStim, wasPrevYes);
    }

    private Tuple<int, int> DetermineNewStim()
    {
        return Tuple.Create(UnityEngine.Random.Range(1, 11),UnityEngine.Random.Range(1, 11)) ;
        ;
    }
}
