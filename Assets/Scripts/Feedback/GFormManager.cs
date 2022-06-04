using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FirebaseHandlers;
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
        [SerializeField] private RectTransform finishedOverlaySuccess;
        [SerializeField] private RectTransform finishedOverlayError;
        [SerializeField] private TMP_Text overlayErrorTxt;
        [SerializeField] private Button btnHomeSuccess;
        [SerializeField] private Button btnHomeError;
        [SerializeField] private Button btnReportBug;
        [SerializeField] private Fader fader;

        private const string GFormBaseURL =
            "https://docs.google.com/forms/d/e/1FAIpQLSeIuceOpgIcaO0oGHOI8_DvNTEscFu3VxxxeiXqB8oEsm5BSA/";

        private void Start()
        {
            btnSubmit.onClick.AddListener(SubmitForm);
            btnHomeSuccess.onClick.AddListener(() => fader.ExitSceneAnimation("MainMenu"));
            btnHomeError.onClick.AddListener(() => fader.ExitSceneAnimation("MainMenu"));
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
                form.AddField("entry.1664004318", 
                    RemoteConfigValueManager.Instance.CustomAchievements 
                        ? "Variant B (player defined achievements)"
                        : "Variant A (static/fixed achievements)");
                
                string urlGFormResponse = GFormBaseURL + "formResponse";
                using (UnityWebRequest www = UnityWebRequest.Post(urlGFormResponse, form))
                {
                    yield return www.SendWebRequest();
                    if(www.result != UnityWebRequest.Result.Success) {
                        finishedOverlayError.gameObject.SetActive(true);
                        overlayErrorTxt.text = www.error;
                        Debug.Log(www.error);
                    }
                    else {
                        Debug.Log("Request sent");
                        finishedOverlaySuccess.gameObject.SetActive(true);
                        // Show results as text
                        Debug.Log(www.downloadHandler.text);
 
                        // Or retrieve results as binary data
                        byte[] results = www.downloadHandler.data;
                    }
                }

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