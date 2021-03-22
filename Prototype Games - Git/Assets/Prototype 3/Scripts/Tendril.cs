using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tendril : MonoBehaviour
{
    public int damage;

    internal Animator MyAnim;

    private void Start()
    {
        MyAnim = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            Player.thePlayer.TakeDamage(damage);
        }
    }
    public void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
