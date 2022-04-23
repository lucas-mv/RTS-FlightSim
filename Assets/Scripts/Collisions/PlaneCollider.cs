using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneCollider : MonoBehaviour
{
    [SerializeField] Plane plane;

    private void OnTriggerEnter(Collider other)
    {
        plane.OnCrashCollision(other);
    }

    private void OnTriggerExit(Collider other)
    {
        plane.OnLeftLandingRunway();
    }
}
