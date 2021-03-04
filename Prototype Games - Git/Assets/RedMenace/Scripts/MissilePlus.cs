using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissilePlus : MonoBehaviour
{
    public GameObject explosion;
    public GameObject missile;
    private GameObject Obj;
    private GameObject projectile1;
    private GameObject projectile2;

    public int speed;
    public float missileSplitDistance;
    private float d0;
    internal bool splitToggle = false;
    internal bool powerup;
    private float d;
    private Vector3 coords;
    private Vector3 coords1;
    private Vector3 r1;

    void Awake()
    {
        Obj = GameObject.Find("Crosshair");
    }

    void Start()
    {
        coords = Obj.transform.position;
        coords1 = coords;
        //New missiles direction
        coords1 = Vector2.Perpendicular(new Vector2(coords.x, coords.y));
        coords1 = Vector3.Normalize(coords1);
        d0 = Vector3.Distance(coords, transform.position);
        r1 = coords - transform.position;
        float r2 = (Mathf.Atan2(r1.y, r1.x) * Mathf.Rad2Deg) - 90;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, r2));
        SFXManager.SFX.StartCoroutine("PlaySFX", "Missile_Launch");
    }
    void FixedUpdate()
    {
        d = Vector3.Distance(coords, transform.position);
        if (d == 0)
        {
            Instantiate(explosion, transform.position, Quaternion.identity);
            SFXManager.SFX.StartCoroutine("PlaySFX", "Missile_Explode");
            GameManager.TheManager.StartCoroutine("ExplosionTimer");
            Destroy(gameObject);
        }

    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, coords, speed * Time.deltaTime);
        if(d < d0 / 2 && !splitToggle)
        {
            splitToggle = true;
            projectile1 = Instantiate(missile, transform.position, Quaternion.identity);
            projectile1.GetComponent<Missile>().SetTarget(coords + coords1 * missileSplitDistance);
            projectile2 = Instantiate(missile, transform.position, Quaternion.identity);
            projectile2.GetComponent<Missile>().SetTarget(coords - coords1 * missileSplitDistance);
        }
    }
}
