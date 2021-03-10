using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject explosionVFX;
    public GameObject explosion;
    public Sprite NukeSprite;
    private Rigidbody2D RB;
    private SpriteRenderer sr;
    public float cityY;
    public float nukeSpeed;
    public bool nuke = false;
    private GameObject[] cityActive;
    private GameObject Target;
    private GameObject obj;

    public int defaultHealth;
    public int nukeHealth;
    private int HP;
    public int speed;
    private int TargetIndex;
    private Vector3 TargetPosition;

    private void Awake()
    {
        RB = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }
    void Start()
    {
        if (!nuke)
        {
            HP = defaultHealth;
            cityActive = GameObject.FindGameObjectsWithTag("City");
            TargetIndex = cityActive[Random.Range(0, cityActive.Length)].GetComponent<City>().index;
            switch (TargetIndex)
            {
                case 0:
                    Target = GameObject.Find("City_0");
                    break;
                case 1:
                    Target = GameObject.Find("City_1");
                    break;
                case 2:
                    Target = GameObject.Find("City_2");
                    break;
                case 3:
                    Target = GameObject.Find("City_3");
                    break;
                case 4:
                    Target = GameObject.Find("City_4");
                    break;
                case 5:
                    Target = GameObject.Find("City_5");
                    break;
                default:
                    break;
            }
        
            TargetPosition = Target.transform.position;

            Vector3 r1 = TargetPosition - transform.position;
            float r2 = (Mathf.Atan2(r1.y, r1.x) * Mathf.Rad2Deg) - 90;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, r2));
        }
        else
        {
            HP = nukeHealth;
            sr.sprite = NukeSprite;
            RB.velocity = new Vector2(0, -nukeSpeed);
        }
    }
    private void FixedUpdate()
    {
        if (Target == null && !nuke)
        {
            Destroy(gameObject);
        }
    }
    void Update()
    {
        if (!nuke)
        {
            transform.position = Vector3.MoveTowards(transform.position, TargetPosition, speed * Time.deltaTime);
            //posible bug fix for origin
            if (transform.position == Vector3.zero)
            {
                transform.position += new Vector3(0.01f, 0.01f);
            }
        }
        else
        {
            if(transform.position.y <= cityY)
            {
                obj = Instantiate(explosion, transform.position, Quaternion.identity);
                obj.GetComponent<Explosion>().nuke = true;
                Destroy(gameObject);
            }
        }
    }
    private void LateUpdate()
    {
        if (HP <= 0)
        {
            GameManager.TheManager.EnemyKill(nuke);
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Missile"))
        {
            if (collision.CompareTag("City") && !nuke)
            {
                Instantiate(explosionVFX, Target.transform.position, Quaternion.identity);
                collision.gameObject.GetComponent<City>().TakeDamage(1);
                Destroy(gameObject);
            }
            if (collision.CompareTag("Explosion"))
            {
                --HP;
            }
        }
    }
}
