using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartInput : MonoBehaviour
{
    private GameObject xP1;
    private GameObject xP2;
    private bool P1Ready = false;
    private bool P2Ready = false;
    public Color color1;
    public Color color2;
    void Start()
    {
        xP1 = GameObject.Find("xP1");
        xP2 = GameObject.Find("xP2");
    }

    void Update()
    {
        if (Input.GetButtonDown("Interact_1") && !P1Ready)
        {
            P1Ready = true;
            xP1.GetComponent<Image>().color = color1;
        }
        if (Input.GetButtonDown("Interact_2") && !P2Ready)
        {
            P2Ready = true;
            xP2.GetComponent<Image>().color = color2;
        }
        if (P1Ready && P2Ready)
        {
            SceneNav.NavTo("Prototype_2");
        }
    }
}
