using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecurityCamera : MonoBehaviour
{
    private GameObject child;
    public float range;
    public float rotateSpeed;
    private float rotatePosition = 0f;
    public float maxAngle;
    public float changeDirectionInterval;
    private float currentTimer;
    private Vector3 raycastDirection;
    private SpriteRenderer AlarmSprite;
    private SpriteRenderer FOVSprite;
    private Color FOVGreen = new Color(0, 1f, 0f, 0.5f);
    private Color FOVYellow = new Color(1f, 0.87f, 0f, 0.5f);
    private Color FOVRed = new Color(1f, 0f, 0.07f, 0.5f);
    private Collider2D FOV;
    public enum Facing
    {
        left,
        right
    }
    public enum Alarm
    {
        green,
        yellow,
        red
    }
    public Facing thisDirection;
    public Alarm alarmStatus;

    private void Start()
    {
        child = transform.GetChild(0).gameObject;
        AlarmSprite = transform.GetChild(2).gameObject.GetComponent<SpriteRenderer>();
        FOVSprite = transform.GetChild(0).GetChild(0).gameObject.GetComponent<SpriteRenderer>();
        FOV = transform.GetChild(0).GetChild(0).gameObject.GetComponent<Collider2D>();
        FOVSprite.color = FOVGreen;
        currentTimer = 0;
    }
    private void FixedUpdate()
    {
        rotatePosition = Mathf.Clamp(rotatePosition + rotateSpeed*Time.deltaTime, -maxAngle, maxAngle);
        child.transform.rotation = Quaternion.Euler(0,0,rotatePosition);
    }
    private void Update()
    {
        switch (thisDirection)
        {
            case Facing.left:
                raycastDirection = -child.transform.right;
                break;
            case Facing.right:
                raycastDirection = child.transform.right;
                break;
        }
        Debug.DrawLine(child.transform.position, child.transform.position + raycastDirection * range, Color.green);
        RaycastHit2D hit = Physics2D.Raycast(child.transform.position, raycastDirection, range);
        if (hit.transform != null && hit.transform.gameObject.CompareTag("Player"))
        {
            Debug.Log("HIT " + hit.transform.name);
            alarmStatus = Alarm.red;
            AlarmStatusChange(alarmStatus);
        }
        if (Mathf.Abs(rotatePosition) == maxAngle)
        {
            if (currentTimer < changeDirectionInterval)
            {
                currentTimer += Time.deltaTime;
            }
            else
            {
                rotateSpeed = -rotateSpeed;
                currentTimer = 0;
            }
        }
    }

    internal void AlarmStatusChange(Alarm a)
    {
        switch (a)
        {
            case Alarm.red:
                AlarmSprite.color = Color.red;
                FOVSprite.color = FOVRed;
                child.GetComponentInChildren<alarmFOV>().Lock = true;
                GameManager1.TheManager1.currentStatus = GameManager1.Status.red;
                break;
            case Alarm.yellow:
                AlarmSprite.color = Color.yellow;
                FOVSprite.color = FOVYellow;
                break;
            case Alarm.green:
                AlarmSprite.color = Color.green;
                FOVSprite.color = FOVGreen;
                break;
        }
    }
}
