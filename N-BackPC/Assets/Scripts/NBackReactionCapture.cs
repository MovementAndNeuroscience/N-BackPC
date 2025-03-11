using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NBackReactionCapture : MonoBehaviour
{
    private int correctYesAnswers = 0;
    private int correctNoAnswers = 0;
    private int incorrectYesAnswers = 0;
    private int incorrectNoAnswers = 0;
    private List<float> msCorrectYesAnswers = new List<float>();
    private List<float> msIncorrectYesAnswers = new List<float>();
    private List<float> msCorrectNoAnswers = new List<float>();
    private List<float> msIncorrectNoAnswers = new List<float>();

    private string rawYesACC = "";
    private string rawNoACC = "";
    private string rawTotACC = "";
    private float percentYesACC = 0.0f;
    private float percentNoACC = 0.0f;
    private float percentTotACC = 0.0f;
    private float meanRTCorrectYes = 0.0f;
    private float meanRTIncorrectYes = 0.0f;
    private float meanRTCorrectNo = 0.0f;
    private float meanRTIncorrectNo = 0.0f;
    private float meanRTtot = 0.0f; 

    public int GetCorrectYesAnswers()
    { return correctYesAnswers; }
    public int GetIncorrectYesAnswers()
    { return incorrectYesAnswers; }
    public int GetCorrectNoAnswers()
    { return correctNoAnswers; }
    public int GetIncorrectNoAnswers()
    { return incorrectNoAnswers; }
    public List<float> GetMsCorrectYesAnswers()
    { return msCorrectYesAnswers; }
    public List<float> GetMsIncorrectYesAnswers()
    { return msIncorrectYesAnswers; }
    public List<float> GetMsCorrectNoAnswers()
    { return msCorrectNoAnswers; }
    public List<float> GetMsIncorrectNoAnswers()
    { return msIncorrectNoAnswers; }

    public bool RecordReaction(int firstStim, int secondStim, float timer)
    {
        if(Input.GetKeyDown(KeyCode.LeftAlt) && firstStim == secondStim)
        {
            correctYesAnswers++;
            msCorrectYesAnswers.Add(timer);
            return true;
        }
        else if (Input.GetKeyDown(KeyCode.LeftAlt) && firstStim != secondStim)
        {
            incorrectNoAnswers++;
            msIncorrectNoAnswers.Add(timer);
            return true; 
        }
        else if(Input.GetKeyDown(KeyCode.RightAlt) && firstStim != secondStim)
        {
            correctNoAnswers++;
            msCorrectNoAnswers.Add(timer);
            return true;
        }
        else if (Input.GetKeyDown(KeyCode.RightAlt) && firstStim == secondStim)
        {   
            incorrectYesAnswers++;
            msIncorrectYesAnswers.Add(timer);
            return true;
        }
        return false;
    }
    public void CalculateReactionMatrics(int totalStim)
    {
        var totCorrect = correctYesAnswers + correctNoAnswers;
        var totIncorrect = incorrectYesAnswers + incorrectNoAnswers;
        rawYesACC = "Correct Yes : " + correctYesAnswers + " / Incorrect Yes : " + incorrectYesAnswers;
        rawNoACC = "Correct No : " + correctNoAnswers + " / Incorrect No : " + incorrectNoAnswers;
        rawTotACC = "Correct : " + totCorrect + " / Incorrect : " + totIncorrect;
        var totalYesAnswers = correctYesAnswers + incorrectYesAnswers;
        var totalNoAnswers = correctNoAnswers + incorrectNoAnswers;
        var totalCorrectAnswers = correctYesAnswers + correctNoAnswers;
        
        percentYesACC = ((float)correctYesAnswers / totalYesAnswers) * 100.0f;
        percentNoACC = ((float)correctNoAnswers / totalNoAnswers) * 100.0f;
        percentTotACC = ((float)totalCorrectAnswers / (totalYesAnswers + totalNoAnswers)) * 100.0f;

        float corrYesSum = 0.0f; 
        float corrNoSum = 0.0f;
        float incorrYesSum = 0.0f;
        float incorrNoSum = 0.0f;

        foreach (var reaction in msCorrectYesAnswers)
        {
            corrYesSum += reaction;
            meanRTCorrectYes = corrYesSum / msCorrectYesAnswers.Count;
        }
        foreach (var reaction in msIncorrectYesAnswers)
        {
            incorrYesSum += reaction;
            meanRTIncorrectYes = incorrYesSum / msIncorrectYesAnswers.Count;
        }
        foreach (var reaction in msCorrectNoAnswers)
        {
            corrNoSum += reaction;
            meanRTCorrectNo = corrNoSum / msCorrectNoAnswers.Count;
        }
        foreach (var reaction in msIncorrectNoAnswers)
        {
            incorrNoSum += reaction;
            meanRTIncorrectNo = incorrNoSum / msIncorrectNoAnswers.Count;
        }

        meanRTtot = ((meanRTCorrectYes * msCorrectYesAnswers.Count) + (meanRTIncorrectYes * msIncorrectYesAnswers.Count) + (meanRTCorrectNo * msCorrectNoAnswers.Count) + (meanRTIncorrectNo * msIncorrectNoAnswers.Count)) / (totalNoAnswers + totalYesAnswers); 
    }
    public void SaveAndPublishReactions(string subjectId, int blockNo)
    {
        GetComponent<FileSaver>().SaveFile(subjectId, blockNo, rawYesACC,rawNoACC,rawTotACC,
            percentYesACC,percentNoACC,percentTotACC, meanRTCorrectYes, meanRTIncorrectYes,
            meanRTCorrectNo,meanRTIncorrectNo, meanRTtot);

        int id = int.Parse(subjectId);

        if (blockNo == 0)
        {
            GetComponent<REDCapCommunicator>().PostRequest(id, rawYesACC, "nback_train_raw_yes_acc");
            GetComponent<REDCapCommunicator>().PostRequest(id, rawNoACC, "nback_train_raw_no_acc");
            GetComponent<REDCapCommunicator>().PostRequest(id, rawTotACC, "nback_train_raw_tot_acc");
            GetComponent<REDCapCommunicator>().PostRequest(id, percentYesACC.ToString(), "nback_train_per_yes_acc");
            GetComponent<REDCapCommunicator>().PostRequest(id, percentNoACC.ToString(), "nback_train_per_no_acc");
            GetComponent<REDCapCommunicator>().PostRequest(id, percentTotACC.ToString(), "nback_train_per_tot_acc");
            GetComponent<REDCapCommunicator>().PostRequest(id, meanRTCorrectYes.ToString(), "nback_train_mean_rt_coryes");
            GetComponent<REDCapCommunicator>().PostRequest(id, meanRTIncorrectYes.ToString(), "nback_train_mean_rt_incyes");
            GetComponent<REDCapCommunicator>().PostRequest(id, meanRTCorrectNo.ToString(), "nback_train_mean_rt_corno");
            GetComponent<REDCapCommunicator>().PostRequest(id, meanRTIncorrectNo.ToString(), "nback_train_mean_rt_incno");
            GetComponent<REDCapCommunicator>().PostRequest(id, meanRTtot.ToString(), "nback_train_mean_rt_all");
        }
        else if(blockNo == 1 || blockNo == 2)
        {
            GetComponent<REDCapCommunicator>().PostRequest(id, rawYesACC, "nback_block_raw_yes_acc");
            GetComponent<REDCapCommunicator>().PostRequest(id, rawNoACC, "nback_block_raw_no_acc");
            GetComponent<REDCapCommunicator>().PostRequest(id, rawTotACC, "nback_block_raw_tot_acc");
            GetComponent<REDCapCommunicator>().PostRequest(id, percentYesACC.ToString(), "nback_block_per_yes_acc");
            GetComponent<REDCapCommunicator>().PostRequest(id, percentNoACC.ToString(), "nback_block_per_no_acc");
            GetComponent<REDCapCommunicator>().PostRequest(id, percentTotACC.ToString(), "nback_block_per_tot_acc");
            GetComponent<REDCapCommunicator>().PostRequest(id, meanRTCorrectYes.ToString(), "nback_block_mean_rt_coryes");
            GetComponent<REDCapCommunicator>().PostRequest(id, meanRTIncorrectYes.ToString(), "nback_block_mean_rt_incyes");
            GetComponent<REDCapCommunicator>().PostRequest(id, meanRTCorrectNo.ToString(), "nback_block_mean_rt_corno");
            GetComponent<REDCapCommunicator>().PostRequest(id, meanRTIncorrectNo.ToString(), "nback_block_mean_rt_incno");
            GetComponent<REDCapCommunicator>().PostRequest(id, meanRTtot.ToString(), "nback_block_mean_rt_all");
        }
    }

}
