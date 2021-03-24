using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile2 : MonoBehaviour
{
    public int damage;
    public int speed;

    private Vector3 Target;
    private Vector3 Parent;
    private Rigidbody2D MyRigid;
    private Animator MyAnim;

    private void Start()
    {
        Parent = Boss.theBoss.gameObject.transform.position;
        Target = Player.thePlayer.transform.position;
        transform.position = Parent;
        MyRigid = GetComponent<Rigidbody2D>();
        MyAnim = GetComponent<Animator>();
        MyRigid.velocity = Vector3.Normalize(Target - Parent) * speed;
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            collider.GetComponent<Player>().TakeDamage(damage);
            MyAnim.SetTrigger("isHit");
            MyRigid.bodyType = RigidbodyType2D.Static;
        }
        if (collider.CompareTag("Boundary"))
        {
            gameObject.SetActive(false);
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
    public void HitSFX()
    {
        GetComponent<AudioSource>().enabled = false;
        SFXManager3.theManager.PlaySFX("Projectile2 Hit");
    }
    public void SelfDestruct()
    {
        Destroy(gameObject);
    }
}
