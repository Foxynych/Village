using UnityEngine;

public class EventAgregator : MonoBehaviour
{
    public static Source source;

    public static Data data;

    public static Icons icons;
    public static ButtonsClasss buttons;
    public static HubManager hubManager;

    public static SpawnPlace spawnPlace;
    public static FightButtons fightButtons;

    private void Awake()
    {
        if (GameObject.FindGameObjectsWithTag("GameController").Length == 0)
        {
            DontDestroyOnLoad(Instantiate(Resources.Load<GameObject>("DDOL/DontDestroyOnLoad")));
        }
    }
}
