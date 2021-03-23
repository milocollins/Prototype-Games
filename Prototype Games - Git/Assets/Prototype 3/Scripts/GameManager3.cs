using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager3 : MonoBehaviour
{
    public enum Level
    {
        level_0,
        level_1,
        level_2
    }
    public Level currentLevel;
    private BoxCollider2D teleport;
    private string teleportString;
    public static GameManager3 theManager;
    public void Awake()
    {
        theManager = this;
    }
    private void Start()
    {
        teleport = GetComponent<BoxCollider2D>();
        switch (currentLevel)
        {
            case Level.level_0:
                teleportString = "Prototype 3";
                break;
            case Level.level_1:
                teleportString = "Prototype 3 1";
                break;
            case Level.level_2:
                break;
            default:
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            SceneManager.LoadScene(teleportString);
        }
    }
    public void ReloadScene()
    {
        switch (currentLevel)
        {
            case Level.level_1:
                SceneManager.LoadScene("Prototype 3");
                break;
            case Level.level_2:
                SceneManager.LoadScene("Prototype 3 1");
                break;
            default:
                break;
        }
    }
}
