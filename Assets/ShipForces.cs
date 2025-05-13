using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipForces : MonoBehaviour
{
    [Header("Force Points")]
    public Rigidbody shipRb;
    [SerializeField] ShipForce FL;
    [SerializeField] ShipForce FR;
    [SerializeField] ShipForce BL;
    [SerializeField] ShipForce BR;

    [Header("Gravity")]
    public Vector3 gravityDirection;
    [SerializeField] float gravityRaycastDistance = 20;


    public LayerMask roadLayer;



    void Update()
    {
        Ray ray = new Ray(transform.position, -transform.up);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, gravityRaycastDistance, roadLayer))
        {
            gravityDirection = hit.normal;
        }

        Suspension();
    }

    private void Suspension()
    {
        FL.PointSuspension();
        FR.PointSuspension();
        BL.PointSuspension();
        BR.PointSuspension();
    }
}