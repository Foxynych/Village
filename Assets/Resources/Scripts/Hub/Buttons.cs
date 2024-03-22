using System;
using System.Collections;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using static EventAgregator;

public class ButtonsClasss : MonoBehaviour
{
    public GameObject buttons, buildButton, setting, achtung;
    public TextMeshProUGUI textMoney, textManPower;

    [HideInInspector] public bool inObj, isPlace;

    private void Awake()
    {
        EventAgregator.buttons = this;
    }

    private void Start()
    {
        data.Load(File.ReadAllText(Application.persistentDataPath + "/Data.json"));

        textMoney.text = Data.money.ToString();
        textManPower.text = Data.manPower.ToString();
    }

    public void ButtonClick()
    {
        switch (EventSystem.current.currentSelectedGameObject.name)
        {
            case "Attack":
                data.Save();

                SceneManager.LoadScene(1);
                break;

            case "Build":
                buttons.transform.GetChild(2).gameObject.SetActive(false);
                buildButton.transform.GetChild(0).gameObject.SetActive(true);

                hubManager.activeObj.transform.GetChild(0).gameObject.SetActive(false);
                break;

            case "Setting":
                hubManager.activeObj.transform.GetChild(0).gameObject.SetActive(false);

                icons.isActiveFunc = true;

                setting.transform.GetChild(0).gameObject.SetActive(true);

                if (source.listener.enabled)
                    setting.transform.GetChild(0).GetChild(4).gameObject.SetActive(true);

                else
                    setting.transform.GetChild(0).GetChild(4).gameObject.SetActive(false);
                break;

            case "Close":
                icons.isActiveFunc = false;

                setting.transform.GetChild(0).gameObject.SetActive(false);
                break;

            case "MusicOn":
                source.listener.enabled = false;
                setting.transform.GetChild(0).GetChild(4).gameObject.SetActive(false);
                break;

            case "MusicOff":
                source.listener.enabled = true;
                setting.transform.GetChild(0).GetChild(4).gameObject.SetActive(true);
                break;

            case "Back":
                buttons.transform.GetChild(2).gameObject.SetActive(true);
                buildButton.transform.GetChild(0).gameObject.SetActive(false);
                break;

            case "Казарма":
                Spawn(Resources.Load<GameObject>("Models/Казарма/Казарма_1"), hubManager.transParent);

                isPlace = true;
                break;

            case "Лагерь":
                Spawn(Resources.Load<GameObject>("Models/Лагерь/Лагерь_1"), hubManager.transParent);

                isPlace = true;
                break;

            case "Шахта":
                Spawn(Resources.Load<GameObject>("Models/Шахта/Шахта_1"), hubManager.transParent);

                isPlace = true;
                break;

            case "Халупа":
                Spawn(Resources.Load<GameObject>("Models/Дом/Дом_1"), hubManager.transParent);

                isPlace = true;
                break;

            case "Right": hubManager.activeObj.transform.Rotate(0f, 0f, 90f); break;

            case "Left": hubManager.activeObj.transform.Rotate(0f, 0f, -90f); break;

            case "Построить":
                if (Data.money >= hubManager.activeObj.GetComponent<Builds>().buyPrice && !inObj)
                {
                    source.sampleSource.clip = source.build;
                    source.sampleSource.Play();

                    buildButton.transform.GetChild(1).gameObject.SetActive(false);
                    buttons.transform.GetChild(2).gameObject.SetActive(true);

                    hubManager.activeObj.GetComponent<Outline>().enabled = false;

                    Data.manPower += hubManager.activeObj.GetComponent<Builds>().addManPower;
                    Data.money -= hubManager.activeObj.GetComponent<Builds>().buyPrice;

                    textManPower.text = Data.manPower.ToString();
                    textMoney.text = Data.money.ToString();

                    hubManager.activeObj.transform.parent = null;
                    isPlace = false;

                    Data.GameObject gameObject1 = new(hubManager.activeObj.name, 1, hubManager.activeObj.transform.position, hubManager.activeObj.transform.rotation);
                    Data.gameObjectsToSave.Add(gameObject1);

                    hubManager.activeObj.GetComponent<Builds>().indexOfBuild = Data.gameObjectsToSave.IndexOf(gameObject1);
                }
                else
                    StartCoroutine(BuildError());
                break;

            case "Отмена":
                Destroy(hubManager.activeObj);

                hubManager.activeObj = hubManager.TX;

                buildButton.transform.GetChild(1).gameObject.SetActive(false);
                buttons.transform.GetChild(2).gameObject.SetActive(true);

                isPlace = false;

                GC.Collect();
                break;
        }
            
    }

    private void Spawn(GameObject gameObj, GameObject parent)
    {
        hubManager.activeObj = Instantiate(gameObj, parent.transform);

        hubManager.activeObj.GetComponent<Outline>().enabled = true;
        hubManager.activeObj.GetComponent<Builds>().indexOfBuild = Data.gameObjectsToSave.Count;

        buildButton.transform.GetChild(0).gameObject.SetActive(false);
        buildButton.transform.GetChild(1).gameObject.SetActive(true);
    }

    IEnumerator BuildError()
    {
        achtung.SetActive(true);

        if (inObj)
            achtung.GetComponentInChildren<TextMeshProUGUI>().text = "объект невозможно\r\nпоставить";
        else
            achtung.GetComponentInChildren<TextMeshProUGUI>().text = "НЕДОСТАТОЧНО РЕСУРСОВ";
        yield return new WaitForSeconds(1.5f);

        achtung.SetActive(false);
    }
}