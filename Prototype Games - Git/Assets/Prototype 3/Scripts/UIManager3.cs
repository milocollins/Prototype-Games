using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager3 : MonoBehaviour
{
    public static UIManager3 theManager;
    public Sprite[] healthSprites = new Sprite[16];
    public Sprite[] manaSprites = new Sprite[6];
    private Image healthRenderer;
    private Image manaRenderer;
    private GameObject DeathScreen;
    private GameObject winScreen;
    private Color color1 = new Color(0f,0f,0f,0f);
    private Color color2 = new Color(0f, 0f, 0f, 1f);
    private Color color3 = new Color(1f,0f,0f,0f);
    private Color color4 = new Color(1f,0f,0f,1f);
    private Color color5 = new Color(1f, 1f, 1f, 1f);
    private Color color6 = new Color(1f, 1f, 1f, 0f);
    public float lerpRate;
    public float winLerpRate;
    private bool die0 = false;
    private bool die1 = false;
    private bool win = false;
    private void Start()
    {
        DeathScreen.transform.GetChild(0).GetComponent<Image>().color = color1;
        DeathScreen.transform.GetChild(0).GetChild(0).GetComponent<Text>().color = color3;
        winScreen.transform.GetComponent<Image>().color = color6;
    }
    private void Update()
    {
        if (die0)
        {
            Image temp1 = DeathScreen.transform.GetChild(0).GetComponent<Image>();
            Text temp3 = DeathScreen.transform.GetChild(0).GetChild(0).GetComponent<Text>();
            temp1.color = Color.Lerp(temp1.color, color2, lerpRate*Time.deltaTime);
            temp3.color = Color.Lerp(temp3.color, color4, lerpRate*Time.deltaTime);
        }
        else if (die1)
        {
            Image temp2 = DeathScreen.transform.GetChild(0).GetComponent<Image>();
            temp2.color = Color.Lerp(temp2.color, color5, lerpRate * Time.deltaTime);
        }
        else if (win)
        {
            Image temp4 = winScreen.transform.GetComponent<Image>();
            temp4.color = Color.Lerp(temp4.color, color5, winLerpRate * Time.deltaTime);
        }
    }
    public void Awake()
    {
        theManager = this;
        healthRenderer = transform.GetChild(0).GetChild(1).GetComponent<Image>();
        manaRenderer = transform.GetChild(0).GetChild(0).GetComponent<Image>();
        DeathScreen = transform.GetChild(1).gameObject;
        winScreen = transform.GetChild(2).gameObject;
        DeathScreen.SetActive(false);
    }
    public void UpdatePlayerStats()
    {
        if (Player.thePlayer.health > 0)
        {
            healthRenderer.sprite = healthSprites[Player.thePlayer.health];
            manaRenderer.sprite = manaSprites[Player.thePlayer.mana];
        }
        else
        {
            healthRenderer.sprite = healthSprites[0];
            manaRenderer.enabled = false;
        }
    }
    public void Die()
    {
        die0 = true;
        DeathScreen.SetActive(true);
    }
    public void Die1()
    {
        die0 = false;
        die1 = true;
    }
    public void Win()
    {
        Player.thePlayer.inputLock = true;
        win = true;
    }
}
