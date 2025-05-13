using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipForce : MonoBehaviour
{
    private ShipForces shipForces;

    [Header("Suspension")]
    [SerializeField] float restHeight = 10;
    [SerializeField] float suspensionStrength = 100;
    [SerializeField] float suspensionDamping = 10;
    [SerializeField] float raycastDistance = 20;
    [SerializeField] float gravity = -1;
    private float currentVelocity;

    private void Awake()
    {
        shipForces = transform.parent.GetComponent<ShipForces>();
    }

    public void PointSuspension()
    {
        Vector3 springDir = transform.up;  // World space spring direction
        Vector3 pointWorldVel = shipForces.shipRb.GetPointVelocity(transform.position);  // World space velocity of this point
        float force = 0;

        currentVelocity = Vector3.Dot(springDir, pointWorldVel);  // Calculate velocity along the point direction


        Ray ray = new Ray(transform.position, -transform.up);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, raycastDistance, shipForces.roadLayer))
        {
            float offset = restHeight - hit.distance;  // Calculate offset from the raycast

            force = (offset * suspensionStrength) - (currentVelocity * suspensionDamping);  // Calculate the magnitude of the suspension force
        }
        Debug.Log(force);

        float currentGravity = gravity / 1000;
        force += currentGravity;

        shipForces.shipRb.AddForceAtPosition(springDir * force, transform.position);  // Apply the force at this point

    }
}
