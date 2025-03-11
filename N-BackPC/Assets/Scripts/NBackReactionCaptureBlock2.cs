using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NBackReactionCaptureBlock2 : MonoBehaviour
{
    public GameObject BLock1;

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
        var b1CorrYes = BLock1.GetComponent<NBackReactionCapture>().GetCorrectYesAnswers();
        var b1InCorrYes = BLock1.GetComponent<NBackReactionCapture>().GetIncorrectYesAnswers();
        var b1CorrNo = BLock1.GetComponent<NBackReactionCapture>().GetCorrectNoAnswers();
        var b1InCorrNo = BLock1.GetComponent<NBackReactionCapture>().GetIncorrectNoAnswers();

        var b1MsCorrYes = BLock1.GetComponent<NBackReactionCapture>().GetMsCorrectYesAnswers();
        var b1MsInCorrYes = BLock1.GetComponent<NBackReactionCapture>().GetMsIncorrectYesAnswers();
        var b1MsCorrNo = BLock1.GetComponent<NBackReactionCapture>().GetMsCorrectNoAnswers();
        var b1MsInCorrNo = BLock1.GetComponent<NBackReactionCapture>().GetMsIncorrectNoAnswers();

        var totCorrect = correctYesAnswers + correctNoAnswers + b1CorrNo + b1CorrYes;
        var totIncorrect = incorrectYesAnswers + incorrectNoAnswers + b1InCorrNo + b1InCorrYes;
        var totCorYes = correctYesAnswers + b1CorrYes;
        var totInCorrYes = incorrectYesAnswers + b1InCorrYes;
        var totCorNo = correctNoAnswers + b1CorrNo;
        var totInCorNo = incorrectNoAnswers + b1InCorrNo;

        rawYesACC = "Correct Yes : " + totCorYes + " / Incorrect Yes : " + totInCorrYes;
        rawNoACC = "Correct No : " + totCorNo + " / Incorrect No : " + totInCorNo;
        rawTotACC = "Correct : " + totCorrect + " / Incorrect : " + totIncorrect;
        var totalYesAnswers = correctYesAnswers + incorrectYesAnswers + b1CorrYes + b1InCorrYes;
        var totalNoAnswers = correctNoAnswers + incorrectNoAnswers + b1CorrNo + b1InCorrNo;
        
        percentYesACC = ((float)(correctYesAnswers + b1CorrYes )/ totalYesAnswers) * 100.0f;
        percentNoACC = ((float)(correctNoAnswers + b1CorrNo)/ totalNoAnswers) * 100.0f;
        percentTotACC = ((float)totCorrect / (totalYesAnswers + totalNoAnswers)) * 100.0f;

        msCorrectNoAnswers.AddRange(b1MsCorrNo);
        msCorrectYesAnswers.AddRange(b1MsCorrYes);
        msIncorrectNoAnswers.AddRange(b1MsInCorrNo);
        msIncorrectYesAnswers.AddRange(b1MsInCorrYes);

        foreach (var reaction in msCorrectYesAnswers)
        {
            float sum =+ reaction;
            meanRTCorrectYes = sum / msCorrectYesAnswers.Count;
        }
        foreach (var reaction in msIncorrectYesAnswers)
        {
            float sum =+ reaction;
            meanRTIncorrectYes = sum / msIncorrectYesAnswers.Count;
        }
        foreach (var reaction in msCorrectNoAnswers)
        {
            float sum =+ reaction;
            meanRTCorrectNo = sum / msCorrectNoAnswers.Count;
        }
        foreach (var reaction in msIncorrectNoAnswers)
        {
            float sum =+ reaction;
            meanRTIncorrectNo = sum / msIncorrectNoAnswers.Count;
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
