using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile3 : MonoBehaviour
{
    [Header ("Internal Parameters:")]
    public int damage;
    public int speed;
    public float timerInterval;

    [Header("Level Parameters:")]
    public float minXBoundary;
    public float maxXBoundary;
    public float maxYBoundary;

    private Vector2 localOrigin;
    private Rigidbody2D MyRigid;
    private Animator MyAnim;
    private float timer;


    private void Start()
    {
        timer = 0f;
        MyRigid = GetComponent<Rigidbody2D>();
        MyAnim = GetComponent<Animator>();
        MyRigid.velocity = Vector3.Normalize(Player.thePlayer.transform.position - GetOrigin()) * speed;
    }
    private void Update()
    {
        if (Check())
        {
            timer += Time.deltaTime;
            if (timer < timerInterval)
            {
                Homing();
                FacePlayer();
            }
        }
    }
    private Vector3 GetOrigin()
    {
        float x = Random.Range(minXBoundary, maxXBoundary);
        Vector2 vec;
        vec = new Vector3(x, maxYBoundary, 0f);
        transform.position = vec;
        localOrigin = vec;
        return vec;

    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            collider.GetComponent<Player>().TakeDamage(damage);
            MyAnim.SetTrigger("isHit");
            MyRigid.bodyType = RigidbodyType2D.Static;
        }
        if (collider.CompareTag("Environment"))
        {
            MyAnim.SetTrigger("isHit");
            MyRigid.bodyType = RigidbodyType2D.Static;
        }
        if (collider.transform.gameObject.name.Contains("Ice Wall"))
        {
            collider.gameObject.GetComponent<PlayerAbility>().TakeDamage();
            MyAnim.SetTrigger("isHit");
            MyRigid.bodyType = RigidbodyType2D.Static;
        }
        
    }
    public bool Check()
    {
        if (transform.position.y> Player.thePlayer.transform.position.y)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public void Homing()
    {
        MyRigid.velocity = Vector3.Normalize(Player.thePlayer.transform.position - transform.position) * speed;
    }
    public void FacePlayer()
    {
        float angle = Mathf.Atan2(Player.thePlayer.transform.position.y - localOrigin.y, Player.thePlayer.transform.position.x - localOrigin.x) * Mathf.Rad2Deg;
        transform.localRotation = Quaternion.Euler(0, 0, angle + 90);
    }
    private void Anim()
    {
        MyAnim.SetTrigger("isHit");
        this.enabled = false;
    }
    public void HitSFX()
    {
        SFXManager3.theManager.PlaySFX("Flameball Hit");
    }
    public void SelfDestruct()
    {
        Destroy(gameObject);
    }
}
