using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public static class SceneController
{
    public static void GoToConfigurationScene()
    {
        Helpers.SetHardwareAltitudeAlert(false);
        Helpers.SetHardwareProximityAlert(false);
        SceneManager.LoadScene("ConfigureScene");
    }

    public static void StartGameScene()
    {
        Helpers.SetHardwareAltitudeAlert(false);
        Helpers.SetHardwareProximityAlert(false);
        SceneManager.LoadScene("Main");
    }
}
