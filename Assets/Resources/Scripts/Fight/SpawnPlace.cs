using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static EventAgregator;

public class SpawnPlace : MonoBehaviour
{
    public static int prize;
    public static string path;
    public static Material[] materials;

    public GameObject cells, cell, cellRight, cellDown;
    public RawImage knightIcon, wizardIcon, archerIcon, katapultaIcon;

    [HideInInspector] public List<GameObject> placedUnits = new(), deletedUnits = new(), enemies = new(), allies = new(), allUnits = new();
    [HideInInspector] public List<int> newCount = new() { 0, 0, 0, 0 }, diedUnits = new() {0, 0, 0, 0}, nextCells = new() {0, 1,  0, -1,  1, 0,  -1, 0};
    [HideInInspector] public GameObject[,] cellsArr = new GameObject[15, 12];
    [HideInInspector] public GameObject chooseUnit;
    [HideInInspector] public Vector3 up, down, left, right;
    [HideInInspector] public int indexPlace, maxCount, indexAll = 0;
    [HideInInspector] public bool isChoose, isMove;

    private void Awake()
    {
        spawnPlace = this;
    }

    private void Start()
    {
        data.Load(Resources.Load<TextAsset>("Json" + path).text);

        gameObject.transform.GetChild(0).GetComponent<MeshRenderer>().material = materials[2];

        float i = cellRight.transform.localPosition.z - cell.transform.localPosition.z, j = cellDown.transform.localPosition.x - cell.transform.localPosition.x;
        int h, w = -1;

        up = new(-j, 0f, 0f); down = new(j, 0f, 0f);
        right = new(0f, 0f, i); left = new(0f, 0f, -i);

        knightIcon.texture = Resources.Load<Texture>($"Sprites/Units/Рыцарь/Рыцарь{Data.lvlUnit[0]}");
        wizardIcon.texture = Resources.Load<Texture>($"Sprites/Units/Маг/Маг{Data.lvlUnit[1]}");
        archerIcon.texture = Resources.Load<Texture>($"Sprites/Units/Лучник/Лучник{Data.lvlUnit[2]}");
        katapultaIcon.texture = Resources.Load<Texture>($"Sprites/Units/Катапульта/Катапульта{Data.lvlUnit[3]}");

        for (int i1 = 0; i1 < 15; i1++)
        {
            w++;
            h = 0;

            for (int j1 = 0; j1 < 12; j1++)
            {
                GameObject cellj = null;

                if (i1 % 2 == 1)
                {
                    if (j1 % 2 == 1)
                    {
                        cellj = Instantiate(Resources.Load<GameObject>("Models/Поле/Cells/CellLight"), new Vector3(cell.transform.position.x + j1 * j, cell.transform.position.y, cell.transform.position.z + i1 * i), cell.transform.rotation, cells.transform);
                        cellj.GetComponentInChildren<MeshRenderer>().material = materials[0];
                    }
                    else if (j1 % 2 == 0)
                    {
                        cellj = Instantiate(Resources.Load<GameObject>("Models/Поле/Cells/CellDark"), new Vector3(cell.transform.position.x + j1 * j, cell.transform.position.y, cell.transform.position.z + i1 * i), cell.transform.rotation, cells.transform);
                        cellj.GetComponentInChildren<MeshRenderer>().material = materials[1];
                    }
                }
                else
                {
                    if (j1 % 2 == 0)
                    {
                        cellj = Instantiate(Resources.Load<GameObject>("Models/Поле/Cells/CellLight"), new Vector3(cell.transform.position.x + j1 * j, cell.transform.position.y, cell.transform.position.z + i1 * i), cell.transform.rotation, cells.transform);
                        cellj.GetComponentInChildren<MeshRenderer>().material = materials[0];
                    }
                    else if (j1 % 2 == 1)
                    {
                        cellj = Instantiate(Resources.Load<GameObject>("Models/Поле/Cells/CellDark"), new Vector3(cell.transform.position.x + j1 * j, cell.transform.position.y, cell.transform.position.z + i1 * i), cell.transform.rotation, cells.transform);
                        cellj.GetComponentInChildren<MeshRenderer>().material = materials[1];
                    }
                }

                cellj.GetComponent<PLaceManager>().W = w;
                cellj.GetComponent<PLaceManager>().H = h;

                cellsArr[w, h] = cellj;
                h++;
            }

        }
           
        Destroy(cell); Destroy(cellRight); Destroy(cellDown);
    }
}