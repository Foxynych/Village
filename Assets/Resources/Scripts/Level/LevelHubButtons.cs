using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using static EventAgregator;

public class LevelHubButtons : MonoBehaviour
{
    public GameObject fightBack, cellLight, cellDark;
    public Material[] cellMaterials;
    public Material[] fightBackMaterials;

    public void ButtonClick()
    {
        switch (EventSystem.current.currentSelectedGameObject.name)
        {
            case "Back":
                SceneManager.LoadScene(0);
                break;

            case "lvl1":
                SpawnPlace.path = "/Levels/Level_1";
                SpawnPlace.prize = 500;

                source.mainSource.Stop();
                source.mainSource.clip = source.fightTheme;
                source.mainSource.Play();

                SpawnPlace.materials = new Material[] { cellMaterials[0], cellMaterials[1], fightBackMaterials[0] };

                SceneManager.LoadScene(2);
                break;

            case "lvl2":
                SpawnPlace.path = "/Levels/Level_2";
                SpawnPlace.prize = 500;

                source.mainSource.Stop();
                source.mainSource.clip = source.fightTheme;
                source.mainSource.Play();

                SpawnPlace.materials = new Material[] { cellMaterials[0], cellMaterials[1], fightBackMaterials[0] };

                SceneManager.LoadScene(2);
                break;

            case "lvl3":
                SpawnPlace.path = "/Levels/Level_3";
                SpawnPlace.prize = 500;

                source.mainSource.Stop();
                source.mainSource.clip = source.fightTheme;
                source.mainSource.Play();

                SpawnPlace.materials = new Material[] { cellMaterials[0], cellMaterials[1], fightBackMaterials[0] };

                SceneManager.LoadScene(2);
                break;

            case "lvl4":
                SpawnPlace.path = "/Levels/Level_4";
                SpawnPlace.prize = 500;

                source.mainSource.Stop();
                source.mainSource.clip = source.fightTheme;
                source.mainSource.Play();

                SpawnPlace.materials = new Material[] { cellMaterials[0], cellMaterials[1], fightBackMaterials[0] };

                SceneManager.LoadScene(2);
                break;

            case "lvl5":
                SpawnPlace.path = "/Levels/Level_5";
                SpawnPlace.prize = 500;

                source.mainSource.Stop();
                source.mainSource.clip = source.fightTheme;
                source.mainSource.Play();

                SpawnPlace.materials = new Material[] { cellMaterials[0], cellMaterials[1], fightBackMaterials[0] };

                SceneManager.LoadScene(2);
                break;

            case "lvl6":
                SpawnPlace.path = "/Levels/Level_6";
                SpawnPlace.prize = 1000;

                source.mainSource.Stop();
                source.mainSource.clip = source.fightTheme;
                source.mainSource.Play();

                SpawnPlace.materials = new Material[] { cellMaterials[0], cellMaterials[1], fightBackMaterials[1] };

                SceneManager.LoadScene(2);
                break;

            case "lvl7":
                SpawnPlace.path = "/Levels/Level_7";
                SpawnPlace.prize = 1000;

                source.mainSource.Stop();
                source.mainSource.clip = source.fightTheme;
                source.mainSource.Play();

                SpawnPlace.materials = new Material[] { cellMaterials[0], cellMaterials[1], fightBackMaterials[1] };

                SceneManager.LoadScene(2);
                break;

            case "lvl8":
                SpawnPlace.path = "/Levels/Level_8";
                SpawnPlace.prize = 1000;

                source.mainSource.Stop();
                source.mainSource.clip = source.fightTheme;
                source.mainSource.Play();

                SpawnPlace.materials = new Material[] { cellMaterials[0], cellMaterials[1], fightBackMaterials[1] };

                SceneManager.LoadScene(2);
                break;

            case "lvl9":
                SpawnPlace.path = "/Levels/Level_9";
                SpawnPlace.prize = 1000;

                source.mainSource.Stop();
                source.mainSource.clip = source.fightTheme;
                source.mainSource.Play();

                SpawnPlace.materials = new Material[] { cellMaterials[0], cellMaterials[1], fightBackMaterials[1] };

                SceneManager.LoadScene(2);
                break;

            case "lvl10":
                SpawnPlace.path = "/Levels/Level_10";
                SpawnPlace.prize = 1000;

                source.mainSource.Stop();
                source.mainSource.clip = source.fightTheme;
                source.mainSource.Play();

                SpawnPlace.materials = new Material[] { cellMaterials[0], cellMaterials[1], fightBackMaterials[1] };

                SceneManager.LoadScene(2);
                break;

            case "lvl11":
                SpawnPlace.path = "/Levels/Level_11";
                SpawnPlace.prize = 1500;

                source.mainSource.Stop();
                source.mainSource.clip = source.fightTheme;
                source.mainSource.Play();

                SpawnPlace.materials = new Material[] { cellMaterials[2], cellMaterials[3], fightBackMaterials[2] };

                SceneManager.LoadScene(2);
                break;

            case "lvl12":
                SpawnPlace.path = "/Levels/Level_12";
                SpawnPlace.prize = 1500;

                source.mainSource.Stop();
                source.mainSource.clip = source.fightTheme;
                source.mainSource.Play();

                SpawnPlace.materials = new Material[] { cellMaterials[2], cellMaterials[3], fightBackMaterials[2] };

                SceneManager.LoadScene(2);
                break;

            case "lvl13":
                SpawnPlace.path = "/Levels/Level_13";
                SpawnPlace.prize = 1500;

                source.mainSource.Stop();
                source.mainSource.clip = source.fightTheme;
                source.mainSource.Play();

                SpawnPlace.materials = new Material[] { cellMaterials[2], cellMaterials[3], fightBackMaterials[2] };

                SceneManager.LoadScene(2);
                break;

            case "lvl14":
                SpawnPlace.path = "/Levels/Level_14";
                SpawnPlace.prize = 1500;

                source.mainSource.Stop();
                source.mainSource.clip = source.fightTheme;
                source.mainSource.Play();

                SpawnPlace.materials = new Material[] { cellMaterials[2], cellMaterials[3], fightBackMaterials[2] };

                SceneManager.LoadScene(2);
                break;

            case "lvl15":
                SpawnPlace.path = "/Levels/Level_15";
                SpawnPlace.prize = 1500;

                source.mainSource.Stop();
                source.mainSource.clip = source.fightTheme;
                source.mainSource.Play();

                SpawnPlace.materials = new Material[] { cellMaterials[2], cellMaterials[3], fightBackMaterials[2] };

                SceneManager.LoadScene(2);
                break;

            case "lvl16":
                SpawnPlace.path = "/Levels/Level_16";
                SpawnPlace.prize = 2000;

                source.mainSource.Stop();
                source.mainSource.clip = source.fightTheme;
                source.mainSource.Play();

                SpawnPlace.materials = new Material[] { cellMaterials[0], cellMaterials[1], fightBackMaterials[3] };

                SceneManager.LoadScene(2);
                break;

            case "lvl17":
                SpawnPlace.path = "/Levels/Level_17";
                SpawnPlace.prize = 2000;

                source.mainSource.Stop();
                source.mainSource.clip = source.fightTheme;
                source.mainSource.Play();

                SpawnPlace.materials = new Material[] { cellMaterials[0], cellMaterials[1], fightBackMaterials[3] };

                SceneManager.LoadScene(2);
                break;

            case "lvl18":
                SpawnPlace.path = "/Levels/Level_18";
                SpawnPlace.prize = 2000;

                source.mainSource.Stop();
                source.mainSource.clip = source.fightTheme;
                source.mainSource.Play();

                SpawnPlace.materials = new Material[] { cellMaterials[0], cellMaterials[1], fightBackMaterials[3] };

                SceneManager.LoadScene(2);
                break;

            case "lvl19":
                SpawnPlace.path = "/Levels/Level_19";
                SpawnPlace.prize = 2000;

                source.mainSource.Stop();
                source.mainSource.clip = source.fightTheme;
                source.mainSource.Play();

                SpawnPlace.materials = new Material[] { cellMaterials[0], cellMaterials[1], fightBackMaterials[3] };

                SceneManager.LoadScene(2);
                break;

            case "lvl20":
                SpawnPlace.path = "/Levels/Level_20";
                SpawnPlace.prize = 2000;

                source.mainSource.Stop();
                source.mainSource.clip = source.fightTheme;
                source.mainSource.Play();

                SpawnPlace.materials = new Material[] { cellMaterials[0], cellMaterials[1], fightBackMaterials[3] };

                SceneManager.LoadScene(2);
                break;

            case "lvl21":
                SpawnPlace.path = "/Levels/Level_21";
                SpawnPlace.prize = 2500;

                source.mainSource.Stop();
                source.mainSource.clip = source.fightTheme;
                source.mainSource.Play();

                SpawnPlace.materials = new Material[] { cellMaterials[0], cellMaterials[1], fightBackMaterials[4] };

                SceneManager.LoadScene(2);
                break;

            case "lvl22":
                SpawnPlace.path = "/Levels/Level_22";
                SpawnPlace.prize = 2500;

                source.mainSource.Stop();
                source.mainSource.clip = source.fightTheme;
                source.mainSource.Play();

                SpawnPlace.materials = new Material[] { cellMaterials[0], cellMaterials[1], fightBackMaterials[4] };

                SceneManager.LoadScene(2);
                break;

            case "lvl23":
                SpawnPlace.path = "/Levels/Level_23";
                SpawnPlace.prize = 2500;

                source.mainSource.Stop();
                source.mainSource.clip = source.fightTheme;
                source.mainSource.Play();

                SpawnPlace.materials = new Material[] { cellMaterials[0], cellMaterials[1], fightBackMaterials[4] };

                SceneManager.LoadScene(2);
                break;

            case "lvl24":
                SpawnPlace.path = "/Levels/Level_24";
                SpawnPlace.prize = 2500;

                source.mainSource.Stop();
                source.mainSource.clip = source.fightTheme;
                source.mainSource.Play();

                SpawnPlace.materials = new Material[] { cellMaterials[0], cellMaterials[1], fightBackMaterials[4] };

                SceneManager.LoadScene(2);
                break;

            case "lvl25":
                SpawnPlace.path = "/Levels/Level_25";
                SpawnPlace.prize = 2500;

                source.mainSource.Stop();
                source.mainSource.clip = source.fightTheme;
                source.mainSource.Play();

                SpawnPlace.materials = new Material[] { cellMaterials[0], cellMaterials[1], fightBackMaterials[4] };

                SceneManager.LoadScene(2);
                break;
        }
    }
}
