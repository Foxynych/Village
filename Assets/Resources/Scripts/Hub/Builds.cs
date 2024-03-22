using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using static EventAgregator;

public class Builds : MonoBehaviour
{
    public int lvl, maxLvl, soldPrice, upgradePrice, buyPrice, indexOfBuild, addGold, addManPower;
    public DateTimeOffset allRecrutingTime;
    public bool isRecruting;

    private Material[] oldMat;

    private void Start()
    {
        oldMat = gameObject.GetComponent<MeshRenderer>().materials;
    }

    private void OnTriggerStay(Collider other)
    {
        if (gameObject == hubManager.activeObj)
        {
            Material[] materials;
            buttons.inObj = true;

            materials = gameObject.GetComponent<MeshRenderer>().materials;

            for (int y = 0; y < materials.Length; y++)
                materials[y] = hubManager.achtung;

            gameObject.GetComponent<MeshRenderer>().materials = materials;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (gameObject == hubManager.activeObj)
        {
            buttons.inObj = false;

            gameObject.GetComponent<MeshRenderer>().materials = oldMat;
        }
    }

    private void OnMouseDown()
    {
        if (!(buttons.isPlace || icons.isActiveFunc))
        {
            icons.isActiveFunc = true;

            Vector3 delta = new(1.52f, 10.22f - gameObject.transform.position.y, -7.92f);

            hubManager.activeObj.transform.GetChild(0).gameObject.SetActive(false);

            hubManager.activeObj = gameObject;
            hubManager.cam.transform.position = gameObject.transform.position + delta;

            icons.buildsFuncIcons = gameObject.transform.GetChild(0).GetChild(1).gameObject;

            buttons.buildButton.transform.GetChild(0).gameObject.SetActive(false);
            gameObject.transform.GetChild(0).gameObject.SetActive(true);

            foreach (Button button in gameObject.transform.GetChild(0).GetComponentsInChildren<Button>())
                button.onClick.AddListener(icons.ButtonClick);

            foreach (Animator animator in icons.buildsFuncIcons.GetComponentsInChildren<Animator>())
                animator.SetTrigger("Play");

            buttons.buttons.transform.GetComponentInParent<Touches>().enabled = false;
        }
    }

    public IEnumerator Timer(DateTimeOffset allTime, int index, int count)
    {
        isRecruting = true;
        allRecrutingTime = allTime;

        if (allTime > DateTimeOffset.Now)
            yield return new WaitForSeconds(60f);

        else
        {
            isRecruting = false;

            Data.count[index] += count;

            Data.gameObjectsToSave[indexOfBuild].allRecrutingTime = 0;
            Data.gameObjectsToSave[indexOfBuild].index = 0;
            Data.gameObjectsToSave[indexOfBuild].count = 0;
            Data.gameObjectsToSave[indexOfBuild].isRecruting = false;
        }
    }

    public IEnumerator Gold(DateTimeOffset goldTime)
    {
        if (goldTime > DateTimeOffset.Now && !buttons.isPlace)
            yield return new WaitForSeconds(60f);

        else
        {
            Data.money += addGold;
            buttons.textMoney.text = Data.money.ToString();

            Data.gameObjectsToSave[indexOfBuild].allGoldTime = goldTime.ToUnixTimeSeconds();

            yield return Gold(DateTimeOffset.FromUnixTimeSeconds(DateTimeOffset.Now.ToUnixTimeSeconds() + 600));
        }
    }
}
