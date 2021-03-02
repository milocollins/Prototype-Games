using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class City : MonoBehaviour
{
    public GameObject child;
    public GameObject explosion;
    private BoxCollider2D bco;
    private SpriteRenderer sr;

    public int index;
    public int MaxHealth;
    internal bool shield = false;
    internal int Health;

    void Awake()
    {
        bco = gameObject.GetComponent<BoxCollider2D>();
        sr = gameObject.GetComponent<SpriteRenderer>();
    }
    void Start()
    {
        Health = MaxHealth;
    }

    void Update()
    {
        //Shield Colour
        if (shield == true)
        {
            sr.color = Color.blue;
        }
        else
        {
            sr.color = Color.white;
        }

        if (Health <= 0)
        {
            Instantiate(explosion, transform.position, Quaternion.identity);
            SFXManager.SFX.StartCoroutine("PlaySFX", "City_Death");
            GameManager.TheManager.GameLoss();
            Destroy(gameObject);
        }
    }

    public  void TakeDamage(int dmg)
    {
        if (!shield)
        {
            Health -= dmg;
            child.GetComponent<CityHealth>().ChangeState(Health);
        }
        SFXManager.SFX.StartCoroutine("PlaySFX","City_Hit");
    }

    public void HealthRegen()
    {
        if(Health != MaxHealth)
        {
            ++Health;
            child.GetComponent<CityHealth>().ChangeState(Health);
        }
    }
}
