using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Sound
{
    effortUp,
    effortLoop,
    corneta,
    logro,
    anger
}

public class SFXManager : MonoBehaviour
{
    public static SFXManager SI;

    private void Awake()
    {
        SI = SI == null ? this : SI;
    }

    //Referencias a los audios source respectivos
    [SerializeField] private AudioSource effortUp, effortLoop, corneta, logro, anger;

    public void PlaySound(Sound soundToPlay)
    {
        switch (soundToPlay)
        {
            case Sound.effortUp:
                effortUp.PlayOneShot(effortUp.clip);
                break;
            case Sound.effortLoop:
                effortLoop.PlayOneShot(effortLoop.clip);
                break;
            case Sound.corneta:
                corneta.PlayOneShot(corneta.clip);
                break;
            case Sound.logro:
                logro.PlayOneShot(logro.clip);
                break;
            case Sound.anger:
                anger.PlayOneShot(anger.clip);
                break;
            default:
                break;
        }
    }
}