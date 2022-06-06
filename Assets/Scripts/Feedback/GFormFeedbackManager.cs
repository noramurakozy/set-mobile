using System.Collections;
using System.Collections.Generic;
using FirebaseHandlers;
using UnityEngine;
using UnityEngine.Networking;

namespace Feedback
{
    public class GFormFeedbackManager : MonoBehaviour, IGFormManager
    {
        private const string GFormBaseURL =
            "https://docs.google.com/forms/d/e/1FAIpQLSeiW_vFQprCdkLhaCTX2yTPdl_CZn6NKE68sfETeOTlXfUUzA/";
        
        public void SendBugReport(GFormQuestion bugQuestion)
        {
            StartCoroutine(SendGFormData(new List<GFormQuestion> {bugQuestion}));
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

            form.AddField("entry.156133122",
                RemoteConfigValueManager.Instance.CustomAchievements
                    ? "Variant B (player defined achievements)"
                    : "Variant A (static/fixed achievements)");

            string urlGFormResponse = GFormBaseURL + "formResponse";
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