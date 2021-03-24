using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu3 : MonoBehaviour
{
    public float lerpRate;
    public Image Fade;
    private bool fadeTrigger = true;
    private float timer;
    public float timeInterval;
    private bool endTrigger;
    public AudioSource menuClip;

    private void Start()
    {
        fadeTrigger = true;
        endTrigger = false;
        timer = 0f;
        Fade.color = Color.black;
        Fade.gameObject.SetActive(true);
        Time.timeScale = 1f;
        menuClip.volume = 0;
    }
    void Update()
    {
        if (fadeTrigger)
        {
            timer += Time.deltaTime;
            if (timer > timeInterval)
            {
                Fade.color = Color.clear;
                menuClip.volume = 1;
            }
            if (Fade.color != Color.clear)
            {
                Fade.color = Color.Lerp(Fade.color, Color.clear, lerpRate*Time.deltaTime);
                menuClip.volume = Mathf.Lerp(menuClip.volume, 1, lerpRate * Time.deltaTime);
            }
            else
            {
                Fade.gameObject.SetActive(false);
                fadeTrigger = false;
            }
        }
        else if (endTrigger)
        {
            timer += Time.deltaTime;
            if (timer > timeInterval)
            {
                Fade.color = Color.black;
                menuClip.volume = 0;
            }
            if (Fade.color != Color.black)
            {
                Fade.color = Color.Lerp(Fade.color, Color.black, lerpRate * Time.deltaTime);
                menuClip.volume = Mathf.Lerp(menuClip.volume, 0, lerpRate * Time.deltaTime);
            }
            else
            {
                SceneManager.LoadScene("Prototype 3 0");
            }
        }
    }
    public void PlayButton()
    {
        timer = 0f;
        endTrigger = true;
        Fade.gameObject.SetActive(true);
    }
}
