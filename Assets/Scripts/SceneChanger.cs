using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
 
public class SceneChanger
{
    private static SceneChanger _instance;
    public static SceneChanger Instance => _instance ??= new SceneChanger();
    
    private List<string> sceneHistory = new();
    
    private SceneChanger()
    {
        sceneHistory.Add(SceneManager.GetActiveScene().name);
    }

    public void LoadScene(string newScene)
    {
        sceneHistory.Add(newScene);
        SceneManager.LoadScene(newScene);
    }
 
    public bool PreviousScene()
    {
        bool returnValue = false;
        if (sceneHistory.Count >= 2)
        {
            returnValue = true;
            sceneHistory.RemoveAt(sceneHistory.Count -1);
            SceneManager.LoadScene(sceneHistory[^1]);
        }
 
        return returnValue;
    }
 
}