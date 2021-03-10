using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager1 : MonoBehaviour
{
    public static GameManager1 TheManager1;
    public List<GameObject> securityCameras;
    public GameObject Guard;
    public GameObject MainCamera;
    private GameObject backgroundGO;
    internal GameObject chaseTarget;
    internal bool camerasActive = true;
    public bool displayAlarmActive = true;
    public GameObject theAsset;
    public GameObject van;
    public Sprite inactiveAlarm;
    private bool Countdown = false;
    private float currentTimer;
    public float timerInterval;
    private Text time;
    public GameObject pauseMenu;

    internal enum GameState
    {
        inProgress,
        win,
        lose
    }
    internal GameState currentGameState = GameState.inProgress;
    public enum Status
    {
        green,
        red
    }
    public Status currentStatus;
    void Start()
    {
        time = transform.GetComponentInChildren<Canvas>().GetComponentInChildren<Text>();
        transform.GetChild(0).gameObject.SetActive(false);
        TheManager1 = this;
        currentTimer = timerInterval;
        currentStatus = Status.green;
        foreach (var item in GameObject.FindGameObjectsWithTag("cameraNPC"))
        {
            securityCameras.Add(item);
        }
        backgroundGO = SFXManager1.SFX.StartLoop("lobby_loop");
    }
    private void Update()
    {
        if (Countdown)
        {
            currentTimer -= Time.deltaTime;
            time.text = currentTimer.ToString();
            if (currentTimer <= 0)
            {
                EndGame(GameState.lose);
            }
        }
        if (Input.GetKeyDown(KeyCode.Joystick1Button9) || Input.GetKeyDown(KeyCode.Joystick2Button9))
        {
            if (!GameObject.Find("Pause Menu"))
            {
                Instantiate(pauseMenu);
            }
        }
    }
    public void RedAlert()
    {
        if (currentStatus != Status.red)
        {
            transform.GetChild(0).gameObject.SetActive(true);
            Countdown = true;
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
    public void DeactivateCameras()
    {
        camerasActive = false;
        foreach (var item in securityCameras)
        {
            item.GetComponent<SecurityCamera>().cameraActive = false;
            item.transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
        }
        SFXManager1.SFX.StartCoroutine("PlaySFX", "beep_SFX");
    }
    public void DeactivateDisplayAlarm()
    {
        displayAlarmActive = false;
        GameObject.Find("Alarm Panel").GetComponent<SpriteRenderer>().sprite = inactiveAlarm;
        SFXManager1.SFX.StartCoroutine("PlaySFX", "beep_SFX");
    }
    public void DisplayAlarmCheck()
    {
        if (displayAlarmActive)
        {
            RedAlert();
        }
        theAsset.SetActive(false);
        van.GetComponent<Van>().haveAsset = true;

    }
    internal void EndGame(GameManager1.GameState gs)
    {
        currentGameState = gs;
        string navToString ="";
        switch (currentGameState)
        {
            case GameState.inProgress:
                navToString = "Prototype_2_Pause";
                break;
            case GameState.win:
                navToString = "Prototype_2_Win";
                break;
            case GameState.lose:
                navToString = "Prototype_2_Lose";
                break;
            default:
                break;
        }
        SceneNav.NavTo(navToString);
    }
}
