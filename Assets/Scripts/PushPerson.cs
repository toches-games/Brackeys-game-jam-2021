using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushPerson : MonoBehaviour
{
    private GameObject carController;
    private GameObject target;
    private Rigidbody rb;

    public Animator AnimController { get; set; }

    public const string TRIGGER_PUSH = "OnPush";
    public const string TRIGGER_DEAD = "OnDead";
    public const string FLOAT_RUN = "MultiplierRun";
    public const string FLOAT_PUSH = "MultiplierPush";

    public float speedToMoveTarget;
    public bool init = false;

    public float paddingX = 0;
    public float paddingZ = 0;

    // Start is called before the first frame update
    void Awake()
    {
        AnimController = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        carController = GameObject.Find("Car Controller");
        target = carController.GetComponent<CarController>().referToPerson;
    }

    void Start()
    {
        AnimController.SetFloat(FLOAT_RUN, 1.5f);
        StartCoroutine(GoToTarget());
    }

    private IEnumerator GoToTarget()
    {
        while (transform.position != new Vector3(   target.transform.position.x + paddingX,
                                                    target.transform.position.y,
                                                    target.transform.position.z + paddingZ))
        {

            transform.position = Vector3.MoveTowards(transform.position, new Vector3(
                                                    target.transform.position.x + paddingX,
                                                    target.transform.position.y,
                                                    target.transform.position.z + paddingZ),
                                                    Time.deltaTime * speedToMoveTarget);

            yield return new WaitForEndOfFrame();
        }

        AnimController.SetTrigger(TRIGGER_PUSH);
        init = true;
    }

    void Update()
    {
        if (init) {
            AnimController.SetFloat(FLOAT_PUSH, carController.GetComponent<CarController>().rig.velocity.z / 1.5f);
            transform.position = new Vector3(
                                                    target.transform.position.x + paddingX,
                                                    target.transform.position.y,
                                                    target.transform.position.z + paddingZ);
        }
    }

}
