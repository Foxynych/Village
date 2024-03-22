using System;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Android;
using static EventAgregator;

public class Data : MonoBehaviour
{
    public static int money, manPower;
    public static List<int> lvlUnit = new() { 0, 0, 0, 0 }, count = new() { 90, 20, 20, 10 };

    public static List<GameObject> gameObjectsToSave = new();

    public List<UnityEngine.GameObject> enemies = new();

    private void Awake()
    {
        data = this;

        if (!File.Exists(Application.persistentDataPath + "/Data.json"))
        {
            Permission.RequestUserPermission(Permission.ExternalStorageRead);
            Permission.RequestUserPermission(Permission.ExternalStorageWrite);

            StreamWriter file = new(File.Create(Application.persistentDataPath + "/Data.json"));
            file.Write(Resources.Load<TextAsset>("Json/Data").text);
            file.Close();
        }
    }

    public void Save()
    {
        Resource resource = new(money, manPower, lvlUnit, count);
        string[] json = { "{", "    \"gameObjects\":[", "", "    ],", "  \"resources\":[", "", "  ]", "}" };

        for (int i = 0; i < gameObjectsToSave.Count - 1; i++)
        {
            json[2] += Environment.NewLine + JsonUtility.ToJson(gameObjectsToSave[i], true) + ",";
        }

        json[2] += Environment.NewLine + JsonUtility.ToJson(gameObjectsToSave[^1], true);

        json[5] += Environment.NewLine + JsonUtility.ToJson(resource, true);

        File.WriteAllLines(Application.persistentDataPath + "/Data.json", json);
    }

    public void Load(string json)
    {
        DataBase dataBase = JsonUtility.FromJson<DataBase>(json);

        gameObjectsToSave.RemoveRange(0, gameObjectsToSave.Count);

        for (int i = 0; i < dataBase.resources.Length; i++)
        {
            money = dataBase.resources[i].money;
            manPower = dataBase.resources[i].manPower;
            lvlUnit = dataBase.resources[i].lvlUnit;
            count = dataBase.resources[i].count;
        }

        for (int i = 0; i < dataBase.gameObjects.Length; i++)
        {
            gameObjectsToSave.Add(dataBase.gameObjects[i]);

            string name = dataBase.gameObjects[i].name[..dataBase.gameObjects[i].name.IndexOf('_')];

            UnityEngine.GameObject obj = Instantiate(Resources.Load<UnityEngine.GameObject>($"Models/{name}/{name}_{dataBase.gameObjects[i].lvl}"), dataBase.gameObjects[i].position, dataBase.gameObjects[i].quaternion);

            Builds builds = obj.GetComponent<Builds>();

            builds.lvl = dataBase.gameObjects[i].lvl;
            builds.indexOfBuild = i;

            if (dataBase.gameObjects[i].isRecruting)
            {
                builds.isRecruting = dataBase.gameObjects[i].isRecruting;

                StartCoroutine(builds.Timer(DateTimeOffset.FromUnixTimeSeconds(dataBase.gameObjects[i].allRecrutingTime), dataBase.gameObjects[i].index, dataBase.gameObjects[i].count));
            }

            else if (dataBase.gameObjects[i].name[..dataBase.gameObjects[i].name.IndexOf("_")] == "Шахта")
            {
                money += Convert.ToInt32(DateTimeOffset.Now.ToUnixTimeSeconds() / dataBase.gameObjects[i].allGoldTime);
                buttons.textMoney.text = money.ToString();

                StartCoroutine(builds.Gold(DateTimeOffset.FromUnixTimeSeconds(DateTimeOffset.Now.ToUnixTimeSeconds() + 600)));
            }

            else if (name == "ТХ")
            {
                hubManager.txLvl = dataBase.gameObjects[i].lvl;
                hubManager.activeObj = hubManager.TX = obj;
            }
        }

        for (int i = 0; i < dataBase.units.Length; i++)
        {
            string name = dataBase.units[i].name[..dataBase.units[i].name.IndexOf('_')];

            UnityEngine.GameObject obj = Instantiate(Resources.Load<UnityEngine.GameObject>($"Models/{name}/{name}_{dataBase.units[i].lvl}"), dataBase.units[i].position, dataBase.units[i].quaternion);

            spawnPlace.enemies.Add(obj);
            spawnPlace.allUnits.Add(obj);
        }

        for (int i = 0; i < dataBase.obstacles.Length; i++)
        {
            string name = dataBase.obstacles[i].name[..dataBase.obstacles[i].name.IndexOf("(")];

            Instantiate(Resources.Load<UnityEngine.GameObject>($"Models/Препятствия/{name}"), dataBase.obstacles[i].position, dataBase.obstacles[i].quaternion);
        }
    }

    [System.Serializable]
    public class DataBase
    {
        public GameObject[] gameObjects = { };
        public Resource[] resources = { };

        public Obstacle[] obstacles = { };
        public Unit[] units = { };
    }

    [System.Serializable]
    public class GameObject
    {
        public string name;
        public int lvl;
        public Vector3 position;
        public Quaternion quaternion;

        public long allGoldTime;

        public long allRecrutingTime;
        public int index;
        public int count;
        public bool isRecruting;

        public GameObject(string Name, int Lvl,  Vector3 Position, Quaternion Quaternion)
        {
            name = Name;
            lvl = Lvl;
            position = Position;
            quaternion = Quaternion;
        }
    }

    [System.Serializable]
    public class Resource
    {
        public int money;
        public int manPower;
        public List<int> lvlUnit = new();
        public List<int> count = new();

        public Resource(int Money, int ManPower, List<int> LvlUnit, List<int> Count)
        {
            money = Money;
            manPower = ManPower;
            lvlUnit = LvlUnit;
            count = Count;
        }
    }

    [System.Serializable]
    public class Unit
    {
        public string name;
        public int lvl;
        public Vector3 position;
        public Quaternion quaternion;
    }

    [System.Serializable]
    public class Obstacle
    {
        public string name;
        public Vector3 position;
        public Quaternion quaternion;
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause)
            Save();
    }
}
