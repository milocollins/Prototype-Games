using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    public Sprite HealthS;
    public Sprite ShieldS;
    public Sprite MissileS;
    public float speed;
    public float shieldTimer;
    internal Type powerup;
    private GameObject[] cityArray;
    private Rigidbody2D MyRigid;

    public enum Type
    {
        Shield,
        Health,
        MissileT
    }
    private void Awake()
    {
        MyRigid = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        MyRigid.velocity = new Vector2(0, -speed);
        cityArray = GameObject.FindGameObjectsWithTag("City");
        int r = Random.Range(0, 3);
        SpriteRenderer sr = gameObject.GetComponent<SpriteRenderer>();
        switch (r)
        {
            case 0:
                powerup = Type.Shield;
                sr.sprite = ShieldS;
                    break;
            case 1:
                powerup = Type.Health;
                sr.sprite = HealthS;
                break;
            case 2:
                powerup = Type.MissileT;
                sr.sprite = MissileS;
                break;
            default:
                break;
        }
    }
    private void Update()
    {
        if (transform.position.y < -4)
        {
            --GameManager.TheManager.powerupCount;
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Missile") || collision.CompareTag("Explosion"))
        {
            switch (powerup)
            {
                case Type.Shield:
                    foreach (GameObject GO in cityArray)
                    {
                        GO.GetComponent<City>().shield = true;
                    }
                    SFXManager.SFX.StartCoroutine("PlaySFX", "City_Shield");
                    GameManager.TheManager.shieldToggle = true;
                    break;
                case Type.Health:
                    foreach (GameObject GO in cityArray)
                    {
                        GO.GetComponent<City>().HealthRegen();
                    }
                    SFXManager.SFX.StartCoroutine("PlaySFX", "City_Health");
                    break;
                case Type.MissileT:
                    GameManager.TheManager.powerupM = true;
                    SFXManager.SFX.StartCoroutine("PlaySFX", "Powerup");
                    break;
                default:
                    break;
            }
            --GameManager.TheManager.powerupCount;
            Destroy(gameObject);
        }
    }
}