using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CarController : MonoBehaviour {
    [SerializeField] private float acceleration = 10f;
    [SerializeField] private float deceleration = 1f;
    [SerializeField] private float gravityScale = 1f;
    [SerializeField] private Rigidbody rig = default;

    private float currentAcceleration = 0f;

    // Test
    IEnumerator Start() {
        while(true) {
            yield return new WaitForSeconds(3);
            currentAcceleration += acceleration;
        }
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

        rig.velocity = new Vector3(0, rig.velocity.y, currentAcceleration);
    }

    private void Update() {
        SubtractAcceleration();
    }

    private void FixedUpdate() {
        Movement();
        Gravity();
    }
}