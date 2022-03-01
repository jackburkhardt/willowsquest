using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    private AudioSource audiosrc;

    public AudioClip bgclip;
    public AudioClip battleclip;

    void Start()
    {
        audiosrc = FindObjectOfType<AudioSource>();
        audiosrc.Play();
    }

    public void StartBattleMusic()
    {
        audiosrc.Stop();
        audiosrc.clip = battleclip;
        audiosrc.Play();
    }

    public void StopBattleMusic()
    {
        audiosrc.Stop();
        audiosrc.clip = bgclip;
        audiosrc.Play();
    }
}
	