using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class SceneController : MonoBehaviour
{
    public GameObject subjectIdTextField;
    public GameObject InsertSubIdText;
    public GameObject demoSession;
    public GameObject normalTrainingSession;
    public GameObject introtext;
    public GameObject vejledningstext;
    public GameObject pause;
    public GameObject bike;
    public GameObject car;
    public GameObject cloud;
    public GameObject globe;
    public GameObject headset;
    public GameObject key;
    public GameObject cutlery;
    public GameObject eye;
    public GameObject node;
    public GameObject plane;
    public GameObject block1;
    public GameObject block2;



    public string subjectId;

    private bool enableIntroText = true;
    private bool enablePauseAfterDemo = true;
    private bool enablePauseAfterNormTraining = true;
    private bool enablePauseAfterBlock1 = true;
    private bool enablePauseAfterBlock2 = true; 

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;

        block1.SetActive(false);
        block2.SetActive(false);
        demoSession.SetActive(false);
        normalTrainingSession.SetActive(false);
        vejledningstext.SetActive(false);
        subjectIdTextField.SetActive(true);
        InsertSubIdText.SetActive(true);
        introtext.SetActive(false);
        pause.SetActive(false);
        bike.SetActive(false);
        car.SetActive(false);
        cloud.SetActive(false);
        globe.SetActive(false);
        headset.SetActive(false);
        key.SetActive(false);
        cutlery.SetActive(false);
        eye.SetActive(false);
        node.SetActive(false);
        plane.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Return) && enableIntroText)
        {
            var textField = subjectIdTextField.GetComponent<TMPro.TMP_InputField>();
            subjectId = textField.text;

            if(int.Parse(subjectId) > 9999)
            {
                var id = GetComponent<REDCapCommunicator>().GetRecordIdFromChildId(subjectId);
                if (id != null)
                {
                    subjectId = id;
                    subjectIdTextField.SetActive(false);
                    InsertSubIdText.SetActive(false);
                    introtext.SetActive(true);
                    enableIntroText = false;
                }
                else
                {
                    InsertSubIdText.GetComponent<TMPro.TMP_Text>().text = "Indtast et validt barne ID eller record ID";
                    subjectIdTextField.GetComponent<ActiveFieldAtStartUp>().activateField();
                }
            }
            else
            {
                if (GetComponent<REDCapCommunicator>().ValidateRecordId(subjectId))
                {
                    subjectIdTextField.SetActive(false);
                    InsertSubIdText.SetActive(false);
                    introtext.SetActive(true);
                    enableIntroText = false;
                }
                else
                {
                    InsertSubIdText.GetComponent<TMPro.TMP_Text>().text = "Indtast et validt barne ID eller record ID";
                    subjectIdTextField.GetComponent<ActiveFieldAtStartUp>().activateField();
                }
            }

            
        }

        if (demoSession.GetComponent<demoTrials>().allStimGiven && enablePauseAfterDemo)
        {
                pause.SetActive(true);
                normalTrainingSession.GetComponent<NBacktrainingSession>().setSubjectID(subjectId);
                enablePauseAfterDemo = false;
                demoSession.SetActive(false);
        }

        if (normalTrainingSession.GetComponent<NBacktrainingSession>().allStimGiven && enablePauseAfterNormTraining)
        {
            pause.SetActive(true);
            enablePauseAfterNormTraining = false;
            normalTrainingSession.SetActive(false);
            block1.GetComponent<NBacktrainingSession>().setSubjectID(subjectId);   
        }
        else if(normalTrainingSession.GetComponent<NBacktrainingSession>().allStimGiven == false)
        {
            enablePauseAfterNormTraining = true; 
        }

        if(block1.GetComponent<NBacktrainingSession>().allStimGiven && enablePauseAfterBlock1)
        {
            pause.SetActive(true);
            enablePauseAfterBlock1 = false;
            block1.SetActive(false);
            block2.GetComponent<NBacktrainingSession>().setSubjectID(subjectId);
        }
        if (block2.GetComponent<NBacktrainingSession>().allStimGiven && enablePauseAfterBlock2)
        {
            pause.SetActive(true);
            enablePauseAfterBlock2 = false;
            block2.SetActive(false);
        }
        //        if(block1.GetComponent<BlockController>().AllReactionTimesFound() && enablePauseAfterBlock1)
        //        {
        //           
        //            var blockno = 1;

        //            if(saveBlock1)
        //            {
        //                SaveBlock(audVisStimBlock1, blockno);
        //                saveBlock1 = false; 
        //            }
        //            block1.SetActive(false);
        //        }

        //        if (block2.GetComponent<BlockController>().AllReactionTimesFound() && enablePauseAfterBlock2)
        //        {
        //            pause.SetActive(true);
        //            enablePauseAfterBlock2 = false;
        //            var blockno = 2;

        //            if (saveBlock2)
        //            {
        //                SaveBlock(audVisStimBlock2, blockno);
        //                saveBlock2 = false;
        //            }
        //            block2.SetActive(false);
        //        }
        //        if (block3.GetComponent<BlockController>().AllReactionTimesFound() && enablePauseAfterBlock3)
        //        {
        //            pause.SetActive(true);
        //            enablePauseAfterBlock3 = false;
        //            var blockno = 3;

        //            if (saveBlock3)
        //            {
        //                SaveBlock(audVisStimBlock3, blockno);
        //                saveBlock3 = false;
        //            }
        //            block3.SetActive(false);
        //        }


        //        if (block1.GetComponent<BlockController>().AllReactionTimesFound() && block2.GetComponent<BlockController>().AllReactionTimesFound() && block3.GetComponent<BlockController>().AllReactionTimesFound() && block4.GetComponent<BlockController>().AllReactionTimesFound())
        //        {
        //            pause.SetActive(true);
        //            var blockno = 4;
        //            if (saveBlock4)
        //            {
        //                SaveBlock(audVisStimBlock4, blockno);
        //                saveBlock4 = false;
        //            }
        //        }
        //    }
    }

}
