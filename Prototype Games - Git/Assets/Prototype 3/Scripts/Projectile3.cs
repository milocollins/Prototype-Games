using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile3 : MonoBehaviour
{
    [Header ("Internal Parameters:")]
    public int damage;
    public int speed;
    public float RNG;

    [Header("Level Parameters:")]
    public float minXBoundary;
    public float maxXBoundary;
    public float maxYBoundary;

    private Vector2 localOrigin;
    private Vector2 localTarget;
    private Rigidbody2D MyRigid;

    private void Start()
    {
        MyRigid = GetComponent<Rigidbody2D>();
        MyRigid.velocity = Vector3.Normalize(GetTarget() - GetOrigin()) * speed;
        float angle = Mathf.Atan2(localTarget.y - localOrigin.y, localTarget.x - localOrigin.x)*Mathf.Rad2Deg;
        transform.localRotation = Quaternion.Euler(0, 0, angle + 90);
    }

    private Vector2 GetOrigin()
    {
        float x = Random.Range(minXBoundary, maxXBoundary);
        Vector2 vec;
        vec = new Vector2(x, maxYBoundary);
        transform.position = vec;
        localOrigin = vec;
        return vec;

    }
    private Vector2 GetTarget()
    {
        float r = Random.Range(-RNG, RNG);
        float x = Random.Range(minXBoundary, maxXBoundary);
        Vector2 vec;
        vec = new Vector2(Mathf.Clamp(Player.thePlayer.transform.position.x + r, minXBoundary -1f, maxXBoundary + 1f), Player.thePlayer.transform.position.y);
        localTarget = vec;
        return vec;
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
