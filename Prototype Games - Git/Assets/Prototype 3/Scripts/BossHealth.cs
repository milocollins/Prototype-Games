using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealth : MonoBehaviour
{
    private Slider bossHealth2;
    private Slider bossHealth1;
    private Slider bossHealth0;
    public static BossHealth bossHealth;
    private bool lerpTrigger = true;
    private bool lerp0 = true;
    private bool lerp1 = false;
    public float lerpRate;

    private void Awake()
    {
        bossHealth = this;
        bossHealth0 = transform.GetChild(0).GetComponent<Slider>();
        bossHealth1 = transform.GetChild(1).GetComponent<Slider>();
        bossHealth2 = transform.GetChild(2).GetComponent<Slider>();
    }
    private void Update()
    {
        if (lerpTrigger)
        {
            if (lerp0)
            {
                if (bossHealth0.maxValue - bossHealth0.value > 1f)
                {
                    bossHealth0.value = Mathf.Lerp(bossHealth0.value, bossHealth0.maxValue, lerpRate * Time.deltaTime);
                }
                else
                {
                    bossHealth0.value = bossHealth0.maxValue;
                    lerp0 = false;
                    lerp1 = true;
                }
            }
            else if (lerp1)
            {
                if (bossHealth1.maxValue - bossHealth1.value > 0.8f)
                {
                    bossHealth1.value = Mathf.Lerp(bossHealth1.value, bossHealth1.maxValue, lerpRate * Time.deltaTime);
                }
                else
                {
                    bossHealth1.value = bossHealth1.maxValue;
                    lerp1 = false;
                }
            }
            else
            {
                if (bossHealth2.maxValue - bossHealth2.value > 1f)
                {
                    bossHealth2.value = Mathf.Lerp(bossHealth2.value, bossHealth2.maxValue, lerpRate * Time.deltaTime);
                }
                else
                {
                    bossHealth2.value = bossHealth2.maxValue;
                    lerpTrigger = false;
                }
            }
        }
    }
    public void UpdateHealth()
    {
        if (Boss.theBoss.currentHealth >= Level2.levelManager.phase2Health)
        {
            bossHealth2.value = Boss.theBoss.currentHealth;
        }
        else if (Boss.theBoss.currentHealth >= Level2.levelManager.phase3Health)
        {
            bossHealth1.value = Boss.theBoss.currentHealth;
        }
        else
        {
            bossHealth0.value = Boss.theBoss.currentHealth;
        }
    }
}
