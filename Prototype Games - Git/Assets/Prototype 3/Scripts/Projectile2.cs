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

    private void Start()
    {
        Parent = Boss.theBoss.gameObject.transform.position;
        Target = Player.thePlayer.transform.position;
        transform.position = Parent;
        MyRigid = GetComponent<Rigidbody2D>();
        MyRigid.velocity = Vector3.Normalize(Target - Parent) * speed;
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            collider.GetComponent<Player>().TakeDamage(damage);
            gameObject.SetActive(false);
        }
        if (collider.CompareTag("Boundary"))
        {
            gameObject.SetActive(false);
        }
        if (collider.transform.gameObject.name.Contains("Ice Wall"))
        {
            collider.gameObject.GetComponent<PlayerAbility>().TakeDamage();
            gameObject.SetActive(false);
        }
    }
}
