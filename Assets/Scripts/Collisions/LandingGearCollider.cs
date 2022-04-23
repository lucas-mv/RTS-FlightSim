
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandingGearCollider : MonoBehaviour
{
    [SerializeField] Plane plane;

    private void OnTriggerEnter(Collider other)
    {
        plane.OnLandingGearCollision(other);
    }
}
