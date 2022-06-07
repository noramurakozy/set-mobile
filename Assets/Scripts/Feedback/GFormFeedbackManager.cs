using System.Collections;
using System.Collections.Generic;
using FirebaseHandlers;
using UnityEngine;
using UnityEngine.Networking;

namespace Feedback
{
    public class GFormFeedbackManager : MonoBehaviour, IGFormManager
    {
        private string _gFormBaseURL = "";
        private string _appVersionEntryID = "";
        
        public void SendBugReport(GFormQuestion bugQuestion)
        {
            _appVersionEntryID = "entry.156133122";
            _gFormBaseURL =
                "https://docs.google.com/forms/d/e/1FAIpQLSeiW_vFQprCdkLhaCTX2yTPdl_CZn6NKE68sfETeOTlXfUUzA/";
            StartCoroutine(SendGFormData(new List<GFormQuestion> {bugQuestion}));
        }
        
        public void SendFeedback(GFormQuestion feedbackQuestion)
        {
            _appVersionEntryID = "entry.156133122";
            _gFormBaseURL =
                "https://docs.google.com/forms/d/e/1FAIpQLSeu-iuY0A4hy4bCbkHTLZsdy5hYRg6RgcZsxa2tjMNZzPm1NQ/";
            StartCoroutine(SendGFormData(new List<GFormQuestion> {feedbackQuestion}));
        }

        public IEnumerator SendGFormData(List<GFormQuestion> questionList)
        {
            WWWForm form = new WWWForm();
            Debug.Log("Data sent:");
            foreach (var question in questionList)
            {
                form.AddField(question.entryID, question.Answer.ToString());
                Debug.Log($"{question.entryID}: {question.Answer}");
            }

            form.AddField(_appVersionEntryID,
                RemoteConfigValueManager.Instance.CustomAchievements
                    ? "Variant B (player defined achievements)"
                    : "Variant A (static/fixed achievements)");

            string urlGFormResponse = _gFormBaseURL + "formResponse";
            using (UnityWebRequest www = UnityWebRequest.Post(urlGFormResponse, form))
            {
                yield return www.SendWebRequest();
                if (www.result != UnityWebRequest.Result.Success)
                {
                    Debug.Log(www.error);
                }
                else
                {
                    Debug.Log("Request sent");
                    // Show results as text
                    Debug.Log(www.downloadHandler.text);
                }
            }
        }
    }
}