using UnityEngine;
using UnityEngine.SceneManagement;

public class BackButtonClickHandler : MonoBehaviour
{
    public bool backToMainMenu;
    public void OnClick()
    {
        if (backToMainMenu)
        {
            SceneChanger.Instance.LoadScene("MainMenu");
        }
        else
        {
            // Go back in history
            SceneChanger.Instance.PreviousScene();
        }
    }
}