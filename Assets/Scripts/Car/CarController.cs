using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CarController : MonoBehaviour {
    [SerializeField] private Vector3 visualVelocity;
    [SerializeField] private float limitVelocity;
    [SerializeField] private float time;
    [SerializeField] private float pushAcceleration;

    [SerializeField] private float currentAcceleration;
    [SerializeField] private float forceForCorrect;
    [SerializeField] private float forceForIncorrect;
    [SerializeField] private float forceForPlus;
    [SerializeField] private float targetAcceleration;
    [SerializeField] private float percentForAerodynamicsDeceleration;
    [SerializeField] private Rigidbody rig = default;

    private const float MAX_VELOCITY = 5f;
    private const float MAX_PUSH_ACCELERATION = 1500f;
    private const float INIT_PUSH_ACCELERATION = 300f;
    private bool changeForce = true;

    IEnumerator Start()
    {
        while (true)
        {
            if(rig.velocity.z < limitVelocity)
            {
                AddForceZ(pushAcceleration);
            }

            yield return new WaitForSeconds(time);
        }
    }

    private void SubtractAerodynamicsAcceleration() {
        if(rig.velocity.z > 0) {
            
            rig.AddRelativeForce(transform.forward * -1 * rig.velocity.z 
                    * percentForAerodynamicsDeceleration, ForceMode.Force);
        }
    }

    private void AddForceZ(float v)
    {
        rig.AddRelativeForce(transform.forward * v, ForceMode.Force);
    }

    private void Update() {

        SubtractAerodynamicsAcceleration();
        visualVelocity = rig.velocity;

        if(transform.rotation.x < -0.05f && changeForce)
        {
            
            changeForce = false;
        }
        else if(transform.rotation.x > -0.1)
        {
            changeForce = true;

        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            limitVelocity += forceForPlus;

        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            if (transform.rotation.x < -0.05f)
            {
                pushAcceleration += 50f;
            }
            else if(limitVelocity < MAX_VELOCITY && rig.velocity.z > limitVelocity - 1.5f)
            {
                limitVelocity += forceForCorrect;

            }
            else
            {
                pushAcceleration += 20f;

            }

            if(pushAcceleration >= MAX_PUSH_ACCELERATION)
            {
                if (!changeForce)
                {

                    pushAcceleration = MAX_PUSH_ACCELERATION;
                }
                else
                {
                    pushAcceleration /= 1.5f;
                }
            }
        }
        else if (Input.GetKeyDown(KeyCode.I))
        {
            limitVelocity += forceForIncorrect;
        }

    }

}