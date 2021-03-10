using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Missile : MonoBehaviour
{
    public GameObject explosion;
    private GameObject Obj;

    public int speed;
    internal bool powerup;
    private float d;
    private Vector3 coords = Vector3.zero; 
    private Vector3 r1;

    void Awake()
    {
        Obj = GameObject.Find("Crosshair");
        powerup = GameManager.TheManager.powerupM;
    }

    void Start()
    {
        if (!powerup)
        {
            coords = Obj.transform.position;
            SFXManager.SFX.StartCoroutine("PlaySFX", "Missile_Launch");
        }
        r1 = coords - transform.position;
        float r2 = (Mathf.Atan2(r1.y, r1.x) * Mathf.Rad2Deg) - 90;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, r2));
    }
    void FixedUpdate()
    {
        d = Vector3.Distance(coords, transform.position);
        if (d==0)
        {
            Instantiate(explosion, transform.position, Quaternion.identity);
            if (!powerup)
            {
                GameManager.TheManager.StartCoroutine("ExplosionTimer");
            }
            SFXManager.SFX.StartCoroutine("PlaySFX", "Missile_Explode");
            Destroy(gameObject);
        }   

    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, coords, speed * Time.deltaTime);
    }
    public void SetTarget(Vector3 newTarget)
    {
        coords = newTarget;
    }
}
