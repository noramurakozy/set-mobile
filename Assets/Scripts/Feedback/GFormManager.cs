using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace Feedback
{
    public class GFormManager : MonoBehaviour
    {
        [SerializeField] private Button btnSubmit;
        [SerializeField] private List<GFormQuestion> questions;
        
        private const string kGFormBaseURL = "https://docs.google.com/forms/d/e/1FAIpQLSeiW_vFQprCdkLhaCTX2yTPdl_CZn6NKE68sfETeOTlXfUUzA/";
        // private const string kGFormEntryID = "entry.534933602";
        // entry.1798791499

        private void Start()
        {
            btnSubmit.onClick.AddListener(SubmitForm);
        }

        private void SubmitForm()
        {
            StartCoroutine(SendGFormData(questions));
        }
        
        private static IEnumerator SendGFormData(List<GFormQuestion> questions) {
 
            WWWForm form = new WWWForm();
            foreach (var question in questions)
            {
                form.AddField( question.entryID, question.Answer.ToString() );
                Debug.Log(question.Answer.ToString());
            }
            string urlGFormResponse = kGFormBaseURL + "formResponse";
            using ( UnityWebRequest www = UnityWebRequest.Post(urlGFormResponse, form) ) {
                yield return www.SendWebRequest();
            }
            Debug.Log("Request sent");
        }
    }
}