using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Level1 : MonoBehaviour
{
    private Text message;
    public string[] tutorials = new string[2];
    void Start()
    {
        message = transform.GetChild(0).GetComponentInChildren<Text>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            message.enabled = true;
            if (collision.transform.position.x < 12)
            {
                message.text = tutorials[0];
            }
            else
            {
                message.text = tutorials[1];
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            message.enabled = false;
        }

    }
}
