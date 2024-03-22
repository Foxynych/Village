using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static EventAgregator;

public class HubManager : MonoBehaviour
{
    public int txLvl;
    public GameObject transParent, cam, TX;
    public Material achtung;
    public RawImage knightIcon, wizardIcon, archerIcon, katapultaIcon, knightIconUp, wizardIconUp, archerIconUp, katapultaIconUp;

    [HideInInspector] public GameObject activeObj;

    private void Awake()
    {
        if (GameObject.FindGameObjectsWithTag("GameController").Length == 0)
        {
            DontDestroyOnLoad(Instantiate(Resources.Load<GameObject>("DDOL/DontDestroyOnLoad")));
        }

        hubManager = this;

        knightIconUp.texture = knightIcon.texture = Resources.Load<Texture>($"Sprites/Units/Рыцарь/Рыцарь{Data.lvlUnit[0]}");
        wizardIconUp.texture = wizardIcon.texture = Resources.Load<Texture>($"Sprites/Units/Маг/Маг{Data.lvlUnit[1]}");
        archerIconUp.texture = archerIcon.texture = Resources.Load<Texture>($"Sprites/Units/Лучник/Лучник{Data.lvlUnit[2]}");
        katapultaIconUp.texture = katapultaIcon.texture = Resources.Load<Texture>($"Sprites/Units/Катапульта/Катапульта{Data.lvlUnit[3]}");
    }
}