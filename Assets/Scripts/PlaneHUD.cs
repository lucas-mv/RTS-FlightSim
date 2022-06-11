using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlaneHUD : MonoBehaviour {
    [SerializeField]
    Color normalColor;
    [SerializeField]
    Color lockColor;
    [SerializeField]
    List<GameObject> helpDialogs;
    [SerializeField]
    Compass compass;
    [SerializeField]
    PitchLadder pitchLadder;
    [SerializeField]
    Bar throttleBar;
    [SerializeField]
    Transform hudCenter;
    [SerializeField]
    Transform velocityMarker;
    [SerializeField]
    Text airspeed;
    [SerializeField]
    Text aoaIndicator;
    [SerializeField]
    Text gforceIndicator;
    [SerializeField]
    Text altitude;
    [SerializeField]
    Bar healthBar;
    [SerializeField]
    Text healthText;
    [SerializeField]
    Transform targetBox;
    [SerializeField]
    Text targetName;
    [SerializeField]
    Text targetRange;
    [SerializeField]
    Transform missileLock;
    [SerializeField]
    Transform reticle;
    [SerializeField]
    RectTransform reticleLine;
    [SerializeField]
    RectTransform targetArrow;
    [SerializeField]
    RectTransform missileArrow;
    [SerializeField]
    float targetArrowThreshold;
    [SerializeField]
    float missileArrowThreshold;
    [SerializeField]
    float cannonRange;
    [SerializeField]
    float bulletSpeed;
    [SerializeField]
    GameObject aiMessage;

    [SerializeField]
    List<Graphic> missileWarningGraphics;

    Plane plane;
    AIController aiController;
    Target selfTarget;
    Transform planeTransform;
    new Camera camera;
    Transform cameraTransform;

    GameObject hudCenterGO;
    GameObject velocityMarkerGO;
    GameObject targetBoxGO;
    Image targetBoxImage;
    GameObject missileLockGO;
    Image missileLockImage;
    GameObject reticleGO;
    GameObject targetArrowGO;
    GameObject missileArrowGO;

    float lastUpdateTime;

    const float metersToKnots = 1.94384f;
    const float metersToFeet = 3.28084f;

    void Start() {
        hudCenterGO = hudCenter.gameObject;
        velocityMarkerGO = velocityMarker.gameObject;
        targetBoxGO = targetBox.gameObject;
        targetBoxImage = targetBox.GetComponent<Image>();
        missileLockGO = missileLock.gameObject;
        missileLockImage = missileLock.GetComponent<Image>();
        reticleGO = reticle.gameObject;
        targetArrowGO = targetArrow.gameObject;
        missileArrowGO = missileArrow.gameObject;
    }

    public void SetPlane(Plane plane) {
        this.plane = plane;

        if (plane == null) {
            planeTransform = null;
            selfTarget = null;
        }
        else {
            aiController = plane.GetComponent<AIController>();
            planeTransform = plane.GetComponent<Transform>();
            selfTarget = plane.GetComponent<Target>();
        }

        if (compass != null) {
            compass.SetPlane(plane);
        }

        if (pitchLadder != null) {
            pitchLadder.SetPlane(plane);
        }
    }

    public void SetCamera(Camera camera) {
        this.camera = camera;

        if (camera == null) {
            cameraTransform = null;
        } else {
            cameraTransform = camera.GetComponent<Transform>();
        }

        if (compass != null) {
            compass.SetCamera(camera);
        }

        if (pitchLadder != null) {
            pitchLadder.SetCamera(camera);
        }
    }

    public void ToggleHelpDialogs() {
        foreach (var dialog in helpDialogs) {
            dialog.SetActive(!dialog.activeSelf);
        }
    }

    void UpdateVelocityMarker() {
        var velocity = planeTransform.forward;

        if (plane.LocalVelocity.sqrMagnitude > 1) {
            velocity = plane.Rigidbody.velocity;
        }

        var hudPos = TransformToHUDSpace(cameraTransform.position + velocity);

        if (hudPos.z > 0) {
            velocityMarkerGO.SetActive(true);
            velocityMarker.localPosition = new Vector3(hudPos.x, hudPos.y, 0);
        } else {
            velocityMarkerGO.SetActive(false);
        }
    }

    void UpdateAirspeed() {
        var speed = plane.LocalVelocity.z * metersToKnots;
        airspeed.text = string.Format("{0:0}", speed);
    }

    void UpdateAOA() {
        aoaIndicator.text = string.Format("{0:0.0} AOA", plane.AngleOfAttack * Mathf.Rad2Deg);
    }

    void UpdateGForce() {
        var gforce = plane.LocalGForce.y / 9.81f;
        gforceIndicator.text = string.Format("{0:0.0} G", gforce);
    }

    void UpdateAltitude() {
        var altitude = plane.Rigidbody.position.y * metersToFeet;
        this.altitude.text = string.Format("{0:0}", altitude);
    }

    Vector3 TransformToHUDSpace(Vector3 worldSpace) {
        var screenSpace = camera.WorldToScreenPoint(worldSpace);
        return screenSpace - new Vector3(camera.pixelWidth / 2, camera.pixelHeight / 2);
    }

    void UpdateHUDCenter() {
        var rotation = cameraTransform.localEulerAngles;
        var hudPos = TransformToHUDSpace(cameraTransform.position + planeTransform.forward);

        if (hudPos.z > 0) {
            hudCenterGO.SetActive(true);
            hudCenter.localPosition = new Vector3(hudPos.x, hudPos.y, 0);
            hudCenter.localEulerAngles = new Vector3(0, 0, -rotation.z);
        } else {
            hudCenterGO.SetActive(false);
        }
    }

    void UpdateWarnings() {
        missileArrowGO.SetActive(false);

        foreach (var graphic in missileWarningGraphics) {
            graphic.color = normalColor;
        }

        pitchLadder.UpdateColor(normalColor);
        compass.UpdateColor(normalColor);
    }

    public void UpdateHUD() {
        if (plane == null) return;
        if (camera == null) return;

        float degreesToPixels = camera.pixelHeight / camera.fieldOfView;

        throttleBar.SetValue(plane.Throttle);

        UpdateVelocityMarker();
        UpdateHUDCenter();
        
        if (aiController != null) {
            aiMessage.SetActive(aiController.enabled);
        }

        UpdateAirspeed();
        UpdateAltitude();
        UpdateWarnings();
    }
}
