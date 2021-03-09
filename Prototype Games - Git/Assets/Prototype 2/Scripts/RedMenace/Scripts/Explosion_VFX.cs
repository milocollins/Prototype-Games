using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion_VFX : MonoBehaviour
{
    public float Interval;
    public float scaleFactor;
    public int scaleTicks;
    internal Vector3 scaleVec;
    private int currentTicks;
    private float currentTimer;

    void Start()
    {
        currentTimer = 0;
        currentTicks = 0;
    }

    void Update()
    {
        currentTimer += Time.deltaTime;
        if (currentTimer > Interval)
        {
            currentTimer = 0;
            ++currentTicks;
            if (currentTicks == scaleTicks)
            {
                Destroy(gameObject);
            }
            else
            {
                Scale();
            }
        }
    }

    private void Scale()
    {
        scaleFactor *= scaleFactor;
        scaleVec = new Vector3(scaleFactor, scaleFactor, 1);
        transform.localScale = scaleVec;
        return;
    }
}
