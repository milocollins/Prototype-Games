using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecurityCamera : MonoBehaviour
{
    private GameObject child;
    public float range;
    public float rotateSpeed;
    private float rotatePosition;
    public float maxAngle;
    public float maxAngleConstraint;
    public float minAngleConstraint;
    public float changeDirectionInterval;
    private float currentTimer;
    private Vector3 raycastDirection;
    private SpriteRenderer AlarmSprite;
    private SpriteRenderer FOVSprite;
    private Color FOVGreen = new Color(0, 1f, 0f, 0.5f);
    private Color FOVRed = new Color(1f, 0f, 0.07f, 0.5f);
    private Collider2D FOV;
    public enum Facing
    {
        left,
        right
    }
    public Facing thisDirection;
    internal GameManager1.Status alarmStatus;

    private void Start()
    {
        alarmStatus = GameManager1.Status.green;
        child = transform.GetChild(0).gameObject;
        rotatePosition = child.transform.localEulerAngles.z;
        maxAngleConstraint = child.transform.localEulerAngles.z + maxAngle;
        minAngleConstraint = child.transform.localEulerAngles.z - maxAngle;
        AlarmSprite = transform.GetChild(2).gameObject.GetComponent<SpriteRenderer>();
        FOVSprite = transform.GetChild(0).GetChild(0).gameObject.GetComponent<SpriteRenderer>();
        FOV = transform.GetChild(0).GetChild(0).gameObject.GetComponent<Collider2D>();
        FOVSprite.color = FOVGreen;
        currentTimer = 0;
    }
    private void FixedUpdate()
    {
        rotatePosition = Mathf.Clamp(rotatePosition + rotateSpeed*Time.deltaTime, minAngleConstraint, maxAngleConstraint);
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
    private void LateUpdate()
    {
            
    }

    internal void AlarmStatusChange(GameManager1.Status a)
    {
        switch (a)
        {
            case GameManager1.Status.red:
                AlarmSprite.color = Color.red;
                FOVSprite.color = FOVRed;
                child.GetComponentInChildren<alarmFOV>().Lock = true;
                break;
            case GameManager1.Status.green:
                AlarmSprite.color = Color.green;
                FOVSprite.color = FOVGreen;
                break;
        }
    }
    public bool RayCast(GameObject p)
    {
        bool hitPlayer = false;
        LayerMask pMask = LayerMask.GetMask("Player", "Walls", "Interactable");
        Debug.DrawLine(transform.position, p.transform.position, Color.red);
        RaycastHit2D hit = Physics2D.Raycast(child.transform.position, p.transform.position - child.transform.position, 10f, pMask);
        if (hit && hit.transform.gameObject.CompareTag("Player"))
        {
            hitPlayer = true;
        }
        return hitPlayer;
    }
}
