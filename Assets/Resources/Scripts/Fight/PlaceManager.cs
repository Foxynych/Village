using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static EventAgregator;

public class PLaceManager : MonoBehaviour
{
    public GameObject unit;
    public int W, H;
    public bool isEnemyPlace, isAllyPlace;

    private List<GameObject> splash = new(), atachedUnits = new();
    private bool iskatAtack;

    private void OnTriggerEnter(Collider other)
    {
        unit = other.gameObject;

        if (other.gameObject.CompareTag("Ally"))
            isAllyPlace = true;

        else if (other.gameObject.CompareTag("Enemy"))
            isEnemyPlace = true;
    }

    private void OnTriggerExit(Collider other)
    {
        unit = null;

        if (other.gameObject.CompareTag("Ally"))
            isAllyPlace = false;

        else if (other.gameObject.CompareTag("Enemy"))
            isEnemyPlace = false;
    }

    private void OnMouseUp()
    {
        if (fightButtons.isPLace && !fightButtons.isDelete)
        {
            if (gameObject.GetComponentInChildren<MeshRenderer>().material.color.a == 1f && Data.count[spawnPlace.indexPlace] > 0 && !(isEnemyPlace && isAllyPlace))
            {
                GameObject placedUnit = Instantiate(spawnPlace.chooseUnit);

                placedUnit.transform.position = new Vector3(gameObject.transform.localPosition.x, spawnPlace.chooseUnit.transform.position.y, gameObject.transform.localPosition.z);

                placedUnit.GetComponentInChildren<Outline>().enabled = true;
                placedUnit.GetComponentInChildren<Outline>().OutlineColor = new Color(255f, 255f, 255f);

                if (Data.count[spawnPlace.indexPlace] >= spawnPlace.maxCount)
                    Count(spawnPlace.maxCount, placedUnit);
                else
                    Count(Data.count[spawnPlace.indexPlace], placedUnit);

                spawnPlace.placedUnits.Add(placedUnit);
            }

            else if (Data.count[spawnPlace.indexPlace] == 0)
                StartCoroutine(fightButtons.Achtung(fightButtons.placePhase.transform.GetChild(2).gameObject, "юниты закончились"));

            else if (gameObject.GetComponentInChildren<MeshRenderer>().material.color.a != 1f || isAllyPlace || isEnemyPlace)
                StartCoroutine(fightButtons.Achtung(fightButtons.placePhase.transform.GetChild(2).gameObject, "нельзя поставить здесь"));
        }

        else if (iskatAtack)
        {
            iskatAtack = false;
            spawnPlace.isMove = true;

            spawnPlace.chooseUnit.GetComponentInChildren<Outline>().enabled = false;
            spawnPlace.chooseUnit.GetComponentInChildren<Animator>().SetTrigger("Start");

            source.sampleSource.clip = spawnPlace.chooseUnit.GetComponent<Units>().atackSample;
            source.sampleSource.Play();

            StartCoroutine(spawnPlace.chooseUnit.GetComponent<Units>().Atack(gameObject.transform.localPosition, spawnPlace.chooseUnit, atachedUnits));
        }

        else if (!spawnPlace.isMove && spawnPlace.isChoose && gameObject.GetComponentInChildren<MeshRenderer>().material.color.a == 1f)
        {
            spawnPlace.isMove = true;

            Vector3 speed = gameObject.transform.localPosition - spawnPlace.chooseUnit.transform.position;

            spawnPlace.chooseUnit.GetComponentInChildren<Outline>().enabled = false;
            spawnPlace.chooseUnit.GetComponentInChildren<Animator>().SetTrigger("Start");

            source.sampleSource.clip = spawnPlace.chooseUnit.GetComponent<Units>().walkSample;
            source.sampleSource.Play();

            StartCoroutine(spawnPlace.chooseUnit.GetComponent<Units>().Walk(spawnPlace.chooseUnit, gameObject.transform.localPosition, speed));
        }
    }

    private void OnMouseEnter()
    {
        if (!fightButtons.isPLacePhase && !spawnPlace.isMove && spawnPlace.chooseUnit.name[..spawnPlace.chooseUnit.name.IndexOf("_")] == "Катапульта")
        {
            if (W < spawnPlace.chooseUnit.GetComponent<Units>().W - 3)
            {
                Units units = spawnPlace.chooseUnit.GetComponent<Units>();
                int distSplashW = units.distAtackW, index = 0;

                if (W - distSplashW < 0)
                {
                    index = distSplashW - W;
                    distSplashW = W;
                }

                for (int i = distSplashW; i >= -units.distAtackW; i--)
                {
                    int minH = Math.Max(H - units.heightSplash[index], 0), maxH = Math.Min(H + units.heightSplash[index], 11);

                    for (int h = minH; h <= maxH; h++)
                    {
                        if (spawnPlace.cellsArr[W - i, h].GetComponentInChildren<MeshRenderer>().material.color.a != 1f)
                        {
                            splash.Add(spawnPlace.cellsArr[W - i, h]);

                            Alpha(1f, spawnPlace.cellsArr[W - i, h]);

                            if (spawnPlace.cellsArr[W - i, h].GetComponent<PLaceManager>().isEnemyPlace)
                                atachedUnits.Add(spawnPlace.cellsArr[W - i, h].GetComponent<PLaceManager>().unit);
                        }
                    }

                    index++;
                }

                iskatAtack = true;
            }
        }
    }

    private void OnMouseExit()
    {
        if (iskatAtack)
            foreach (GameObject cell in splash)
                Alpha(0.35f, cell);

        iskatAtack = false;
    }

    private void Count(int count, GameObject placeUnit)
    {
        placeUnit.transform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>().text = count.ToString();
        Data.count[spawnPlace.indexPlace] -= count;
        fightButtons.placePhase.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = "осталось юнитов: " + Data.count[spawnPlace.indexPlace].ToString();
    }

    private void Alpha(float alpha, GameObject obj)
    {
        Color newColor = obj.GetComponentInChildren<MeshRenderer>().material.color;

        newColor.a = alpha;
        obj.GetComponentInChildren<MeshRenderer>().material.color = newColor;
    }
}