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

    public LayerMask roadLayer;



    void Update()
    {
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