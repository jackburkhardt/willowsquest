using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    private AudioSource loop;
    private AudioSource sting;
    private AudioSource click;
    private AudioSource next;

    public AudioClip bgclip;
    public AudioClip battleclip;
    public AudioClip finalbattleclip;

    public AudioClip encounter;
    public AudioClip win;

    void Start()
    {
        AudioSource[] srcs = GetComponents<AudioSource>();
        loop = srcs[0];
        loop.Play();
        sting = srcs[1];
        click = srcs[2];
        next = srcs[3];
    }

    public void Encounter()
    {
        loop.Stop();
        sting.clip = encounter;
        sting.Play();
    }

    public void StartBattleMusic()
    {
        loop.clip = battleclip;
        loop.Play();
    }

    public void StartFinalBattle()
    {
        loop.clip = finalbattleclip;
        loop.Play();
    }

    public void StopBattleMusic()
    {
        loop.Stop();
        loop.clip = bgclip;
        loop.PlayDelayed(win.length - 1);
    }

    public void Victory()
    {
        sting.clip = win;
        sting.Play();
    }

    public void ButtonClick()
    {
        click.Play();
    }

    public void Next()
    {
        next.Play();
    }
}
	