using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorLock : MonoBehaviour
{
    public bool isLocked = false;
    private bool isOpen;
    private Animation MyAnim;

    void Start()
    {
        MyAnim = GetComponent<Animation>();
        isOpen = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Triggered");
            DoorAnimation();
        }
    }
    public void DoorAnimation()
    {
        if (!isLocked)
        {
            if (!isOpen)
            {
                MyAnim.Play("Side_Door_Open");
            }/*
            else
            {
                MyAnim.Play("DoorClose1");
            }*/
            isOpen = !isOpen;
        }
    }   
}
