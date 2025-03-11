using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RestSharp;
using Newtonsoft.Json.Linq;
using System.Linq;

public class REDCapCommunicator : MonoBehaviour
{
    public GameObject dataGameObject;
    public REDCapSubjectData redcapData;

    public void Start()
    {
        redcapData = dataGameObject.GetComponent<REDCapSubjectData>();
    }
    public void PostRequest(int record, string data, string field_name)
    {
        redcapData = dataGameObject.GetComponent<REDCapSubjectData>();

        var client = new RestClient("https://redcap.nexs.ku.dk/api/");
        var request = new RestRequest();
        request.AddParameter("token", "D18893F3BA4ED8866D4FCB839968A3F9");
        request.AddParameter("content", "version");
        request.AddParameter("content", "record");
        request.AddParameter("format", "xml");
        request.AddParameter("type", "eav");
        request.AddParameter("overwriteBehavior", "overwite");
        request.AddParameter("forceAutoNumber", "false");
        request.AddParameter("data", "<records> <item> <record>" + redcapData.record_id + "</record> <field_name>"+field_name+"</field_name> <value>" + data + "</value> <redcap_event_name>"+ redcapData.redcap_event_name +"</redcap_event_name> </item> </records>");
        request.AddParameter("returnContent", "count");
        request.AddParameter("returnFormat", "json");

        request.AddHeader("header", "value");
        var response = client.Post(request);
        var content = response.Content;
        print("HTTP Status: " + content);
    }

    public string GetRecordIdFromChildId(string id)
    {
        var rec_id = "";
        var client = new RestClient("https://redcap.nexs.ku.dk/api/");
        var request = new RestRequest();
        request.AddParameter("token", "D18893F3BA4ED8866D4FCB839968A3F9");
        request.AddParameter("content", "report");
        request.AddParameter("format", "json");
        request.AddParameter("report_id", "566");
        request.AddParameter("csvDelimiter", "");
        request.AddParameter("rawOrLabel", "raw");
        request.AddParameter("rawOrLabelHeaders", "raw");
        request.AddParameter("exportCheckboxLabel", "false");
        request.AddParameter("returnFormat", "json");

        var response = client.Post(request);
        var content = response.Content;
        JArray convertedJson = JArray.Parse(content);

        var Ids = convertedJson.SelectToken("$.[?(@.child_id=='" + id + "')]");
        if (Ids != null)
        {
            rec_id = Ids["record_id"].ToString();
            var redCap_Event = Ids["redcap_event_name"].ToString();
            var splitted = redCap_Event.Split('_');
            redcapData.redcap_event_name = "post_intervention_arm_" + splitted[splitted.Length-1];
            var child_id = Ids["child_id"].ToString();
            redcapData.record_id = rec_id;
            redcapData.child_id = child_id;

            print("record_id : " + rec_id);
            return rec_id;
        }
        else
            return null;

    }
    public bool ValidateRecordId(string id)
    {
        var rec_id = "";
        var client = new RestClient("https://redcap.nexs.ku.dk/api/");
        var request = new RestRequest();
        request.AddParameter("token", "D18893F3BA4ED8866D4FCB839968A3F9");
        request.AddParameter("content", "report");
        request.AddParameter("format", "json");
        request.AddParameter("report_id", "566");
        request.AddParameter("csvDelimiter", "");
        request.AddParameter("rawOrLabel", "raw");
        request.AddParameter("rawOrLabelHeaders", "raw");
        request.AddParameter("exportCheckboxLabel", "false");
        request.AddParameter("returnFormat", "json");

        var response = client.Post(request);
        var content = response.Content;
        JArray convertedJson = JArray.Parse(content);

        var Ids = convertedJson.SelectTokens("$.[?(@.record_id=='" + id + "')]").ToList();
        if (Ids.Count != 0 )
        {
            rec_id = Ids[0]["record_id"].ToString();
            var redCap_Event = Ids[0]["redcap_event_name"].ToString();
            var splitted = redCap_Event.Split('_');
            redcapData.redcap_event_name = "post_intervention_arm_" + splitted[splitted.Length - 1];
            var child_id = Ids[0]["child_id"].ToString();
            redcapData.record_id = rec_id;
            redcapData.child_id = child_id;
            return true;
        }
        else
            return false;

    }

}
