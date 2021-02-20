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

    [SerializeField] public Difficulty currentDifficulty = Difficulty.easy;
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
        StartCoroutine(InitTimer(2f));
        buttonReferActives = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(buttonReferActives.Count);
        //Debug.Log(buttonLimitForLevel);
    }

    public void ChangeDifficulty(Difficulty newDifficulty)
    {
        currentDifficulty = newDifficulty;

        switch (currentDifficulty)
        {
            case Difficulty.easy:

                timeOfCreateButtonRefer = 2.5f;
                timeForButtonRefer = 15;

                break;
            case Difficulty.normal:

                timeOfCreateButtonRefer = 2f;
                timeForButtonRefer = 10;

                break;
            case Difficulty.hard:

                timeOfCreateButtonRefer = 1.5f;
                timeForButtonRefer = 5;

                break;
            case Difficulty.extreme:

                timeOfCreateButtonRefer = 0.7f;
                timeForButtonRefer = 3;

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
                GameObject instance = Instantiate(buttonRefer, Vector3.zero, Quaternion.identity,
                                                    parentButtonRefer.transform);

                instance.GetComponent<ButtonRefer>().InitButtonRefer(timeForButtonRefer);
                buttonReferActives.Add(instance);
                
            }
            yield return new WaitForSeconds(timeOfCreateButtonRefer);

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

    public void CalculateForce(dynamic time)
    {
        float pushForStraigh = 0, pushForUphill = 0, pushLimit = 0;
        switch (currentDifficulty)
        {
            case Difficulty.easy:

                switch ((float)time)
                {
                    case 0:

                        //Nothing

                        break;

                    case float n when (time > 0 && time <= timeForButtonRefer/10):

                        pushForStraigh = 100f;
                        pushForUphill = 200f;
                        pushLimit = 3;
                        plusCount++;

                        if(plusCount >= 4)
                        {
                            pushForStraigh = 400f;
                            pushForUphill = 500f;
                            //Code animation plus count
                            plusCount = 0;
                            Debug.Log("START PLUS");
                        }
                        break;

                    case float n when (time > timeForButtonRefer / 10 && time <= timeForButtonRefer / 3):

                        pushForStraigh = 50f;
                        pushForUphill = 100f;
                        pushLimit = 3;

                        break;

                    case var n when (time > timeForButtonRefer / 3 || time == -1):

                        pushForStraigh = -100f;
                        pushForUphill = -150f;
                        pushLimit = 0;

                        break;
                    default:
                        break;
                }

                break;
            case Difficulty.normal:



                break;
            case Difficulty.hard:



                break;
            case Difficulty.extreme:



                break;
            default:
                break;
        }

        carController.ApplyForce(pushForStraigh, pushForUphill, pushLimit);

    }

}
