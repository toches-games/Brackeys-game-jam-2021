using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CarController : MonoBehaviour {
    [SerializeField] private Vector3 visualVelocity;
    [SerializeField] private float limitVelocity;
    [SerializeField] private float time;
    [SerializeField] private float pushAcceleration;
    [SerializeField] private float percentForAerodynamicsDeceleration;
    [SerializeField] private Rigidbody rig = default;

    [SerializeField] private GameEvent onGameOver;
    [SerializeField] private GameEvent onChangeForce;


    private float timeOfResponse;
    public float MAX_VELOCITY;
    public float MAX_PUSH_ACCELERATION;
    public float INIT_PUSH_ACCELERATION;
    public float CONDITION_GAME_OVER;
    private bool changeForce = true;

    private void Awake()
    {
        INIT_PUSH_ACCELERATION = pushAcceleration;
        limitVelocity = MAX_VELOCITY;
    }

    IEnumerator Start()
    {
        while (true)
        {
            if (rig.velocity.z < limitVelocity && pushAcceleration > 0)
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

        //Debug.Log(transform.rotation);

        SubtractAerodynamicsAcceleration();
        visualVelocity = rig.velocity;

        if(transform.rotation.x < -0.05f && changeForce)
        {
            if(pushAcceleration >= 1000)
            {
                float subtractor = pushAcceleration / 4f;
                pushAcceleration -= Mathf.Round(subtractor);
                onChangeForce.Raise(-subtractor);
            }


            changeForce = false;
        }
        else if(transform.rotation.x > -0.1)
        {
            changeForce = true;

        }

        if (rig.velocity.z < CONDITION_GAME_OVER)
        {
            onGameOver.Raise(1);
        }

    }

    public void ApplyForce(float pushForStraigh, float pushForUphill, float pushLimit)
    {
        float oldAcceleration = pushAcceleration;
        if (transform.rotation.x < -0.05f)
        {
            pushAcceleration += pushForUphill;
            //Debug.Log("PushUphill");

        }
        //else if (limitVelocity < MAX_VELOCITY && rig.velocity.z > limitVelocity - 0.5f)
        //{
        //    if((limitVelocity + pushLimit) < 0)
        //        {
        //        limitVelocity = 0;
        //    }
        //    else
        //    {
        //        limitVelocity += pushLimit;
        //    }
        //    //Debug.Log("PushLimit");
        //}
        else
        {
            //Debug.Log("PushStraigh");
            if((pushAcceleration + pushForStraigh) < 0)
            {
                pushAcceleration = 0;
            }
            else
            {
                pushAcceleration += pushForStraigh;
            }
            

        }

        if (pushAcceleration >= MAX_PUSH_ACCELERATION)
        {
            if (!changeForce)
            {

                pushAcceleration = MAX_PUSH_ACCELERATION;
            }
            //else
            //{
            //    pushAcceleration /= 1.5f;
            //}
        }
        //if(limitVelocity <= 0)
        //{
        //    onChangeForce.Raise(-pushAcceleration);

        //}
        //else
        //{
        //}


        onChangeForce.Raise(pushAcceleration - oldAcceleration);

    }

    public Rigidbody GetRB()
    {
        return rig;
    }
}