using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Difficulty
{
    easy,
    normal,
    hard,
    extreme
}

public class GameManager : MonoBehaviour
{
    public static GameManager SI;
    [SerializeField] private GameObject buttonRefer;
    private List<GameObject> buttonReferActives;
    [SerializeField] private GameObject parentButtonRefer;

    [SerializeField] private CarController carController;
    [SerializeField] private UIManager uiManager;

    [SerializeField] public Difficulty currentDifficulty;
    private int buttonLimitForLevel = 1;
    private float timeOfCreateButtonRefer;
    private int timeForButtonRefer;
    private bool endGame = false;
    private int plusCount = 0;

    [SerializeField] private List<GameObject> pushPersonsPrefab;
    private List<PushPerson> pushPersonsActive;
    private int pushPersonCount = 1;
    private float accumulatorPaddingPersonZ = -0.5f;
    float paddingZ = 0;
    float paddingX = 0;

    private void Awake()
    {
        SI = SI == null ? this : SI;
    }
    // Start is called before the first frame update
    void Start()
    {
        
        buttonReferActives = new List<GameObject>();
        pushPersonsActive = new List<PushPerson>();
    }

    // Update is called once per frame
    public void StartGame()
    {
        uiManager.UpdateWeidhtScore(carController.GetRB().mass);
        StartCoroutine(uiManager.InitText());
        StartCoroutine(InitTimer(4f));
    }

    public void ChangeDifficulty(Difficulty newDifficulty)
    {
        currentDifficulty = newDifficulty;

        switch (currentDifficulty)
        {
            case Difficulty.easy:

                timeOfCreateButtonRefer = 1.3f;
                timeForButtonRefer = 10;

                break;
            case Difficulty.normal:

                timeOfCreateButtonRefer = 1.2f;
                timeForButtonRefer = 9;

                break;
            case Difficulty.hard:

                timeOfCreateButtonRefer = 1.1f;
                timeForButtonRefer = 8;

                break;
            case Difficulty.extreme:

                timeOfCreateButtonRefer = 1f;
                timeForButtonRefer = 7;

                break;
            default:
                break;
        }
    }

    private IEnumerator CreateButtonRefer()
    {
        while (!endGame)
        {
            if(buttonReferActives.Count < buttonLimitForLevel)
            {
                yield return new WaitForSeconds(timeOfCreateButtonRefer);
                GameObject instance = Instantiate(buttonRefer, Vector3.zero, Quaternion.identity,
                                                    parentButtonRefer.transform);

                instance.GetComponent<ButtonRefer>().InitButtonRefer(timeForButtonRefer);
                buttonReferActives.Add(instance);
                
            }
            yield return new WaitForEndOfFrame();

        }
    }

    public void DeleteButtonOfActives(GameObject btn)
    {
        buttonReferActives.Remove(btn);
    }

    private IEnumerator InitTimer(float duration)
    {
        yield return new WaitForSeconds(duration);
        ChangeDifficulty(Difficulty.easy);
        StartCoroutine(CreateButtonRefer());
    }

    public void CalculateForce(float time)
    {
        //Debug.Log("TIME: " + time);
        float pushForStraigh = 0, pushForUphill = 0, pushLimit = 0;

        switch (time)
        {
            case 0:

                //Nothing

                break;

            case float n when (time > 0 && time <= timeForButtonRefer / 10):

                pushForStraigh = 120f;
                pushForUphill = 170;
                pushLimit = 1;
                plusCount++;

                if (plusCount >= 4)
                {

                    pushForStraigh = 300f;
                    pushForUphill = 400;
                    plusCount = 0;

                    GeneratePushPerson(pushForStraigh);
                    
                }
                break;

            case float n when (time > timeForButtonRefer / 10 && time <= timeForButtonRefer / 3):

                pushForStraigh = 70;
                pushForUphill = 100;
                pushLimit = 1;
                plusCount = 0;

                break;

            case var n when (time > timeForButtonRefer / 3 || time == -1):

                pushForStraigh = -80f;
                pushForUphill = -60f;
                pushLimit = -1;
                plusCount = 0;

                break;
            default:

                pushForStraigh = 20;
                pushForUphill = 80;
                pushLimit = 1;
                plusCount = 0;

                break;
        }

        if (time > 0 && pushPersonCount == 1)
        {
            GeneratePushPerson(pushForStraigh);
        }
        carController.ApplyForce(pushForStraigh, pushForUphill, pushLimit);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            
            ChangeDifficulty(currentDifficulty + 1);
        }
    }

    private void GeneratePushPerson(float force)
    {
        pushPersonCount++;
        if (pushPersonCount > 2)
        {

            paddingZ = accumulatorPaddingPersonZ;

            if (pushPersonCount >= 4)
            {
                pushPersonCount = 2;
                accumulatorPaddingPersonZ += accumulatorPaddingPersonZ;

            }
        }
        if (pushPersonCount % 2 != 0)
        {
            paddingX = -0.2f;
        }
        else
        {
            paddingX = 0.8f;

        }

        Vector3 target = new Vector3(carController.referToPerson.transform.position.x + paddingX,
                    carController.referToPerson.transform.localPosition.y + 1,
                    carController.referToPerson.transform.position.z + paddingZ);

        GameObject instance = Instantiate(pushPersonsPrefab[Random.Range(0, pushPersonsPrefab.Count)],
                    new Vector3(target.x,
                    target.y,
                    target.z - 5),
                    Quaternion.identity,
                    GameObject.Find("PushPersons").transform);

        if(pushPersonCount == 2)
        {
            instance.GetComponent<PushPerson>().speedToMoveTarget =
                (carController.pushAcceleration + force) / 30f;
        }
        else
        {
            instance.GetComponent<PushPerson>().speedToMoveTarget =
                (carController.pushAcceleration + force) / 90f;
        }

        instance.GetComponent<PushPerson>().paddingX = paddingX;
        instance.GetComponent<PushPerson>().paddingZ = paddingZ;

        pushPersonsActive.Add(instance.GetComponent<PushPerson>());
    }

    public void GameOver()
    {
        foreach (var item in pushPersonsActive)
        {
            item.AnimController.SetTrigger("OnDead");
            item.init = false;
        }

        endGame = true;
    }

}
