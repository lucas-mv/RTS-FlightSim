using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {
    [SerializeField] new Camera camera;
    [SerializeField] Plane plane;
    [SerializeField] PlaneHUD planeHUD;

    Vector3 controlInput;
    PlaneCamera planeCamera;
    AIController aiController;

    void Start() {
        planeCamera = GetComponent<PlaneCamera>();
        SetPlane(plane);    //SetPlane if var is set in inspector
    }

    void SetPlane(Plane plane) {
        this.plane = plane;
        aiController = plane.GetComponent<AIController>();

        if (planeHUD != null) {
            planeHUD.SetPlane(plane);
            planeHUD.SetCamera(camera);
        }

        planeCamera.SetPlane(plane);
    }

    public void OnToggleHelp(InputAction.CallbackContext context) {
        if (plane == null) return;

        if (context.phase == InputActionPhase.Performed) {
            planeHUD.ToggleHelpDialogs();
        }
    }

    public void SetThrottleInput(InputAction.CallbackContext context) {
        if (plane == null) return;
        if (aiController.enabled) return;

        plane.SetThrottleInput(context.ReadValue<float>());
    }

    public void OnRollPitchInput(InputAction.CallbackContext context) {
        if (plane == null) return;

        var input = context.ReadValue<Vector2>();
        controlInput = (new Vector3(input.y, controlInput.y, -input.x))/10;
    }

    public void OnYawInput(InputAction.CallbackContext context) {
        if (plane == null) return;

        var input = context.ReadValue<float>();
        controlInput = (new Vector3(controlInput.x, input, controlInput.z))/10;
    }

    public void OnCameraInput(InputAction.CallbackContext context) {
        if (plane == null) return;

        var input = context.ReadValue<Vector2>();
        planeCamera.SetInput(input);
    }

    public void OnFlapsInput(InputAction.CallbackContext context) {
        if (plane == null) return;

        if (context.phase == InputActionPhase.Performed) {
            plane.ToggleFlaps();
        }
    }

    public void OnToggleAI(InputAction.CallbackContext context) {
        if (plane == null) return;

        if (aiController != null) {
            aiController.enabled = !aiController.enabled;
        }
    }

    public void OnRestart(InputAction.CallbackContext context)
    {
        if (plane == null) return;

        Debug.Log("Restarting...");
        SceneController.GoToConfigurationScene();
    }

    public void OnToggleLandingGear(InputAction.CallbackContext context)
    {
        if (plane == null) return;
        Debug.Log("Toggling landing gear");
        plane.ToggleLandingGear();
    }

    void Update() {
        if (plane == null) return;
        if (aiController.enabled) return;

        plane.SetControlInput(controlInput);
    }
}
