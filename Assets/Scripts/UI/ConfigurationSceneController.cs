using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConfigurationSceneController : MonoBehaviour
{
    [SerializeField] JobConfigurationSlider[] _jobConfigurationSliders;
    [SerializeField] Text _jobConfigurationText;

    const int MOVEMENT_JOB_INDEX = 0;
    const int HUD_JOB_INDEX = 1;
    const int PROXIMITY_JOB_INDEX = 2;
    const int ALTITUDE_JOB_INDEX = 3;
    const int LANDING_GEAR_JOB_INDEX = 4;

    private void Update()
    {
        int totalTime = 0;
        foreach(var job in _jobConfigurationSliders) totalTime += job.GetValue();

        _jobConfigurationText.text = totalTime > 100 ?
            "Some jobs will not be executed" :
            "All jobs will be executed";
    }

    public void OnStartGameClick()
    {
        PlayerPrefs.SetString(Constants.JOB_CONFIGURATIONS_PLAYER_PREFS_KEY,
            JsonUtility.ToJson(new JobConfigurationPlayerPref
            {
                MovementJobDuration = _jobConfigurationSliders[MOVEMENT_JOB_INDEX].GetValue(),
                HUDJobDuration = _jobConfigurationSliders[HUD_JOB_INDEX].GetValue(),
                ProximityJobDuration = _jobConfigurationSliders[PROXIMITY_JOB_INDEX].GetValue(),
                AltitudeJobDuration = _jobConfigurationSliders[ALTITUDE_JOB_INDEX].GetValue(),
                LandingGearJobDuration = _jobConfigurationSliders[LANDING_GEAR_JOB_INDEX].GetValue()
            }));
        SceneController.StartGameScene();
    }
}
