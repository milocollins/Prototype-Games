using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMenu : MonoBehaviour
{
    private SFXManager1 thisSFX;
    private GameObject Title;
    private GameObject button0;
    private GameObject button1;

    public string title;
    public Color titleColor;
    public string loopSFX;
    public string button0name;
    public string button1name;
    public bool StartMenu;

    private void Awake()
    {
        thisSFX = GameObject.Find("SFX Manager").GetComponent<SFXManager1>();
        Title = GameObject.Find("Title");
        button0 = GameObject.Find("Button0");
        button1 = GameObject.Find("Button1");
    }
    private void Start()
    {
        thisSFX.StartLoop(loopSFX);
        Title.GetComponent<Text>().text = title;
        Title.GetComponent<Text>().color = titleColor;
        button0.GetComponentInChildren<Text>().text = button0name;
        button1.GetComponentInChildren<Text>().text = button1name;
    }
    public void Button_Click(string s)
    {
        SceneNav.NavTo(s);
    }
}
