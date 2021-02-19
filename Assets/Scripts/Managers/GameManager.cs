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

    public Difficulty currentDifficulty = Difficulty.easy;
    private int buttonLimitForLevel;
    private float timeOfCreateButtonRefer;
    private bool endGame = false;

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
        switch (currentDifficulty)
        {
            case Difficulty.easy:
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
    }

    public void ChangeDifficulty(Difficulty newDifficulty)
    {
        currentDifficulty = newDifficulty;

        switch (currentDifficulty)
        {
            case Difficulty.easy:

                timeOfCreateButtonRefer = 2.5f;
                buttonLimitForLevel = 1;

                break;
            case Difficulty.normal:

                timeOfCreateButtonRefer = 2f;
                buttonLimitForLevel = 2;

                break;
            case Difficulty.hard:

                timeOfCreateButtonRefer = 1.5f;
                buttonLimitForLevel = 2;

                break;
            case Difficulty.extreme:

                timeOfCreateButtonRefer = 0.5f;
                buttonLimitForLevel = 3;

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
                buttonReferActives.Add(instance);

                yield return new WaitForSeconds(timeOfCreateButtonRefer);
            }
        }
    }


    //TODO
    private IEnumerator InitTimer(float duration)
    {
        while (true)
        {
            yield return new WaitForSeconds(duration);
            StartCoroutine(CreateButtonRefer());
        }
    }
}
