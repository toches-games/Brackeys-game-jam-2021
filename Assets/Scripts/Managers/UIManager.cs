using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Text forceText;
    [SerializeField] private Text weightText;
    private int force;

    // Start is called before the first frame update
    void Start()
    {
        force = 0;
        forceText.text = force.ToString();
    }

    public void UpdateForceScore(float v)
    {
        StartCoroutine(UpdateForceScoreCoroutine(v));
    }

    public void UpdateWeidhtScore(float v)
    {

        weightText.text = (v * 10).ToString();
    }

    private IEnumerator UpdateForceScoreCoroutine(float v)
    {
        int changeForce = Mathf.RoundToInt(v);
        int adder = changeForce / 10;
        int target = force + changeForce;

        if (changeForce < 0)
        {
            while(force > target)
            {
                force += adder;
                forceText.text = force.ToString();
                yield return new WaitForSeconds(0.05f);

            }
        }
        else
        {
            while (force < target)
            {
                force += adder;
                forceText.text = force.ToString();
                yield return new WaitForSeconds(0.05f);

            }
        }

    }
}
