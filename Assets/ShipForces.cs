using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipForces : MonoBehaviour
{
    [Header("Force Points")]
    public Transform shipTrans;
    public Rigidbody shipRb;
    [SerializeField] ShipForce FL;
    [SerializeField] ShipForce FR;
    [SerializeField] ShipForce BL;
    [SerializeField] ShipForce BR;

    [Header("Gravity")]
    public Vector3 gravityDirection;
    [SerializeField] float gravityRaycastDistance = 20;
    public bool grounded;
    [SerializeField][Range(0, 4)] int groundedPointsRequired = 3;

    [SerializeField] Vector3 targetAirRotation;
    [SerializeField] float airRotationResetSpeed = 0.5f;

    public float topSpeed = 10;


    public LayerMask roadLayer;

    private void Update()
    {
        CheckGrounded();

        CheckGravityDirection();

        Steering(Input.GetAxis("Horizontal"));

        if (!grounded)
        {
            gravityDirection = Vector3.zero;
            //Quaternion rotationTarget = Quaternion.Euler(new Vector3(targetAirRotation.x, shipTrans.rotation.y, shipTrans.rotation.z));         
            //shipTrans.rotation = Quaternion.Slerp(shipTrans.rotation, rotationTarget, airRotationResetSpeed * Time.deltaTime);

            //shipTrans.rotation = Quaternion.Euler(new Vector3(
            //    Mathf.Lerp(shipTrans.rotation.eulerAngles.x, targetAirRotation.x, 0.1f * Time.deltaTime),
            //    shipTrans.rotation.eulerAngles.y,
            //    Mathf.Lerp(shipTrans.rotation.eulerAngles.z, targetAirRotation.z, 0.1f * Time.deltaTime)
            //    ));
        }
    }

    void FixedUpdate()
    {
        Suspension();
        Slipping();
        Acceleration(Input.GetAxis("Vertical"));
    }

    private void CheckGrounded()
    {
        int groundedPoints = 0;
        if (FL.CheckGrounded()) groundedPoints++;
        if (FR.CheckGrounded()) groundedPoints++;
        if (BL.CheckGrounded()) groundedPoints++;
        if (BR.CheckGrounded()) groundedPoints++;

        grounded = groundedPoints >= groundedPointsRequired;
    }

    private void CheckGravityDirection()
    {
        Vector3 g = Vector3.zero;
        g = FL.CheckGravity(g);
        g = FR.CheckGravity(g);
        g = BL.CheckGravity(g);
        g = BR.CheckGravity(g);

        gravityDirection = Vector3.Normalize(g);
    }

    private void Suspension()
    {
        FL.PointSuspension();
        FR.PointSuspension();
        BL.PointSuspension();
        BR.PointSuspension();
    }

    private void Steering(float input)
    {
        FL.Steering(input);
        FR.Steering(input);
        BL.Steering(input);
        BR.Steering(input);
    }


    private void Slipping()
    {
        FL.Slipping();
        FR.Slipping();
        BL.Slipping();
        BR.Slipping();
    }


    private void Acceleration(float input)
    {
        FL.Acceleration(input);
        FR.Acceleration(input);
        BL.Acceleration(input);
        BR.Acceleration(input);
    }
}