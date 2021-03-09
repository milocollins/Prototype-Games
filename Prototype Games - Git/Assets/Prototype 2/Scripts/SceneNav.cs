using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneNav : MonoBehaviour
{
    public string winScene;
    public string loseScene;
    private string sceneToNavTo;
    public void NavTo()
    {
        switch (GameManager1.TheManager1.currentGameState)
        {
            case GameManager1.GameState.win:
                sceneToNavTo = winScene;
                break;
            case GameManager1.GameState.lose:
                sceneToNavTo = loseScene;
                break;
            default:
                break;
        }
        SceneManager.LoadScene(sceneToNavTo);
    }
}
