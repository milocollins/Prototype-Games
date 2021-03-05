using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class alarmFOV : MonoBehaviour
{
    private Collider2D MyCollider;
    private SecurityCamera MyParent;
    internal bool Lock = false;
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
                MyParent.alarmStatus = SecurityCamera.Alarm.yellow;
                MyParent.AlarmStatusChange(SecurityCamera.Alarm.yellow);
            }

        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (!Lock)
        {
            if (other.CompareTag("Player"))
            {
                MyParent.alarmStatus = SecurityCamera.Alarm.green;
                MyParent.AlarmStatusChange(SecurityCamera.Alarm.green);
            }
        }
    }
}
