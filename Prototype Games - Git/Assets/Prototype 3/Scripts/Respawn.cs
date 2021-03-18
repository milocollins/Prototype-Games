using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{
    private BoxCollider2D thisBox;
    void Start()
    {
        thisBox = GetComponent<BoxCollider2D>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            if (0 < collision.transform.position.x && collision.transform.position.x  < 8)
            {
                Debug.Log("0");
                collision.transform.position = transform.GetChild(0).position;
            }
            else
            {
                Debug.Log("1");
                collision.transform.position = transform.GetChild(1).position;
            }
        }
    }
}
