using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public float Interval;
    public float scaleFactor;
    public int defaultTicks;
    public int nukeTicks;
    internal bool nuke = false;
    internal Vector3 scaleVec;
    internal int scaleTicks = 0;
    private int currentTicks;
    private float currentTimer;

    void Start()
    {
        if (!nuke)
        {
            scaleTicks = defaultTicks;
        }
        else
        {
            scaleTicks = nukeTicks;
        }
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
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (nuke)
        {
            if (collision.CompareTag("City"))
            {
                collision.GetComponent<City>().TakeDamage(2);
            }
            if (collision.CompareTag("Enemy"))
            {
                Destroy(collision.gameObject);
            }
            if (collision.CompareTag("Missile"))
            {
                GameManager.TheManager.StartCoroutine("ExplosionTimer");
                Destroy(collision.gameObject);
            }
        }
    }
}
