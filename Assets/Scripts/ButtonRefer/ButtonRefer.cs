﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonRefer : MonoBehaviour
{
    [SerializeField] private RectTransform objectToRotate;
    [SerializeField] private Text textBox;
    [SerializeField] private GameEvent onDied;

    //Deserializar al invocar en GameManager
    [SerializeField] private int duration = 4;
    [SerializeField] private float smoothAnimation = 200;
    private float waitTime = 0;

    private KeyCode currentKeyCode;
    private bool detectingKey = false;
    private int time = 1;

    private KeyCode[] keyCodes = {
            KeyCode.Q, KeyCode.W, KeyCode.E, KeyCode.R, KeyCode.A, KeyCode.S, KeyCode.D, KeyCode.F,
            KeyCode.Z, KeyCode.X, KeyCode.C, KeyCode.V,

            KeyCode.T, KeyCode.Y, KeyCode.U, KeyCode.I, KeyCode.O, KeyCode.P, KeyCode.G, KeyCode.H,
            KeyCode.J, KeyCode.K, KeyCode.L, KeyCode.B, KeyCode.N, KeyCode.M
    };

    private void OnEnable()
    {
        currentKeyCode = keyCodes[Random.Range(0, keyCodes.Length)];
        textBox.text = currentKeyCode.ToString();
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Timer());
    }

    // Update is called once per frame
    void Update()
    {
        if (detectingKey)
        {
            DetectKeyPressed(currentKeyCode);
        }
    }

    private IEnumerator Timer()
    {
        detectingKey = true;
        waitTime = duration / smoothAnimation;
        for (time = 1; time <= smoothAnimation; time++)
        {
            objectToRotate.Rotate(new Vector3(0, 0, -(360f / smoothAnimation)));

            //Para sacar cada segundo
            //if(i == ((smoothAnimation / duration) * (time + 1)))

            yield return new WaitForSeconds(waitTime);
        }

        if (detectingKey)
        {
            onDied.Raise(0);
            detectingKey = false;
        }

    }

    private void DetectKeyPressed(KeyCode targetKey)
    {
        foreach (KeyCode vKey in keyCodes)
        {
            if (Input.GetKeyDown(vKey))
            {
                if(vKey == targetKey)
                {
                    onDied.Raise(CalculateRealTimeOfPressed(time));
                }
                else
                {
                    onDied.Raise(-1);
                }
                detectingKey = false;

                //Cambiar por animacion
                Destroy(gameObject);

                return;
            }
        }
    }

    private float CalculateRealTimeOfPressed(float time)
    {
        return time * waitTime;
    }

    public void ProbandoElEvento(dynamic v)
    {
        Debug.Log(v);
    }

    public void DestroyGameObject()
    {
        Destroy(gameObject);
    }
}
