using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static EventAgregator;

public class Units : MonoBehaviour
{
    public AudioClip walkSample, atackSample;
    public int lvl, distAtackW, distWalkW, minDmg, maxDmg, addResist, addEndOfResist;
    public float timeOfAnim;
    public List<int> heightWalk = new(), heightAtack = new(), heightSplash = new();
    public bool isRange;

    [HideInInspector] public int H, W, resist, endOfResist;

    private int index;
    private Func<GameObject, int, int, int, List<int>, GameObject> AtachedUnit = null;
    private Func<GameObject, int, int, int, List<int>, List<int>, (List<GameObject>, Vector3)> AtachedUnits = null;
    private Func<GameObject, int, int, int, List<int>, bool> IsAtack = null;
    private bool isHold = true, isEndTurn, isEndOfAnim;


    private void Awake()
    {
        switch (gameObject.name[..gameObject.name.IndexOf("_")])
        {
            case "ќрк–ыцарь":
                AtachedUnit = OrkKnight.AtachedUnits;
                IsAtack = OrkKnight.IsAtack;

                index = 0;
                break;

            case "ќркћаг":
                AtachedUnit = OrkWizard.AtachedUnits;
                IsAtack = OrkWizard.IsAtack;

                index = 1;
                break;

            case "ќркЋучник":
                AtachedUnit = OrkAracher.AtachedUnits;
                IsAtack = OrkAracher.IsAtack;

                index = 2;
                break;

            case "ќрк атапульта":
                AtachedUnits = OrkKatapulta.AtachedUnits;
                IsAtack = OrkKatapulta.IsAtack;

                index = 3;
                break;
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        W = other.GetComponent<PLaceManager>().W;
        H = other.GetComponent<PLaceManager>().H;
    }

    private void OnMouseUp()
    {
        if (fightButtons.isDelete)
        {
            spawnPlace.deletedUnits.Add(gameObject);

            gameObject.GetComponentInChildren<Outline>().enabled = true;
            gameObject.GetComponentInChildren<Outline>().OutlineColor = new Color(255f, 0f, 0f);
        }

        else if(!(gameObject.CompareTag("Enemy") || fightButtons.isPLacePhase || spawnPlace.isMove || isEndTurn))
        {
            spawnPlace.isChoose = true;

            int height = heightWalk[0], weight = distWalkW, index = 0;

            if (spawnPlace.chooseUnit != gameObject)
                spawnPlace.chooseUnit.GetComponentInChildren<Outline>().enabled = false;

            spawnPlace.chooseUnit = gameObject;
            gameObject.GetComponentInChildren<Outline>().enabled = true;

            if (W - weight < 0)
            {
                height = weight - W;
                weight -= height;
                index = heightWalk.IndexOf(height);
            }

            for (int w = 0; w < 15; w++)
            {
                for (int h = 0; h < 12; h++)
                {
                    if (spawnPlace.cellsArr[w, h] == spawnPlace.cellsArr[Math.Min(14, W - weight), Math.Max(0, H - height)] && weight >= -distWalkW)
                    {
                        int minH = Math.Max(0, H - height), maxH = Math.Min(11, H + height);

                        for (int i = minH; i <= maxH; i++)
                        {
                            Alpha(1f, spawnPlace.cellsArr[w, h]);
                            h++;
                        }

                        index++;
                        height = heightWalk[index];

                        weight--;

                        if (h < 12)
                            Alpha(0.35f, spawnPlace.cellsArr[w, h]);
                    }
                    else if (spawnPlace.cellsArr[w, h].GetComponent<PLaceManager>().isEnemyPlace && Math.Abs(W - w) < distAtackW)
                        continue;

                    else
                        Alpha(0.35f, spawnPlace.cellsArr[w, h]);
                }
            }
        }

        else if(gameObject.CompareTag("Enemy") && !(spawnPlace.isMove || fightButtons.isPLacePhase))
        {
            if (spawnPlace.cellsArr[W, H].GetComponentInChildren<MeshRenderer>().material.color.a == 1f)
            {
                Vector3 needPosition = NeedPosition(spawnPlace.chooseUnit, W, H), speed;

                if (!isHold || spawnPlace.chooseUnit.GetComponent<Units>().isRange)
                {
                    speed = needPosition - spawnPlace.chooseUnit.transform.position;

                    spawnPlace.chooseUnit.GetComponentInChildren<Outline>().enabled = false;
                    spawnPlace.chooseUnit.GetComponentInChildren<Animator>().SetTrigger("Start");
                    StartCoroutine(spawnPlace.chooseUnit.GetComponent<Units>().Atack(needPosition, spawnPlace.chooseUnit, gameObject, speed));
                }
                else
                    StartCoroutine(fightButtons.Achtung(fightButtons.fightPhase.transform.GetChild(0).gameObject, "нельз€ атаковать здесь"));
            }
            else
            {
                for (int x = 0; x < spawnPlace.cells.transform.childCount; x++)
                    Alpha(1f, spawnPlace.cells.transform.GetChild(x).gameObject);

                spawnPlace.chooseUnit.GetComponentInChildren<Outline>().enabled = false;
                spawnPlace.isChoose = false;

                StartCoroutine(fightButtons.Achtung(fightButtons.fightPhase.transform.GetChild(0).gameObject, "нельз€ атаковать здесь"));
            }
        }
    }

    private void EnemyTurn()
    {
        GameObject atachedUnit;
        Vector3 speed, needPosition;

        bool isAtack = IsAtack(gameObject, distAtackW, H, W, heightAtack);
        
        if (isAtack)
        {
            if (AtachedUnit != null)
            {
                atachedUnit = AtachedUnit(gameObject, distAtackW, H, W, heightAtack);

                if (isRange)
                    needPosition = atachedUnit.transform.position;

                else
                    needPosition = NeedPosition(gameObject, atachedUnit.GetComponent<Units>().W, atachedUnit.GetComponent<Units>().H);

                speed = needPosition - gameObject.transform.position;
                gameObject.GetComponentInChildren<Animator>().SetTrigger("Start");

                source.sampleSource.clip = atackSample;
                source.sampleSource.Play();

                StartCoroutine(Atack(needPosition, gameObject, atachedUnit, speed));
            }

            else
            {
                (List<GameObject> atachedUnits, Vector3 targetPos) = AtachedUnits(gameObject, distAtackW, H, W, heightAtack, heightSplash);

                gameObject.GetComponentInChildren<Animator>().SetTrigger("Start");

                source.sampleSource.clip = atackSample;
                source.sampleSource.Play();

                StartCoroutine(Atack(targetPos, gameObject, atachedUnits));
            }
        }

        else
        {
            if (gameObject.name[..gameObject.name.IndexOf("_")] != "ќркћаг")
            {
                atachedUnit = spawnPlace.allies[0];

                foreach (GameObject ally in spawnPlace.allies)
                    if (Vector3.Distance(ally.transform.position, gameObject.transform.position) < Vector3.Distance(atachedUnit.transform.position, gameObject.transform.position))
                        atachedUnit = ally;
            }

            else
            {
                atachedUnit = spawnPlace.enemies[0];

                foreach (GameObject enemy in spawnPlace.enemies)
                    if (Vector3.Distance(enemy.transform.position, gameObject.transform.position) < Vector3.Distance(atachedUnit.transform.position, gameObject.transform.position))
                        atachedUnit = enemy;
            }

            List<GameObject> nextToCells = NextToCellsWalk();

            needPosition = new(100f, 100f, 100f);

            foreach (GameObject cell in nextToCells)
            {
                PLaceManager pLaceManager = cell.GetComponent<PLaceManager>();

                if (!(pLaceManager.isAllyPlace || pLaceManager.isEnemyPlace)
                    && Vector3.Distance(needPosition, atachedUnit.transform.position) > Vector3.Distance(cell.transform.localPosition, atachedUnit.transform.position))
                    needPosition = cell.transform.localPosition;
            }

            speed = needPosition - gameObject.transform.position;
            gameObject.GetComponentInChildren<Animator>().SetTrigger("Start");

            source.sampleSource.clip = walkSample;
            source.sampleSource.Play();

            StartCoroutine(Walk(gameObject, needPosition, speed));
        }
    }

    private List<GameObject> NextToCellsWalk()
    {
        List<GameObject> nextToCells = new();
        int height = heightWalk[0], weight = distWalkW, index = 0;

        if (W - weight < 0)
        {
            height = weight - W;
            weight -= height;
            index = heightWalk.IndexOf(height) + 1;
        }

        for (int i = weight; i >= -distWalkW; i--)
        {
            int minH = Math.Max(0, H - height), maxH = Math.Min(11, H + height);

            nextToCells.Add(spawnPlace.cellsArr[Math.Min(14, W - i), minH]);
            nextToCells.Add(spawnPlace.cellsArr[Math.Min(14, W - i), maxH]);

            height = heightWalk[index];

            index++;
        }

        return nextToCells;
    }

    private Vector3 NeedPosition(GameObject unit, int w, int h)
    {
        Vector3 needPosition = new(100f, 100f, 100f);

        if (Vector3.Distance(unit.transform.position, spawnPlace.cellsArr[w, h].transform.localPosition) > 1.3f)
        {
            for (int i = 0; i < spawnPlace.nextCells.Count; i += 2)
            {
                if (w + spawnPlace.nextCells[i] >= 0 && h + spawnPlace.nextCells[i + 1] >= 0)
                {
                    PLaceManager pLaceManager = spawnPlace.cellsArr[Math.Min(14, w + spawnPlace.nextCells[i]), Math.Min(11, h + spawnPlace.nextCells[i + 1])].GetComponent<PLaceManager>();

                    if (!(pLaceManager.isEnemyPlace || pLaceManager.isAllyPlace)
                        && Vector3.Distance(unit.transform.position, needPosition) > Vector3.Distance(unit.transform.position, spawnPlace.cellsArr[Math.Min(14, w + spawnPlace.nextCells[i]), Math.Min(11, h + spawnPlace.nextCells[i + 1])].transform.localPosition))
                    {
                        needPosition = spawnPlace.cellsArr[Math.Min(14, w + spawnPlace.nextCells[i]), Math.Min(11, h + spawnPlace.nextCells[i + 1])].transform.localPosition;
                    }
                }
            }
        }
        else
        {
            needPosition = unit.transform.position;
        }

        return needPosition;
    }

    private bool IsHold(int w, int h)
    {
        bool isHold = true;

        for (int i = 0; i < spawnPlace.nextCells.Count; i += 2)
        {
            if (w + spawnPlace.nextCells[i] >= 0 && h + spawnPlace.nextCells[i + 1] >= 0)
            {
                PLaceManager pLaceManager = spawnPlace.cellsArr[Math.Min(14, w + spawnPlace.nextCells[i]), Math.Min(11, h + spawnPlace.nextCells[i + 1])].GetComponent<PLaceManager>();

                if (!(pLaceManager.isEnemyPlace && pLaceManager.isAllyPlace))
                {
                    isHold = false;
                    break;
                }
            }
        }

        return isHold;
    }

    private void EndOfTurn(GameObject unit)
    {
        Units units = unit.GetComponent<Units>();

        spawnPlace.indexAll++;

        if (units.endOfResist > 0)
        {
            units.endOfResist--;

            if (units.endOfResist == 0)
            {
                unit.GetComponent<ParticleSystem>().Stop();

                units.resist = 0;
            }
        }

        if (spawnPlace.indexAll >= spawnPlace.allUnits.Count)
        {
            spawnPlace.indexAll = 0;

            foreach (GameObject gameObject in spawnPlace.allUnits)
                gameObject.GetComponent<Units>().isEndTurn = false;
        }

        else if (spawnPlace.allUnits[spawnPlace.indexAll].CompareTag("Enemy"))
        {
            spawnPlace.allUnits[spawnPlace.indexAll].GetComponent<Units>().EnemyTurn();
            spawnPlace.chooseUnit = spawnPlace.allUnits[spawnPlace.indexAll];
        }

        spawnPlace.isMove = spawnPlace.isChoose = false;

        units.isEndTurn = true;

        isHold = IsHold(units.W, units.H);
    }

    private void Alpha(float alpha, GameObject obj)
    {
        Color newColor = obj.GetComponentInChildren<MeshRenderer>().material.color;

        newColor.a = alpha;
        obj.GetComponentInChildren<MeshRenderer>().material.color = newColor;
    }

    public IEnumerator Walk(GameObject unit, Vector3 targetPos, Vector3 speed)
    {
        if (Vector3.Distance(targetPos, unit.transform.position) > 0.005f)
        {
            yield return new WaitForFixedUpdate();

            unit.transform.position += speed * 0.008f;
            StartCoroutine(Walk(unit, targetPos, speed));
        }
        else
        {
            source.sampleSource.Stop();

            if (unit.CompareTag("Ally"))
                foreach (GameObject cell in spawnPlace.cellsArr)
                    Alpha(1f, cell);

            unit.GetComponentInChildren<Animator>().SetTrigger("End");

            EndOfTurn(unit);
        }
    }

    public IEnumerator Atack(Vector3 targetPos, GameObject unit, GameObject atachedUnit, Vector3 speed)
    {
        if (!isRange && Vector3.Distance(unit.transform.position, targetPos) > 0.005f)
        {
            yield return new WaitForFixedUpdate();

            unit.transform.position += speed * 0.008f;
            StartCoroutine(Atack(targetPos, unit, atachedUnit, speed));
        }

        else
        {
            if (unit.CompareTag("Ally"))
                foreach (GameObject cell in spawnPlace.cellsArr)
                    Alpha(1f, cell);

            unit.GetComponentInChildren<Animator>().SetTrigger("Atack");
            yield return new WaitForSeconds(timeOfAnim);

            if (isRange)
            {
                GameObject misile = unit.transform.GetChild(0).GetChild(0).gameObject;
                Vector3 oldPosition = misile.transform.position;

                misile.SetActive(true);
                yield return StartCoroutine(Shoot(misile, atachedUnit, speed, oldPosition));

                while (!isEndOfAnim)
                    yield return null;

                isEndOfAnim = false;
            }

            unit.GetComponentInChildren<Animator>().SetTrigger("End");

            int text = Convert.ToInt32(atachedUnit.transform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>().text), dmg = UnityEngine.Random.Range(minDmg, maxDmg);

            //dmg -= atachedUnit.name[atachedUnit.name.IndexOf(".") + 1].ConvertTo<int>() - unit.name[unit.name.IndexOf(".") + 1].ConvertTo<int>();

            if (text - dmg > 0)
            {
                atachedUnit.transform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>().text = (text - dmg).ToString();


                EndOfTurn(unit);
            }

            else
            {
                spawnPlace.allUnits.Remove(atachedUnit);

                if (atachedUnit.CompareTag("Ally"))
                    spawnPlace.allies.Remove(atachedUnit);
                else
                    spawnPlace.enemies.Remove(atachedUnit);

                Destroy(atachedUnit);

                if (spawnPlace.allies.Count == 0)
                {
                    fightButtons.lose.SetActive(true);
                }

                else if (spawnPlace.enemies.Count == 0)
                {
                    fightButtons.win.SetActive(true);
                }
            }
        }
    }
    public IEnumerator Atack(Vector3 targetPos, GameObject unit, List<GameObject> atachedUnits)
    {
        if (unit.CompareTag("Ally"))
            foreach (GameObject cell in spawnPlace.cellsArr)
                Alpha(1f, cell);

        GameObject misile = unit.transform.GetChild(0).GetChild(0).gameObject;
        Vector3 oldPosition = misile.transform.position;

        unit.GetComponentInChildren<Animator>().SetTrigger("Atack");
        yield return new WaitForSeconds(timeOfAnim);

        yield return StartCoroutine(Shoot(misile, targetPos, targetPos - misile.transform.position, oldPosition));

        while (!isEndOfAnim)
            yield return null;

        unit.GetComponentInChildren<Animator>().SetTrigger("End");
        isEndOfAnim = false;


        int dmg = UnityEngine.Random.Range(minDmg, maxDmg);

        foreach (GameObject atachedUnit in atachedUnits)
        {
            int text = Convert.ToInt32(atachedUnit.transform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>().text);

            dmg -= atachedUnit.GetComponent<Units>().lvl - unit.GetComponent<Units>().lvl;
            dmg -= atachedUnit.GetComponent<Units>().resist;

            if (text - dmg > 0)
            {
                atachedUnit.transform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>().text = (text - dmg).ToString();
            }

            else
            {
                spawnPlace.allUnits.Remove(atachedUnit);

                if (atachedUnit.CompareTag("Ally"))
                {
                    spawnPlace.allies.Remove(atachedUnit);

                    spawnPlace.diedUnits[atachedUnit.GetComponent<Units>().index]++;
                }

                else
                    spawnPlace.enemies.Remove(atachedUnit);

                Destroy(atachedUnit);

                if (spawnPlace.allies.Count == 0)
                {
                    fightButtons.lose.SetActive(true);
                }

                else if (spawnPlace.enemies.Count == 0)
                {
                    fightButtons.win.SetActive(true);
                }
            }
        }

        EndOfTurn(unit);
    }


    IEnumerator Shoot(GameObject misile, GameObject atachedUnit, Vector3 speed, Vector3 oldPosition)
    {
        if (Vector3.Distance(misile.transform.position, atachedUnit.transform.position) > 0.5f)
        {
            misile.transform.position += speed * 0.01f;
            yield return new WaitForFixedUpdate();

            StartCoroutine(Shoot(misile, atachedUnit, speed, oldPosition));
        }

        else
        {
            misile.transform.position = oldPosition;

            if (misile.name == "MisileWizzard")
            {
                atachedUnit.GetComponent<ParticleSystem>().Play();
                atachedUnit.GetComponent<Units>().resist = addResist;
                atachedUnit.GetComponent<Units>().endOfResist = addEndOfResist;
            }

            misile.SetActive(false);

            isEndOfAnim = true;
        }
    }

    IEnumerator Shoot(GameObject misile, Vector3 targetPos, Vector3 speed, Vector3 oldPosition)
    {
        if (Vector3.Distance(misile.transform.position, targetPos) > 0.1f)
        {
            misile.transform.position += speed * 0.01f;
            yield return new WaitForFixedUpdate();

            StartCoroutine(Shoot(misile, targetPos, speed, oldPosition));
        }

        else
        {
            misile.transform.position = oldPosition;
                
            isEndOfAnim = true;
        }
    }


    public class OrkKnight
    {
        public static bool IsAtack(GameObject unit, int distAtackW, int H, int W, List<int> heightAtack)
        {
            bool isAtack = false;

            int height = heightAtack[0], weight = distAtackW, index = 0;

            if (W - weight < 0)
            {
                height = weight - W;
                weight -= height;
                index = heightAtack.IndexOf(height) + 1;
            }

            for (int i = weight; i >= -distAtackW; i--)
            {
                int minH = Math.Max(0, H - height), maxH = Math.Min(11, H + height);

                for (int j = minH; j <= maxH; j++)
                {
                    if (spawnPlace.cellsArr[Math.Min(14, W - i), j].GetComponent<PLaceManager>().isAllyPlace)
                    {
                        isAtack = true;
                        break;
                    }

                }

                height = heightAtack[index];

                index++;
            }

            return isAtack;
        }

        public static GameObject AtachedUnits(GameObject unit, int distAtackW, int H, int W, List<int> heightAtack)
        {
            int height = heightAtack[0], weight = distAtackW, index = 0;
            GameObject atachedUnit = new();
            List<GameObject> atachedUnits = new();

            if (W - weight < 0)
            {
                height = weight - W;
                weight -= height;
                index = heightAtack.IndexOf(height) + 1;
            }

            for (int i = weight; i >= -distAtackW; i--)
            {
                int minH = Math.Max(0, H - height), maxH = Math.Min(11, H + height);

                for (int j = minH; j <= maxH; j++)
                {
                    if (spawnPlace.cellsArr[Math.Min(14, W - i), j].GetComponent<PLaceManager>().isAllyPlace)
                    {
                        atachedUnits.Add(spawnPlace.cellsArr[Math.Min(14, W - i), j].GetComponent<PLaceManager>().unit);
                    }
                }

                height = heightAtack[index];

                index++;
            }

            foreach (GameObject atached in atachedUnits)
                if (Vector3.Distance(atached.transform.position, unit.transform.position) < Vector3.Distance(atachedUnit.transform.position, unit.transform.position))
                    atachedUnit = atached;

            return atachedUnit;
        }
    }

    public class OrkWizard
    {
        public static bool IsAtack(GameObject unit, int distAtackW, int H, int W, List<int> heightAtack)
        {
            bool isAtack = false;

            int height = heightAtack[0], weight = distAtackW, index = 0;

            if (W - weight < 0)
            {
                height = weight - W;
                weight -= height;
                index = heightAtack.IndexOf(height) + 1;
            }

            for (int i = weight; i >= -distAtackW; i--)
            {
                int minH = Math.Max(0, H - height), maxH = Math.Min(11, H + height);

                for (int j = minH; j <= maxH; j++)
                {
                    if (spawnPlace.cellsArr[Math.Min(14, W - i), j].GetComponent<PLaceManager>().isEnemyPlace 
                        && spawnPlace.cellsArr[Math.Min(14, W - i), j].GetComponent<PLaceManager>().unit != unit
                        && spawnPlace.cellsArr[Math.Min(14, W - i), j].GetComponent<PLaceManager>().unit.GetComponent<Units>().resist == 0)
                    {
                        isAtack = true;
                        break;
                    }
                }

                height = heightAtack[index];

                index++;
            }

            return isAtack;
        }

        public static GameObject AtachedUnits(GameObject unit, int distAtackW, int H, int W, List<int> heightAtack)
        {
            int weight = distAtackW, height = heightAtack[0], index = 0;
            List<GameObject> atachedUnits = new();

            if (W - weight < 0)
            {
                height = weight - W;
                weight -= height;
                index = heightAtack.IndexOf(height) + 1;
            }

            for (int i = weight; i >= -distAtackW; i--)
            {
                int minH = Math.Max(0, H - height), maxH = Math.Min(11, H + height);

                for (int j = minH; j <= maxH; j++)
                {
                    if (spawnPlace.cellsArr[Math.Min(14, W - i), j].GetComponent<PLaceManager>().isEnemyPlace
                        && spawnPlace.cellsArr[Math.Min(14, W - i), j].GetComponent<PLaceManager>().unit != unit
                        && spawnPlace.cellsArr[Math.Min(14, W - i), j].GetComponent<PLaceManager>().unit.GetComponent<Units>().resist == 0)
                    {
                        atachedUnits.Add(spawnPlace.cellsArr[Math.Min(14, W - i), j].GetComponent<PLaceManager>().unit);
                    }
                }

                height = heightAtack[index];

                index++;
            }

            GameObject atachedUnit = atachedUnits[0];

            foreach (GameObject atached in atachedUnits)
                if (Vector3.Distance(atached.transform.position, unit.transform.position) < Vector3.Distance(atachedUnit.transform.position, unit.transform.position))
                    atachedUnit = atached;

            return atachedUnit;
        }
    }

    public class OrkAracher
    {
        public static bool IsAtack(GameObject unit, int distAtackW, int H, int W, List<int> heightAtack)
        {
            return true;
        }

        public static GameObject AtachedUnits(GameObject unit, int distAtackW, int H, int W, List<int> heightAtack)
        {
            List<GameObject> atachedUnits = new();

            for (int w = 0; w <= 14; w++)
            {
                for (int h = 0; h <= 11; h++)
                {
                    if (spawnPlace.cellsArr[w, h].GetComponent<PLaceManager>().isAllyPlace)
                    {
                        atachedUnits.Add(spawnPlace.cellsArr[w, h].GetComponent<PLaceManager>().unit);
                        break;
                    }
                }
            }

            return atachedUnits[0];
        }
    }

    public class OrkKatapulta
    {
        public static bool IsAtack(GameObject unit, int distAtackW, int H, int W, List<int> heightAtack)
        {
            return true;
        }

        public static (List<GameObject>, Vector3) AtachedUnits(GameObject unit, int distAtackW, int H, int W, List<int> heightAtack, List<int> heightSplash)
        {
            int distSplashW = distAtackW, index = 0;
            List<GameObject> atachedUnits = new(), newAtachedUnits = new();
            Vector3 needPosition = Vector3.zero, newNeedPosition;

            for (int w = W; w <= 14; w++)
            {
                if (w - distSplashW < 0)
                {
                    index = distSplashW - w;
                    distSplashW = w;
                }

                for (int h = 0; h <= 11; h++)
                {
                    newNeedPosition = spawnPlace.cellsArr[w, h].transform.localPosition;

                    for (int i = distSplashW; i >= -distAtackW; i--)
                    {
                        int minH = Math.Max(h - heightSplash[index], 0), maxH = Math.Min(h + heightSplash[index], 11);

                        for (int j = minH; j <= maxH; j++)
                        {
                            if (spawnPlace.cellsArr[Math.Min(14, w - i), j].GetComponent<PLaceManager>().isAllyPlace)
                            {
                                newAtachedUnits.Add(spawnPlace.cellsArr[Math.Min(14, w - i), j].GetComponent<PLaceManager>().unit);
                            }
                        }

                        index++;
                    }

                    if (atachedUnits.Count <= newAtachedUnits.Count)
                    {
                        atachedUnits = newAtachedUnits;
                        needPosition = newNeedPosition;
                    }

                    newAtachedUnits = new();

                    index = 0;
                }

                distSplashW = distAtackW;
            }

            return (atachedUnits, needPosition);
        }
    }
}