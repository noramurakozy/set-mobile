using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace Feedback
{
    public class GFormManager : MonoBehaviour
    {
        [SerializeField] private Button btnSubmit;
        [SerializeField] private TMP_Text txtError;
        [SerializeField] private List<GFormQuestion> questions;

        private const string GFormBaseURL =
            "https://docs.google.com/forms/d/e/1FAIpQLSeIuceOpgIcaO0oGHOI8_DvNTEscFu3VxxxeiXqB8oEsm5BSA/";

        private void Start()
        {
            btnSubmit.onClick.AddListener(SubmitForm);
        }

        private void Update()
        {
            // All answers except for the last question which is to report the app version the user uses
            // That one is not included in the question list
            if (questions.Select(q => q.Answer).Any(answer => answer == null))
            {
                DisplayErrorText();
            }
            else
            {
                HideErrorText();
            }
        }

        private void SubmitForm()
        {
            StartCoroutine(SendGFormData(questions));
        }

        private IEnumerator SendGFormData(List<GFormQuestion> questionList)
        {
            if (questions.Select(q => q.Answer).Any(answer => answer == null))
            {
                DisplayErrorText();
            }
            else
            {
                HideErrorText();
                WWWForm form = new WWWForm();
                Debug.Log("Data sent:");
                foreach (var question in questionList)
                {
                    form.AddField(question.entryID, question.Answer.ToString());
                    Debug.Log($"{question.entryID}: {question.Answer}");
                }
                // TODO send A/B test version data
                form.AddField("entry.1664004318", "Variant B (player defined achievements)");
                
                string urlGFormResponse = GFormBaseURL + "formResponse";
                using (UnityWebRequest www = UnityWebRequest.Post(urlGFormResponse, form))
                {
                    yield return www.SendWebRequest();
                }

                Debug.Log("Request sent");
            }
        }

        private void DisplayErrorText()
        {
            txtError.gameObject.SetActive(true);
            btnSubmit.enabled = false;
            btnSubmit.GetComponentInChildren<TMP_Text>().color = new Color32(142, 142, 142, 255);
        }

        private void HideErrorText()
        {
            txtError.gameObject.SetActive(false);
            btnSubmit.enabled = true;
            btnSubmit.GetComponentInChildren<TMP_Text>().color = Color.black;
        }
    }
}