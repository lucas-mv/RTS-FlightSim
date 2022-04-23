using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour {
    [SerializeField]
    Plane plane;
    [SerializeField]
    float steeringSpeed;
    [SerializeField]
    float minSpeed;
    [SerializeField]
    float maxSpeed;
    [SerializeField]
    float recoverSpeedMin;
    [SerializeField]
    float recoverSpeedMax;
    [SerializeField]
    LayerMask groundCollisionMask;
    [SerializeField]
    float groundCollisionDistance;
    [SerializeField]
    float groundAvoidanceAngle;
    [SerializeField]
    float groundAvoidanceMinSpeed;
    [SerializeField]
    float groundAvoidanceMaxSpeed;
    [SerializeField]
    float pitchUpThreshold;
    [SerializeField]
    float fineSteeringAngle;
    [SerializeField]
    float rollFactor;
    [SerializeField]
    bool canUseMissiles;
    [SerializeField]
    bool canUseCannon;
    [SerializeField]
    float missileLockFiringDelay;
    [SerializeField]
    float missileFiringCooldown;
    [SerializeField]
    float missileMinRange;
    [SerializeField]
    float missileMaxRange;
    [SerializeField]
    float missileMaxFireAngle;
    [SerializeField]
    float bulletSpeed;
    [SerializeField]
    float cannonRange;
    [SerializeField]
    float cannonMaxFireAngle;
    [SerializeField]
    float cannonBurstLength;
    [SerializeField]
    float cannonBurstCooldown;
    [SerializeField]
    float minMissileDodgeDistance;

    Target selfTarget;
    Plane targetPlane;
    Vector3 lastInput;
    bool isRecoveringSpeed;

    float missileDelayTimer;
    float missileCooldownTimer;

    bool cannonFiring;
    float cannonBurstTimer;
    float cannonCooldownTimer;

    List<Vector3> dodgeOffsets;
    const float dodgeUpdateInterval = 0.25f;
    float dodgeTimer;

    void Start() {
        selfTarget = plane.GetComponent<Target>();

        dodgeOffsets = new List<Vector3>();
    }

    Vector3 AvoidGround() {
        //roll level and pull up
        var roll = plane.Rigidbody.rotation.eulerAngles.z;
        if (roll > 180f) roll -= 360f;
        return new Vector3(-1, 0, Mathf.Clamp(-roll * rollFactor, -1, 1));
    }

    Vector3 RecoverSpeed() {
        //roll and pitch level
        var roll = plane.Rigidbody.rotation.eulerAngles.z;
        var pitch = plane.Rigidbody.rotation.eulerAngles.x;
        if (roll > 180f) roll -= 360f;
        if (pitch > 180f) pitch -= 360f;
        return new Vector3(Mathf.Clamp(-pitch, -1, 1), 0, Mathf.Clamp(-roll * rollFactor, -1, 1));
    }

    Vector3 GetTargetPosition() {
         return plane.Rigidbody.position;
        

        //var targetPosition = plane.Target.Position;

        //if (Vector3.Distance(targetPosition, plane.Rigidbody.position) < cannonRange) {
        //    return Utilities.FirstOrderIntercept(plane.Rigidbody.position, plane.Rigidbody.velocity, bulletSpeed, targetPosition, plane.Target.Velocity);
        //}

        //return targetPosition;
    }

    Vector3 CalculateSteering(float dt, Vector3 targetPosition) {
        var error = targetPosition - plane.Rigidbody.position;
        error = Quaternion.Inverse(plane.Rigidbody.rotation) * error;   //transform into local space

        var errorDir = error.normalized;
        var pitchError = new Vector3(0, error.y, error.z).normalized;
        var rollError = new Vector3(error.x, error.y, 0).normalized;

        var targetInput = new Vector3();

        var pitch = Vector3.SignedAngle(Vector3.forward, pitchError, Vector3.right);
        if (-pitch < pitchUpThreshold) pitch += 360f;
        targetInput.x = pitch;

        if (Vector3.Angle(Vector3.forward, errorDir) < fineSteeringAngle) {
            targetInput.y = error.x;
        } else {
            var roll = Vector3.SignedAngle(Vector3.up, rollError, Vector3.forward);
            targetInput.z = roll * rollFactor;
        }

        targetInput.x = Mathf.Clamp(targetInput.x, -1, 1);
        targetInput.y = Mathf.Clamp(targetInput.y, -1, 1);
        targetInput.z = Mathf.Clamp(targetInput.z, -1, 1);

        var input = Vector3.MoveTowards(lastInput, targetInput, steeringSpeed * dt);
        lastInput = input;

        return input;
    }

    float CalculateThrottle(float minSpeed, float maxSpeed) {
        float input = 0;

        if (plane.LocalVelocity.z < minSpeed) {
            input = 1;
        } else if (plane.LocalVelocity.z > maxSpeed) {
            input = -1;
        }

        return input;
    }

    void FixedUpdate() {
        var dt = Time.fixedDeltaTime;

        Vector3 steering;
        float throttle;

        var velocityRot = Quaternion.LookRotation(plane.Rigidbody.velocity.normalized);
        var ray = new Ray(plane.Rigidbody.position, velocityRot * Quaternion.Euler(groundAvoidanceAngle, 0, 0) * Vector3.forward);

        if (Physics.Raycast(ray, groundCollisionDistance + plane.LocalAngularVelocity.z, groundCollisionMask.value)) {
            steering = AvoidGround();
            throttle = CalculateThrottle(groundAvoidanceMinSpeed, groundAvoidanceMaxSpeed);
        } else {
            Vector3 targetPosition = GetTargetPosition();

            if ((plane.LocalVelocity.z < recoverSpeedMin || isRecoveringSpeed)) {
                isRecoveringSpeed = plane.LocalVelocity.z < recoverSpeedMax;

                steering = RecoverSpeed();
                throttle = 1;
            } else {
                steering = CalculateSteering(dt, targetPosition);
                throttle = CalculateThrottle(minSpeed, maxSpeed);
            }
        }

        plane.SetControlInput(steering);
        plane.SetThrottleInput(throttle);
    }
}
