using UnityEngine;
using UnityEngine.SceneManagement;

namespace DefaultNamespace
{
    public class BackButtonClickHandler : MonoBehaviour
    {
        public void OnClick()
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}