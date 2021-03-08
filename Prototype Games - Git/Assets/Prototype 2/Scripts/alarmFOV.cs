using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class alarmFOV : MonoBehaviour
{
    private Collider2D MyCollider;
    private SecurityCamera MyParent;
    internal bool Lock = false;
    private bool hitPlayer = false;
    void Start()
    {
        MyCollider = GetComponent<Collider2D>();
        MyParent = transform.parent.transform.parent.gameObject.GetComponent<SecurityCamera>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!Lock)
        {
            if (collision.CompareTag("Player"))
            {
                hitPlayer = MyParent.RayCast(collision.gameObject);
                if (hitPlayer)
                {
                    GameManager1.TheManager1.chaseTarget = collision.gameObject;
                    GameManager1.TheManager1.RedAlert();
                }
            }

        }
    }
}
