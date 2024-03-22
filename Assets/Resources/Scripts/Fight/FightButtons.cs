using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using static EventAgregator;

public class FightButtons : MonoBehaviour
{
    public bool isPLace, isDelete, isPLacePhase = true;
    public GameObject placePhase, fightPhase, win, lose, setting;
    private int dist, oldCount;

    private void Awake()
    {
        fightButtons = this;
    }

    public void ButonClick()
    {
        switch (EventSystem.current.currentSelectedGameObject.name)
        {
            case "Setting":
                setting.transform.GetChild(0).gameObject.SetActive(true);

                if (source.listener.enabled)
                    setting.transform.GetChild(0).GetChild(6).gameObject.SetActive(true);

                else
                    setting.transform.GetChild(0).GetChild(6).gameObject.SetActive(false);
                break;

            case "Close":
                setting.transform.GetChild(0).gameObject.SetActive(false);
                break;

            case "MusicOn":
                source.listener.enabled = false;
                setting.transform.GetChild(0).GetChild(6).gameObject.SetActive(false);
                break;

            case "MusicOff":
                source.listener.enabled = true;
                setting.transform.GetChild(0).GetChild(6).gameObject.SetActive(true);
                break;

            case "Рыцарь":
                if (Data.count[0] > 0)
                {
                    ChooseUnit("Рыцарь", 0, 48, 25);
                    Alpha(0.35f, dist, spawnPlace.cells);
                    Transit(placePhase.transform.GetChild(0).gameObject, placePhase.transform.GetChild(1).gameObject);
                }
                else
                    StartCoroutine(Achtung(placePhase.transform.GetChild(2).gameObject, "юниты закончились"));
                break;

            case "Маг":
                if (Data.count[1] > 0)
                {
                    ChooseUnit("Маг", 1, 36, 10);
                    Alpha(0.35f, dist, spawnPlace.cells);
                    Transit(placePhase.transform.GetChild(0).gameObject, placePhase.transform.GetChild(1).gameObject);
                }
                else
                    StartCoroutine(Achtung(placePhase.transform.GetChild(2).gameObject, "юниты закончились"));
                break;

            case "Лучник":
                if (Data.count[2] > 0)
                {
                    ChooseUnit("Лучник", 2, 24, 10);
                    Alpha(0.35f, dist, spawnPlace.cells);
                    Transit(placePhase.transform.GetChild(0).gameObject, placePhase.transform.GetChild(1).gameObject);
                }
                else
                    StartCoroutine(Achtung(placePhase.transform.GetChild(2).gameObject, "юниты закончились"));
                break;

            case "Катапульта":
                if (Data.count[3] > 0)
                {
                    ChooseUnit("Катапульта", 3, 12, 5);
                    Alpha(0.35f, dist, spawnPlace.cells);
                    Transit(placePhase.transform.GetChild(0).gameObject, placePhase.transform.GetChild(1).gameObject);
                }
                else
                    StartCoroutine(Achtung(placePhase.transform.GetChild(2).gameObject, "юниты закончились"));
                break;

            case "YesPlace":
                isPLace = false;

                for (int i = 0; i < spawnPlace.placedUnits.Count; i++)
                {
                    spawnPlace.allUnits.Add(spawnPlace.placedUnits[i]);
                    spawnPlace.allies.Add(spawnPlace.placedUnits[i]);

                    spawnPlace.placedUnits[i].GetComponentInChildren<Outline>().enabled = false;
                    spawnPlace.placedUnits.RemoveAt(i);
                    i--;
                }

                Alpha(1f, dist, spawnPlace.cells);
                Transit(placePhase.transform.GetChild(1).gameObject, placePhase.transform.GetChild(0).gameObject);
                break;

            case "NoPlace":
                Data.count[spawnPlace.indexPlace] = oldCount;
                isPLace = false;

                for (int i = 0; i < spawnPlace.placedUnits.Count; i++)
                {
                    Destroy(spawnPlace.placedUnits[i]);
                    spawnPlace.placedUnits.RemoveAt(i);
                    i--;
                }

                Alpha(1f, dist, spawnPlace.cells);
                Transit(placePhase.transform.GetChild(1).gameObject, placePhase.transform.GetChild(0).gameObject);
                break;

            case "Delete":
                isDelete = true;

                Transit(placePhase.transform.GetChild(0).gameObject, placePhase.transform.GetChild(3).gameObject);
                break;

            case "YesDelete":
                isDelete = false;

                for(int i = 0; i < spawnPlace.deletedUnits.Count; i++)
                {
                    Destroy(spawnPlace.deletedUnits[i]);

                    spawnPlace.allUnits.Remove(spawnPlace.deletedUnits[i]);
                    spawnPlace.allies.Remove(spawnPlace.deletedUnits[i]);
                    spawnPlace.deletedUnits.RemoveAt(i);
                    i--;
                }

                for (int i = 0; i < 4; i++)
                {
                    Data.count[i] += spawnPlace.newCount[i];
                    spawnPlace.newCount[i] = 0;
                }

                Transit(placePhase.transform.GetChild(3).gameObject, placePhase.transform.GetChild(0).gameObject);
                break;

            case "NoDelete":
                isDelete = false;

                for (int i = 0; i < spawnPlace.deletedUnits.Count; i++)
                {
                    spawnPlace.deletedUnits[i].GetComponentInChildren<Outline>().enabled = false;
                    spawnPlace.deletedUnits.RemoveAt(i);
                    i--;
                }

                Transit(placePhase.transform.GetChild(3).gameObject, placePhase.transform.GetChild(0).gameObject);
                break;

            case "Yes":
                if (spawnPlace.allies.Count != 0)
                {
                    isPLacePhase = false;
                    spawnPlace.allUnits.Reverse();

                    Destroy(placePhase);
                }
                else
                    StartCoroutine(Achtung(placePhase.transform.GetChild(2).gameObject, "нельзя начать без юнитов"));
                break;

            case "Home":
                Data.count = spawnPlace.diedUnits;

                if (win.activeSelf)
                    Data.money += SpawnPlace.prize;

                source.mainSource.Stop();
                source.mainSource.clip = source.hubTheme;
                source.mainSource.Play();

                SceneManager.LoadScene(0);
                break;

            case "Retry":
                data.Load(Resources.Load<TextAsset>("Json" + SpawnPlace.path).text);
                break;

            case "Continue":
                Data.count = spawnPlace.diedUnits;
                Data.money += SpawnPlace.prize;

                SceneManager.LoadScene(1);
                break;
        }
    }

