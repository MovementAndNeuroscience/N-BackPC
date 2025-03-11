using UnityEngine;
using System.Collections.Generic;
using System.Text;
using System.IO;

public class FileSaver : MonoBehaviour
{

    private List<string[]> rowData = new List<string[]>();

    public void SaveFile(string subjectId, int blockNo, string rawYesACC, string rawNoACC, string rawTotACC,
        float percentYesACC, float percentNoACC, float percentTotACC, float meanRTCorrectYes, float meanRTIncorrectYes,
        float meanRTCorrectNo, float meanRTIncorrectNo, float meanRTtot)
    {
        // Creating First row of titles manually..
        string[] rowDataTemp = new string[14];
        rowDataTemp[0] = "Raw Accuracy Yes answers" + blockNo;
        rowDataTemp[1] = "Raw Accuracy No answers" + blockNo;
        rowDataTemp[2] = "Raw Accuracy All answers" + blockNo;
        rowDataTemp[3] = "Percent Accuracy Yes answers" + blockNo;
        rowDataTemp[4] = "Percent Accuracy No answers" + blockNo;
        rowDataTemp[5] = "Percent Accuracy All answers" + blockNo;
        rowDataTemp[6] = "Mean reaction time correct yes answers" + blockNo;
        rowDataTemp[7] = "Mean reaction time incorrect yes answers" + blockNo;
        rowDataTemp[8] = "Mean reaction time correct no answers" + blockNo;
        rowDataTemp[9] = "Mean reaction time incorrect no answers" + blockNo;
        rowDataTemp[10] = "Mean reaction time all answers" + blockNo;
        rowData.Add(rowDataTemp);


        rowDataTemp = new string[11];
        rowDataTemp[0] = rawYesACC;
        rowDataTemp[1] = rawNoACC;
        rowDataTemp[2] = rawTotACC;
        rowDataTemp[3] = percentYesACC.ToString();
        rowDataTemp[4] = percentNoACC.ToString();
        rowDataTemp[5] = percentTotACC.ToString();
        rowDataTemp[6] = meanRTCorrectYes.ToString();
        rowDataTemp[7] = meanRTIncorrectYes.ToString();
        rowDataTemp[8] = meanRTCorrectNo.ToString();
        rowDataTemp[9] = meanRTIncorrectNo.ToString();
        rowDataTemp[10] = meanRTtot.ToString();
        rowData.Add(rowDataTemp);

        string[][] output = new string[rowData.Count][];

        for (int i = 0; i < output.Length; i++)
        {
            output[i] = rowData[i];
        }

        int length = output.GetLength(0);
        string delimiter = ";";

        StringBuilder sb = new StringBuilder();

        for (int index = 0; index < length; index++)
            sb.AppendLine(string.Join(delimiter, output[index]));


        string filePath = getPath(subjectId);

        StreamWriter outStream = File.CreateText(filePath);
        outStream.WriteLine(sb);
        outStream.Close();
    }

    // Following method is used to retrive the relative path as device platform
    private string getPath(string SubjectID)
    {
        var fileName = SubjectID + "_NBack_PC_Data.csv";
#if UNITY_EDITOR
        return Application.dataPath + "/CSV/" + fileName;
#elif UNITY_ANDROID
        return Application.persistentDataPath+fileName;
#elif UNITY_IPHONE
        return Application.persistentDataPath+"/"+fileName;
#else
        return Application.dataPath +"/"+fileName;
#endif
    }
}
