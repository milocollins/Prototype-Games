using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager UI;
    public Text pause;
    public Text game_over;
    public Text score;
    public Text stage_survived;

    void Start()
    {
        UI = this;
    }

    void Update()
    {
        score.text = GameManager.TheManager.score.ToString("0000000");
    }
    public void GameOver()
    {
        game_over.enabled = true;
    }
    public void GameWon()
    {
        stage_survived.enabled = true;
    }
    public void PauseToggle()
    {
        if (pause.IsActive())
        {
            pause.enabled = false;
        }
        else
        {
            pause.enabled = true;
        }
    }
}
