using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plane : MonoBehaviour
{
    public float friendlySpeed;
    public float enemySpeed;
    private float speed;
    public GameObject powerup;
    public GameObject nuke;
    internal bool enemy = false;
    public Sprite enemyVariant;
    private SpriteRenderer sr;
    private Rigidbody2D MyRigid;
    private float r;
    private bool dropped = false;
    
    
    void Awake()
    {
        MyRigid = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }
    void Start()
    {
        if (enemy)
        {
            int v1;
            float v2;

            sr.sprite = enemyVariant;
            speed = enemySpeed;
            v1 = Random.Range(-1, 1);
            if (v1 == 0)
            {
                v1 = 1;
            }
            v2 = Random.Range(1.5f, 6f);
            r = v1 * v2;
            Debug.Log(r);
        }
        else
        {
            speed = friendlySpeed;
            r = Random.Range(-7, 7);
        }
        MyRigid.velocity = new Vector2(-transform.position.x * speed , 0);
        if (transform.position.x == -9)
        {
            transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
        }
    }
    private void Update()
    {
        if (ConditionCheck() && dropped == false)
        {
            if (!enemy)
            {
                Instantiate(powerup, new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
            }
            else
            {
                GameObject obj = Instantiate(nuke,new Vector2(transform.position.x, transform.position.y) ,Quaternion.identity);
                obj.GetComponent<Enemy>().nuke = true;
            }
            dropped = true;
        }
    }
    private void LateUpdate()
    {
        if( transform.position.x < -9 || transform.position.x > 9)
        {
            Destroy(gameObject);
        }
    }
    private bool ConditionCheck()
    {
        if (MyRigid.velocity.x > 0 && transform.position.x >= r)
        {
            return true;
        }
        else if (MyRigid.velocity.x < 0 && transform.position.x <= r)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
