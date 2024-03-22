using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static EventAgregator;

public class Icons : MonoBehaviour
{
    public GameObject buildsFunc, buildsFuncIcons;
    public TextMeshProUGUI buyText, upText;
    public bool isActiveFunc;

    private List<RawImage> rawImages = new();
    private long addTime;
    private int buyPrice, upPrice, needManPower, index, count = 1;
    private Texture unit;
    private string fun;
    private Vector3 defPos;

    private void Awake()
    {
        icons = this;
    }

    public void ButtonClick()
    {
        Builds builds = hubManager.activeObj.GetComponent<Builds>();

        switch (EventSystem.current.currentSelectedGameObject.name)
        {
            case "UpgradeIcon":
                if (hubManager.activeObj.name[..hubManager.activeObj.name.IndexOf("_")] != "��" && hubManager.txLvl <= builds.lvl)
                    StartCoroutine(Achtung(buildsFuncIcons.transform.parent.gameObject, "������ �������� ������� \r\n ���� �������� ������"));

                else if (builds.lvl >= builds.maxLvl)
                    StartCoroutine(Achtung(buildsFuncIcons.transform.parent.gameObject, "��������� ������������ \r\n �������"));

                else
                {
                    TextMeshProUGUI upgradeText = buildsFunc.transform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>();

                    fun = "��������?";
                    buyPrice = builds.upgradePrice;

                    buildsFunc.transform.GetChild(0).gameObject.SetActive(true);

                    upgradeText.text = $"��� ����� ������ {buyPrice}\r\n{fun}";

                    hubManager.activeObj.transform.GetChild(0).gameObject.SetActive(false);
                }
                break;

            case "YesUpgrade":
                if (Data.money > builds.upgradePrice)
                {
                    if (hubManager.activeObj.name[..hubManager.activeObj.name.IndexOf("_")] == "��")
                        hubManager.txLvl++;

                    source.sampleSource.clip = source.build;
                    source.sampleSource.Play();

                    GameObject oldActiveObject = hubManager.activeObj;

                    string name = hubManager.activeObj.name[..hubManager.activeObj.name.IndexOf('_')];
                    builds.lvl += 1;

                    GameObject spawnObject = Resources.Load<GameObject>($"Models/{name}/{name}_{builds.lvl}");

                    hubManager.activeObj = Instantiate(spawnObject, new Vector3(oldActiveObject.transform.position.x, spawnObject.transform.position.y, oldActiveObject.transform.position.z), oldActiveObject.transform.rotation);

                    Data.money -= builds.upgradePrice;
                    buttons.textMoney.text = Data.money.ToString();

                    Data.gameObjectsToSave[builds.indexOfBuild].name = hubManager.activeObj.name;
                    Data.gameObjectsToSave[builds.indexOfBuild].lvl = builds.lvl;

                    buildsFunc.transform.GetChild(0).gameObject.SetActive(false);

                    Destroy(oldActiveObject);

                    isActiveFunc = false;
                    buttons.buttons.transform.GetComponentInParent<Touches>().enabled = true;
                }
                else
                    StartCoroutine(Achtung(buildsFunc.transform.GetChild(0).gameObject, "������������ �����"));
                break;

            case "NoUpgrade":
                buildsFunc.transform.GetChild(0).gameObject.SetActive(false);

                isActiveFunc = false;
                buttons.buttons.transform.GetComponentInParent<Touches>().enabled = true;
                break;

            case "SoldIcon":
                if (builds.soldPrice == 0)
                    StartCoroutine(Achtung(buildsFuncIcons.transform.parent.gameObject, "������ ������� ������� ������"));

                else
                {
                    TextMeshProUGUI soldText = buildsFunc.transform.GetChild(1).gameObject.GetComponentInChildren<TextMeshProUGUI>();

                    hubManager.activeObj.transform.GetChild(0).gameObject.SetActive(false);
                    buildsFunc.transform.GetChild(1).gameObject.SetActive(true);

                    soldText.text = $"�� �������, ��� ������ \r\n ������� �������� �� {builds.soldPrice}?";

                    hubManager.activeObj.transform.GetChild(0).gameObject.SetActive(false);
                }
                break;

            case "YesSold":
                source.sampleSource.clip = source.sold;
                source.sampleSource.Play();

                buildsFunc.transform.GetChild(1).gameObject.SetActive(false);

                Destroy(hubManager.activeObj);
                hubManager.activeObj = hubManager.TX;

                Data.gameObjectsToSave.RemoveAt(builds.indexOfBuild);

                Data.manPower -= hubManager.activeObj.GetComponent<Builds>().addManPower;
                Data.money += builds.soldPrice;
                buttons.textManPower.text = Data.manPower.ToString();
                buttons.textMoney.text = Data.money.ToString();

                isActiveFunc = false;
                buttons.buttons.transform.GetComponentInParent<Touches>().enabled = true;

                GC.Collect();
                break;

            case "NoSold":
                buildsFunc.transform.GetChild(1).gameObject.SetActive(false);

                isActiveFunc = false;
                buttons.buttons.transform.GetComponentInParent<Touches>().enabled = true;
                break;

            case "TranslateIcon":
                defPos = hubManager.cam.transform.position;

                hubManager.activeObj.transform.SetParent(hubManager.cam.transform);

                buildsFunc.transform.GetChild(2).gameObject.SetActive(true);
                hubManager.activeObj.transform.GetChild(0).gameObject.SetActive(false);

                buttons.buttons.transform.GetComponentInParent<Touches>().enabled = true;
                break;

            case "Tanslate������":
                hubManager.cam.transform.position = defPos;

                hubManager.activeObj.transform.SetParent(null);

                buildsFunc.transform.GetChild(2).gameObject.SetActive(false);

                isActiveFunc = false;
                break;

            case "Place":
                source.sampleSource.clip = source.build;
                source.sampleSource.Play();

                Data.GameObject newGameObject = new(hubManager.activeObj.name, hubManager.activeObj.GetComponent<Builds>().lvl, hubManager.activeObj.transform.position, hubManager.activeObj.transform.rotation);

                Data.gameObjectsToSave[builds.indexOfBuild] = newGameObject;

                hubManager.activeObj.transform.SetParent(null);

                buildsFunc.transform.GetChild(2).gameObject.SetActive(false);

                isActiveFunc = false;
                break;

            case "KzmFuncIcon":
                if (builds.isRecruting)
                    StartCoroutine(Achtung(hubManager.activeObj.transform.GetChild(0).gameObject, 
                        $"�� ����� �������� ��������\r\n {(builds.allRecrutingTime - DateTime.Now).Hours} ��� {(builds.allRecrutingTime - DateTime.Now).Minutes} �����"));

                else
                {
                    fun = "��������?";

                    buildsFunc.transform.GetChild(3).GetChild(0).GetChild(3).GetComponentInChildren<TextMeshProUGUI>().text = $"������ {Data.count[0]} ������";
                    buildsFunc.transform.GetChild(3).GetChild(0).GetChild(4).GetComponentInChildren<TextMeshProUGUI>().text = $"��� {Data.count[1]} ������";
                    buildsFunc.transform.GetChild(3).GetChild(0).GetChild(5).GetComponentInChildren<TextMeshProUGUI>().text = $"������ {Data.count[2]} ������";
                    buildsFunc.transform.GetChild(3).GetChild(0).GetChild(6).GetComponentInChildren<TextMeshProUGUI>().text = $"���������� {Data.count[3]} ������";

                    buildsFunc.transform.GetChild(3).GetChild(0).gameObject.SetActive(true);
                    hubManager.activeObj.transform.GetChild(0).gameObject.SetActive(false);
                }
                break;

            case "CloseKzm":
                
                buildsFunc.transform.GetChild(3).GetChild(0).gameObject.SetActive(false);

                isActiveFunc = false;
                buttons.buttons.transform.GetComponentInParent<Touches>().enabled = true;
                break;

            case "������":
                addTime = 600;
                needManPower = 20;
                buyPrice = 100 * (Data.lvlUnit[0] + 1);
                index = 0;

                buildsFunc.transform.GetChild(3).GetChild(0).gameObject.SetActive(false);
                buildsFunc.transform.GetChild(3).GetChild(1).gameObject.SetActive(true);

                buyText.text = $"��� ����� ������ {buyPrice} \r\n � {needManPower} {fun}";
                break;

            case "���":
                addTime = 1200;
                needManPower = 10;
                buyPrice = 250 * (Data.lvlUnit[1] + 1);
                index = 1;

                buildsFunc.transform.GetChild(3).GetChild(0).gameObject.SetActive(false);
                buildsFunc.transform.GetChild(3).GetChild(1).gameObject.SetActive(true);

                buyText.text = $"��� ����� ������ {buyPrice} \r\n � {needManPower} {fun}";
                break;

            case "������":
                addTime = 1200;
                needManPower = 10;
                buyPrice = 500 * (Data.lvlUnit[2] + 1);
                index = 2;

                buildsFunc.transform.GetChild(3).GetChild(0).gameObject.SetActive(false);
                buildsFunc.transform.GetChild(3).GetChild(1).gameObject.SetActive(true);

                buyText.text = $"��� ����� ������ {buyPrice} \r\n � {needManPower} {fun}";
                break;

            case "����������":
                addTime = 2400;
                needManPower = 5;
                buyPrice = 1000 * (Data.lvlUnit[3] + 1);
                index = 3;

                buildsFunc.transform.GetChild(3).GetChild(0).gameObject.SetActive(false);
                buildsFunc.transform.GetChild(3).GetChild(1).gameObject.SetActive(true);

                buyText.text = $"��� ����� ������ {buyPrice} \r\n � {needManPower} {fun}";
                break;

            case "YesBuy":
                if (Data.money >= buyPrice * count && Data.manPower >= needManPower * count)
                {
                    long time = DateTimeOffset.Now.ToUnixTimeSeconds() + addTime * count;

                    Data.gameObjectsToSave[builds.indexOfBuild].allRecrutingTime = time;
                    Data.gameObjectsToSave[builds.indexOfBuild].index = index;
                    Data.gameObjectsToSave[builds.indexOfBuild].count = count;
                    Data.gameObjectsToSave[builds.indexOfBuild].isRecruting = true;

                    buildsFunc.transform.GetChild(3).GetChild(1).gameObject.SetActive(false);

                    Data.money -= buyPrice * count;
                    Data.manPower -= needManPower * count;
                    buttons.textMoney.text = Data.money.ToString();
                    buttons.textManPower.text = Data.manPower.ToString();

                    isActiveFunc = false;
                    buttons.buttons.transform.GetComponentInParent<Touches>().enabled = true;

                    StartCoroutine(hubManager.activeObj.GetComponent<Builds>().Timer(DateTimeOffset.FromUnixTimeSeconds(time), index, count));
                }
                else
                    StartCoroutine(Achtung(buildsFunc.transform.GetChild(3).GetChild(1).gameObject, "������������ ��������"));
                break;
            case "NoBuy":
                buildsFunc.transform.GetChild(3).GetChild(1).gameObject.SetActive(false);

                isActiveFunc = false;
                buttons.buttons.transform.GetComponentInParent<Touches>().enabled = true;
                break;

            case "LgrFuncIcon":
                fun = "��������?";

                buildsFunc.transform.GetChild(4).GetChild(0).GetChild(3).GetComponentInChildren<TextMeshProUGUI>().text = $"������ {Data.lvlUnit[0]} �������";
                buildsFunc.transform.GetChild(4).GetChild(0).GetChild(4).GetComponentInChildren<TextMeshProUGUI>().text = $"��� {Data.lvlUnit[1]} �������";
                buildsFunc.transform.GetChild(4).GetChild(0).GetChild(5).GetComponentInChildren<TextMeshProUGUI>().text = $"������ {Data.lvlUnit[2]} �������";
                buildsFunc.transform.GetChild(4).GetChild(0).GetChild(6).GetComponentInChildren<TextMeshProUGUI>().text = $"���������� {Data.lvlUnit[3]} �������";

                buildsFunc.transform.GetChild(4).GetChild(0).gameObject.SetActive(true);
                hubManager.activeObj.transform.GetChild(0).gameObject.SetActive(false);
                break;

            case "CloseUp":
                
                buildsFunc.transform.GetChild(4).GetChild(0).gameObject.SetActive(false);

                isActiveFunc = false;
                buttons.buttons.transform.GetComponentInParent<Touches>().enabled = true;
                break;

            case "������Up":
                upPrice = 500 * (Data.lvlUnit[0] + 1);
                index = 0;

                if (Data.lvlUnit[index] >= hubManager.activeObj.GetComponent<Builds>().lvl)
                    StartCoroutine(Achtung(buildsFunc.transform.GetChild(4).GetChild(0).gameObject, "������ �������� ������� \r\n ���� ������"));

                else if (Data.lvlUnit[index] >= 3)
                    StartCoroutine(Achtung(buildsFunc.transform.GetChild(4).GetChild(0).gameObject, "��������� ������������ \r\n �������"));

                else
                {
                    rawImages.Add(buildsFunc.transform.GetChild(3).GetChild(0).GetChild(3).GetComponent<RawImage>());
                    rawImages.Add(buildsFunc.transform.GetChild(4).GetChild(0).GetChild(3).GetComponent<RawImage>());

                    unit = Resources.Load<Texture>("Sprites/Units/������/������" + (Data.lvlUnit[index] + 1));

                    buildsFunc.transform.GetChild(4).GetChild(0).gameObject.SetActive(false);
                    buildsFunc.transform.GetChild(4).GetChild(1).gameObject.SetActive(true);

                    upText.text = $"��� ����� ������ {upPrice} \r\n {fun}";
                }
                break;

            case "���Up":
                upPrice = 750 * (Data.lvlUnit[1] + 1);
                index = 1;

                if (Data.lvlUnit[index] >= hubManager.activeObj.GetComponent<Builds>().lvl)
                    StartCoroutine(Achtung(buildsFunc.transform.GetChild(4).GetChild(0).gameObject, "������ �������� ������� \r\n ���� ������"));

                else if (Data.lvlUnit[index] >= 3)
                    StartCoroutine(Achtung(buildsFunc.transform.GetChild(4).GetChild(0).gameObject, "��������� ������������ \r\n �������"));

                else
                {
                    rawImages.Add(buildsFunc.transform.GetChild(3).GetChild(0).GetChild(4).GetComponent<RawImage>());
                    rawImages.Add(buildsFunc.transform.GetChild(4).GetChild(0).GetChild(4).GetComponent<RawImage>());

                    unit = Resources.Load<Texture>("Sprites/Units/���/���" + (Data.lvlUnit[index] + 1));

                    buildsFunc.transform.GetChild(4).GetChild(0).gameObject.SetActive(false);
                    buildsFunc.transform.GetChild(4).GetChild(1).gameObject.SetActive(true);

                    upText.text = $"��� ����� ������ {upPrice} \r\n {fun}";
                }
                break;

            case "������Up":
                upPrice = 1000 * (Data.lvlUnit[2] + 1);
                index = 2;

                if (Data.lvlUnit[index] >= hubManager.activeObj.GetComponent<Builds>().lvl)
                    StartCoroutine(Achtung(buildsFunc.transform.GetChild(4).GetChild(0).gameObject, "������ �������� ������� \r\n ���� ������"));

                else if (Data.lvlUnit[index] >= 3)
                    StartCoroutine(Achtung(buildsFunc.transform.GetChild(4).GetChild(0).gameObject, "��������� ������������ \r\n �������"));

                else
                {
                    rawImages.Add(buildsFunc.transform.GetChild(3).GetChild(0).GetChild(5).GetComponent<RawImage>());
                    rawImages.Add(buildsFunc.transform.GetChild(4).GetChild(0).GetChild(5).GetComponent<RawImage>());

                    unit = Resources.Load<Texture>("Sprites/Units/������/������" + (Data.lvlUnit[index] + 1));

                    buildsFunc.transform.GetChild(4).GetChild(0).gameObject.SetActive(false);
                    buildsFunc.transform.GetChild(4).GetChild(1).gameObject.SetActive(true);

                    upText.text = $"��� ����� ������ {upPrice} \r\n {fun}";
                }
                break;

            case "����������Up":
                upPrice = 1500 * (Data.lvlUnit[3] + 1);
                index = 3;

                if (Data.lvlUnit[index] >= hubManager.activeObj.GetComponent<Builds>().lvl)
                    StartCoroutine(Achtung(buildsFunc.transform.GetChild(4).GetChild(0).gameObject, "������ �������� ������� \r\n ���� ������"));

                else if (Data.lvlUnit[index] >= 3)
                    StartCoroutine(Achtung(buildsFunc.transform.GetChild(4).GetChild(0).gameObject, "��������� ������������ \r\n �������"));

                else
                {
                    rawImages.Add(buildsFunc.transform.GetChild(3).GetChild(0).GetChild(6).GetComponent<RawImage>());
                    rawImages.Add(buildsFunc.transform.GetChild(4).GetChild(0).GetChild(6).GetComponent<RawImage>());

                    unit = Resources.Load<Texture>("Sprites/Units/����������/����������" + (Data.lvlUnit[index] + 1));

                    buildsFunc.transform.GetChild(4).GetChild(0).gameObject.SetActive(false);
                    buildsFunc.transform.GetChild(4).GetChild(1).gameObject.SetActive(true);

                    upText.text = $"��� ����� ������ {upPrice} \r\n {fun}";
                }
                break;

            case "YesUp":
                if (Data.money >= upPrice)
                {
                    foreach (RawImage rawImage in rawImages)
                        rawImage.texture = unit;

                    Data.lvlUnit[index] += 1;

                    buildsFunc.transform.GetChild(4).GetChild(1).gameObject.SetActive(false);

                    Data.money -= upPrice;
                    buttons.textMoney.text = Data.money.ToString();

                    isActiveFunc = false;
                    buttons.buttons.transform.GetComponentInParent<Touches>().enabled = true;
                }
                else
                    StartCoroutine(Achtung(buildsFunc.transform.GetChild(4).GetChild(1).gameObject, "������������ �����"));

                break;

            case "NoUp":
                buildsFunc.transform.GetChild(4).GetChild(1).gameObject.SetActive(true);

                isActiveFunc = false;
                buttons.buttons.transform.GetComponentInParent<Touches>().enabled = true;
                break;

            default:
                hubManager.activeObj.transform.GetChild(0).gameObject.SetActive(false);

                isActiveFunc = false;
                buttons.buttons.transform.GetComponentInParent<Touches>().enabled = true;
                break;
        }
    }

    public void InputText(GameObject obj)
    {
        TMP_InputField inputField = obj.transform.GetChild(4).GetComponent<TMP_InputField>();
        TextMeshProUGUI text = obj.transform.GetChild(1).GetComponent<TextMeshProUGUI>();

        count = Convert.ToInt32(inputField.text);
        text.text = $"��� ����� ������ {buyPrice * count} \r\n � {needManPower * count} {fun}";
    }

    IEnumerator Achtung(GameObject currentObject, string s)
    {
        currentObject.SetActive(false);
        buttons.achtung.SetActive(true);

        buttons.achtung.GetComponentInChildren<TextMeshProUGUI>().text = s;
        yield return new WaitForSeconds(1.5f);

        buttons.achtung.SetActive(false);
        currentObject.SetActive(true);
    }
}
