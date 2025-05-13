using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipForce : MonoBehaviour
{
    private ShipForces shipForces;
    [HideInInspector] bool pointGrounded;

    [Header("Suspension")]
    [SerializeField] float restHeight = 10;
    [SerializeField] float suspensionStrength = 100;
    [SerializeField] float suspensionDamping = 10;
    [SerializeField] float raycastDistance = 20;
    [SerializeField] float gravity = -1;
    private float currentVelocity;


    [Header("Steering")]
    [SerializeField] float steeringPower = 30;
    [SerializeField] float airealSteeringPower = 30;


    [Header("Slipping")]
    [SerializeField][Range(0, 100)] float gripPercentage = 25;
    [SerializeField] float pointMass = 1;


    [Header("Acceleration")]
    [SerializeField] AnimationCurve torque;



    private void Awake()
    {
        shipForces = transform.parent.GetComponent<ShipForces>();
    }


    public bool CheckGrounded()
    {
        Ray ray = new Ray(transform.position, -transform.up);
        return Physics.Raycast(ray, raycastDistance * 2, shipForces.roadLayer);
    }

    public Vector3 CheckGravity(Vector3 gravityDir)
    {
        Ray ray = new Ray(transform.position, -transform.up);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, raycastDistance * 2, shipForces.roadLayer))
        {
            gravityDir += hit.normal;
        }

        return gravityDir;
    }


    public void PointSuspension()
    {
        Vector3 springDir = transform.up;  // World space spring direction
        Vector3 pointWorldVel = shipForces.shipRb.GetPointVelocity(transform.position);  // World space velocity of this point
        float force = 0;

        currentVelocity = Vector3.Dot(springDir, pointWorldVel);  // Calculate velocity along the point direction

        if (shipForces.grounded)
        {
            Ray ray = new Ray(transform.position, -transform.up);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, raycastDistance, shipForces.roadLayer))
            {
                float offset = restHeight - hit.distance;  // Calculate offset from the raycast

                force = (offset * suspensionStrength) - (currentVelocity * suspensionDamping);  // Calculate the magnitude of the suspension force
            }
        }

        float currentGravity = Vector3.Dot(shipForces.gravityDirection, new Vector3(0, gravity / 1000, 0));

        Debug.Log(force);

        shipForces.shipRb.AddForceAtPosition((springDir * force) + (shipForces.gravityDirection * (gravity / 1000)), transform.position);  // Apply the force at this point



        Debug.DrawRay(transform.position, (springDir * force) + (shipForces.gravityDirection * (gravity / 1000)), Color.green);
    }



    public void Steering(float input)
    {
        if (shipForces.grounded)
            transform.localRotation = Quaternion.Euler(0, input * steeringPower, 0);
        else
            transform.localRotation = Quaternion.Euler(0, input * airealSteeringPower, 0);
    }



    public void Slipping()
    {
        if (shipForces.grounded)
        {
            Vector3 steeringDir = transform.right;  // World space direction of the spring force

            Vector3 pointWorldVel = shipForces.shipRb.GetPointVelocity(transform.position);  // World space velocity of the point

            float steeringVel = Vector3.Dot(steeringDir, pointWorldVel);  // Calculate velocity along the steering direction

            float desiredVelChange = -steeringVel * (gripPercentage / 100);

            float desiredAccel = desiredVelChange / Time.fixedDeltaTime;  // Turn change in velocity into an acceleration

            shipForces.shipRb.AddForceAtPosition(steeringDir * pointMass * desiredAccel, transform.position);  // Force = Mass * Acceleration

            Debug.DrawRay(transform.position, steeringDir * pointMass * desiredAccel, Color.red);

        }
    }



    public void Acceleration(float accelInput)
    {
        if (shipForces.grounded)
        {
            Vector3 accelDir = transform.forward;  // World space direction of the accel force

            if (accelInput > 0.0f)
            {
                float shipSpeed = Vector3.Dot(shipForces.shipTrans.forward, shipForces.shipRb.velocity);  // Forward speed of the ship
                float normalizedSpeed = Mathf.Clamp01(Mathf.Abs(shipSpeed) / shipForces.topSpeed);
                float availableTorque = torque.Evaluate(normalizedSpeed) * accelInput;

                shipForces.shipRb.AddForceAtPosition(accelDir * availableTorque, transform.position);

                Debug.DrawRay(transform.position, accelDir * availableTorque, Color.blue);

            }


            if (accelInput < 0.0f)
            {
                float shipSpeed = Vector3.Dot(shipForces.shipTrans.forward, shipForces.shipRb.velocity);  // Forward speed of the ship
                float normalizedSpeed = Mathf.Clamp01(Mathf.Abs(shipSpeed) / shipForces.topSpeed);
                float availableTorque = torque.Evaluate(normalizedSpeed) * accelInput;

                shipForces.shipRb.AddForceAtPosition(accelDir * availableTorque, transform.position);

                Debug.DrawRay(transform.position, accelDir * availableTorque, Color.magenta);

            }
        }
    }
}
