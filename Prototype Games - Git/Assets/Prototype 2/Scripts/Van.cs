using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Van : MonoBehaviour
{
    private bool player2Here = false;
    private bool player1Here = false;
    private Color neutral = new Color(255f, 255f, 255f, 0.4f);
    private Color yellow = new Color(1f, 0.87f, 0f, 0.5f);
    private SpriteRenderer childRend;
    public bool haveAsset = false;

    private void Start()
    {
        childRend = transform.GetChild(0).GetComponent<SpriteRenderer>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            switch (collision.GetComponent<PlayerScript>().ID)
            {
                case PlayerScript.PlayerID._1:
                    player1Here = true;
                    break;
                case PlayerScript.PlayerID._2:
                    player2Here = true;
                    break;
            }
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (haveAsset)
        {
            if ((player1Here && !player2Here) || (!player1Here && player2Here))
            {
                childRend.color = yellow;
            }
            if (player1Here && player2Here)
            {
                GameManager1.TheManager1.EndGame(GameManager1.GameState.win);
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            switch (collision.GetComponent<PlayerScript>().ID)
            {
                case PlayerScript.PlayerID._1:
                    player1Here = false;
                    break;
                case PlayerScript.PlayerID._2:
                    player2Here = false;
                    break;
            }
        }
        if (!player1Here && !player2Here)
        {
            childRend.color = neutral;
        }
    }
}
