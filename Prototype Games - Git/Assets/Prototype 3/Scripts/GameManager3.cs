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
    public string teleportString;
    void Start()
    {
        teleport = GetComponent<BoxCollider2D>();
        switch (currentLevel)
        {
            case Level.level_0:
                break;
            case Level.level_1:
                break;
            case Level.level_2:
                break;
            default:
                break;
        }
    }

    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            SceneManager.LoadScene(teleportString);
        }
    }
}
