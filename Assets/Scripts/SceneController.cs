using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public static class SceneController
{
    public static void GoToConfigurationScene()
    {
        SceneManager.LoadScene("ConfigureScene");
    }

    public static void StartGameScene()
    {
        SceneManager.LoadScene("Main");
    }
}
