using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EventAgregator;

public class Source: MonoBehaviour
{
    public AudioListener listener;
    public AudioSource sampleSource, mainSource;
    public AudioClip hubTheme, fightTheme, build, sold;

    private void Awake()
    {
        source = this;
    }
}