    private void ChooseUnit(string name, int indexPlace, int distSpawn, int maxCount)
    {
        spawnPlace.indexPlace = indexPlace;
        spawnPlace.maxCount = maxCount;
        spawnPlace.chooseUnit = Resources.Load<GameObject>($"Models/{name}/{name}_{Data.lvlUnit[spawnPlace.indexPlace]}");
        oldCount = Data.count[spawnPlace.indexPlace];
        dist = distSpawn;
        isPLace = true;

        placePhase.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = "осталось юнитов: " + Data.count[spawnPlace.indexPlace].ToString();
    }

    private void Transit(GameObject obj1, GameObject obj2)
    {
        obj1.SetActive(false);
        obj2.SetActive(true);
    }

    private void Alpha(float alpha, int dist, GameObject  cells)
    {
        for (int i = 0; i < cells.transform.childCount - dist; i++)
        {
            Color newColor = cells.transform.GetChild(i).GetComponentInChildren<MeshRenderer>().material.color;

            newColor.a = alpha;
            cells.transform.GetChild(i).GetComponentInChildren<MeshRenderer>().material.color = newColor;
        }
    }

    public IEnumerator Achtung(GameObject obj, string s)
    {
        obj.GetComponentInChildren<TextMeshProUGUI>().text = s;
        obj.SetActive(true);
        yield return new WaitForSeconds(1.5f);

        obj.SetActive(false);
    }
}