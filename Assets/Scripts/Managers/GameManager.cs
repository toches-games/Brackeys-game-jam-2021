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

    private void Awake()
    {
        SI = SI == null ? this : SI;
    }
    // Start is called before the first frame update
    void Start()
    {
        uiManager.UpdateWeidhtScore(carController.GetRB().mass);
        StartCoroutine(InitTimer(1f));
        buttonReferActives = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(currentDifficulty);
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
        Debug.Log("TIME: " + time);
        float pushForStraigh = 0, pushForUphill = 0, pushLimit = 0;

        switch (time)
        {
            case 0:

                //Nothing

                break;

            case float n when (time > 0 && time <= timeForButtonRefer / 10):

                pushForStraigh = 50f;
                pushForUphill = 100;
                pushLimit = 1;
                plusCount++;

                if (plusCount >= 4)
                {
                    pushForStraigh = 200f;
                    pushForUphill = 300;
                    //Code animation plus count
                    plusCount = 0;
                    Debug.Log("START PLUS");
                }
                break;

            case float n when (time > timeForButtonRefer / 10 && time <= timeForButtonRefer / 3):

                pushForStraigh = 20;
                pushForUphill = 80;
                pushLimit = 1;

                break;

            case var n when (time > timeForButtonRefer / 3 || time == -1):

                pushForStraigh = -80f;
                pushForUphill = -100f;
                pushLimit = -1;

                break;
            default:

                pushForStraigh = 20;
                pushForUphill = 80;
                pushLimit = 1;
                break;
        }

        carController.ApplyForce(pushForStraigh, pushForUphill, pushLimit);
        //switch (currentDifficulty)
        //{
        //    case Difficulty.easy:

        //        switch (time)
        //        {
        //            case 0:

        //                //Nothing

        //                break;

        //            case float n when (time > 0 && time <= timeForButtonRefer/10):

        //                pushForStraigh = 50f;
        //                pushForUphill = 100;
        //                pushLimit = 1;
        //                plusCount++;

        //                if(plusCount >= 4)
        //                {
        //                    pushForStraigh = 200f;
        //                    pushForUphill = 300;
        //                    //Code animation plus count
        //                    plusCount = 0;
        //                    Debug.Log("START PLUS");
        //                }
        //                break;

        //            case float n when (time > timeForButtonRefer / 10 && time <= timeForButtonRefer / 3):

        //                pushForStraigh = 20;
        //                pushForUphill = 80;
        //                pushLimit = 1;

        //                break;

        //            case var n when (time > timeForButtonRefer / 3 || time == -1):

        //                pushForStraigh = -80f;
        //                pushForUphill = -100f;
        //                pushLimit = -1;

        //                break;
        //            default:
        //                break;
        //        }

        //        break;
        //    case Difficulty.normal:



        //        break;
        //    case Difficulty.hard:



        //        break;
        //    case Difficulty.extreme:



        //        break;
        //    default:
        //        break;
        //}


    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            
            ChangeDifficulty(currentDifficulty + 1);
        }
    }

    public void GameOver()
    {
        //TODO
        Debug.Log("PERDISTE");
    }


}
