using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager1 : MonoBehaviour
{
    public static GameManager1 TheManager1;
    public List<GameObject> securityCameras;
    public GameObject Guard;
    public GameObject MainCamera;
    private GameObject backgroundGO;
    internal GameObject chaseTarget;
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
        foreach (var item in GameObject.FindGameObjectsWithTag("cameraNPC"))
        {
            securityCameras.Add(item);
        }
        backgroundGO = SFXManager1.SFX.StartLoop("lobby_loop");
    }

    public void RedAlert()
    {
        if (currentStatus != Status.red)
        {
            currentStatus = Status.red;
            foreach (var item in securityCameras)
            {
                item.GetComponent<SecurityCamera>().AlarmStatusChange(Status.red);
            }
            Guard.GetComponent<GuardScript>().LocalRedAlert();
            Guard.GetComponent<GuardScript>().chaseTarget = chaseTarget;
            if (backgroundGO)
            {
                backgroundGO.SetActive(false);
            }
            backgroundGO = null;
            backgroundGO = SFXManager1.SFX.StartLoop("alarm_loop");
            SFXManager1.SFX.StartCoroutine("PlaySFX","chase");
        }
    }
}
