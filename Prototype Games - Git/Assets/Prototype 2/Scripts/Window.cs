using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Window : MonoBehaviour
{
    private bool player1Here;
    private bool player2Here;
    private float currentTimer;
    private float skillInterval;
    public GameObject Player1;
    public GameObject Player2;
    private bool isAnimating = false;
    private Color neutral = new Color(255f, 255f, 255f, 0.4f);
    private Color yellow = new Color(1f, 0.87f, 0f, 0.5f);
    private Color green = new Color(0, 1f, 0f, 0.5f);
    private SpriteRenderer childRend;
    void Start()
    {
        skillInterval = gameObject.GetComponent<SkillCheck>().skillInterval;
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
        if ((player1Here && !player2Here) || (!player1Here && player2Here))
        {
            childRend.color = yellow;
        }
        if (player1Here && player2Here)
        {
            childRend.color = green;
            currentTimer += Time.deltaTime;
            if (!isAnimating && currentTimer > skillInterval/2)
            {
                isAnimating = true;
            }
            if (currentTimer > skillInterval)
            {
                Player1.transform.position = new Vector2(-12.97f, 7.4f);
                Player2.transform.position = new Vector2(-11.2f, 7.4f);
                currentTimer = 0;
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
