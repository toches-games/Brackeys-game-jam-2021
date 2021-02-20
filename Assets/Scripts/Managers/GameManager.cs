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

    [SerializeField] private Difficulty currentDifficulty = Difficulty.easy;
    private int buttonLimitForLevel = 1;
    private float timeOfCreateButtonRefer;
    private int timeForButtonRefer;
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
                //buttonLimitForLevel = 1;
                timeForButtonRefer = 6;

                break;
            case Difficulty.normal:

                timeOfCreateButtonRefer = 2f;
                //buttonLimitForLevel = 2;
                timeForButtonRefer = 5;

                break;
            case Difficulty.hard:

                timeOfCreateButtonRefer = 1.5f;
                //buttonLimitForLevel = 2;
                timeForButtonRefer = 4;

                break;
            case Difficulty.extreme:

                timeOfCreateButtonRefer = 0.7f;
                //buttonLimitForLevel = 3;
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
}
