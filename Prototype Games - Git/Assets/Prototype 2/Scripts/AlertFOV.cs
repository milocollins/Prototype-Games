using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlertFOV : MonoBehaviour
{
    private GameObject player;
    private GuardScript parent;
    private bool hitPlayer = false;
    private void Start()
    {
        parent = transform.parent.gameObject.GetComponent<GuardScript>();
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            player = collision.gameObject;
            GetComponent<SpriteRenderer>().color = new Color(1f, 0.87f, 0f, 0.5f);
            hitPlayer = parent.RayCast(player);
            if (hitPlayer)
            {
                parent.StartCoroutine("AlertIconToggle");
                gameObject.SetActive(false);
            }
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        GetComponent<SpriteRenderer>().color = new Color(0, 1f, 0f, 0.5f);
    }
}
