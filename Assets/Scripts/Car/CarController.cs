using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CarController : MonoBehaviour {
    [SerializeField] private float acceleration = 10f;
    [SerializeField] private float deceleration = 1f;
    [SerializeField] private float gravityScale = 1f;
    [SerializeField] private Rigidbody rig = default;
    [SerializeField] private WheelCollider[] wheels;

    private float currentAcceleration = 0f;

    // Test
    IEnumerator Start() {
        while(true) {
            yield return new WaitForSeconds(3);
            currentAcceleration += acceleration;
        }
    }

    private void UpdateWheels() {
        foreach (WheelCollider wheel in wheels)
        {
            ApplyLocalPositionToVisuals(wheel);
        }
    }

    private void ApplyLocalPositionToVisuals(WheelCollider collider)
    {
        Transform visualWheel = collider.transform.GetChild(0);
     
        Vector3 position;
        Quaternion rotation;
        collider.GetWorldPose(out position, out rotation);
     
        visualWheel.transform.position = position;
        visualWheel.transform.rotation = rotation;
    }

    private void SubtractAcceleration() {
        if(currentAcceleration > 0) {
            currentAcceleration -= deceleration * Time.deltaTime;
        }
    }

    private void Gravity() {
        rig.AddForce(Vector3.down * 9.8f * gravityScale, ForceMode.Acceleration);
    }

    private void Movement() {
        Vector3 vertical = transform.forward * currentAcceleration;
        Vector2 horizontalVelocity = new Vector2(rig.velocity.x, rig.velocity.z);

        if(rig.velocity.y >= 0f) {
            rig.velocity = new Vector3(0, rig.velocity.y, currentAcceleration);
        }

        else {
            rig.velocity = new Vector3(0, rig.velocity.y, rig.velocity.z);
        }
    }

    private void Update() {
        SubtractAcceleration();
    }

    private void FixedUpdate() {
        Movement();
        Gravity();
        //UpdateWheels();

        print(rig.velocity);
    }
}