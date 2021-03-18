using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager3 : MonoBehaviour
{
    internal Slider healthBar;
    internal Slider manaBar;
    private Text healthValue;
    private Text manaValue;
    public List<GameObject> Bin = new List<GameObject>();
    private void Awake()
    {
        healthBar = transform.GetChild(0).gameObject.GetComponent<Slider>();
        manaBar = transform.GetChild(1).gameObject.GetComponent<Slider>();
        healthValue = transform.GetChild(2).gameObject.GetComponent<Text>();
        manaValue = transform.GetChild(3).gameObject.GetComponent<Text>();
    }
    private void FixedUpdate()
    {
        healthValue.text = ((healthBar.value * 10).ToString() + "/100");
        manaValue.text = ((manaBar.value * 10).ToString() + "/100");
    }
    private void Update()
    {
        foreach (var item in Bin)
        {
            Bin.Clear();
        }
    }
}
