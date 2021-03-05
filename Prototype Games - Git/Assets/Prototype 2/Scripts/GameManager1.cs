using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager1 : MonoBehaviour
{
    public static GameManager1 TheManager1;
    public enum Status
    {
        green,
        red
    }
    public Status currentStatus;
    void Start()
    {
        TheManager1 = this;
        currentStatus = Status.green;
    }

    void Update()
    {
        
    }
}
