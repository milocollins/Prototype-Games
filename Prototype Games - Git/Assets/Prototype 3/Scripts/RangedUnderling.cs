using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedUnderling : MonoBehaviour
{
    private Rigidbody2D MyRigid;
    private Animator MyAnim;
    public float raycastRange;
    public float speed;
    private int health;
    public int maxHealth;
    private float attackTimer = 0;
    public float attackInterval;
    public int damage;
    public float attackRange;
    public float runSpeed;
    private bool escaping = false;

    private Vector2 v;
    private Vector2 leftScale;
    private Vector2 rightScale;
    public enum AIstate
    {
        patrolling,
        attacking,
        dying
    }
    public enum Facing
    {
        left,
        right
    }
    public AIstate currentState;
    public Facing direction;
    private bool cooldown = false;

    void Start()
    {
        leftScale = new Vector2(-Mathf.Abs(transform.localScale.x), transform.localScale.y);
        rightScale = new Vector2(Mathf.Abs(transform.localScale.x), transform.localScale.y);
        health = maxHealth;
        MyRigid = GetComponent<Rigidbody2D>();
        MyAnim = GetComponent<Animator>();
        currentState = AIstate.patrolling;
        if (direction == Facing.left)
        {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
        MyRigid.velocity = Vector2.zero;
    }
    public void FixedUpdate()
    {
        MyRigid.velocity = v;
        if (v.x > 0)
        {
            transform.localScale = rightScale;
            direction = Facing.right;
        }
        else if (v.x < 0)
        {
            transform.localScale = leftScale;
            direction = Facing.left;

        }
    }
    void Update()
    {
        if (Mathf.Abs(MyRigid.velocity.x) > 0.01)
        {
            MyAnim.SetBool("isIdle", false);
        }
        else
        {
            MyAnim.SetBool("isIdle", true);
        }
        #region Patrolling
        if (currentState == AIstate.patrolling)
        {
            Patrol();
        }
        #endregion
        #region Attacking
        else if (currentState == AIstate.attacking)
        {
            Attacking();
        }
        #endregion
        #region Dying
        else if (currentState == AIstate.dying)
        {
            v = Vector2.zero;
        }
        #endregion
    }

    private void Attacking()
    {
        if (!cooldown && !escaping)
        {
            if (Mathf.Abs(Vector2.Distance(Player.thePlayer.transform.position, transform.position)) < attackRange)
            {
                RaycastHit2D hit;
                if (Player.thePlayer.transform.position.x > transform.position.x)
                {
                    hit = Physics2D.Raycast(transform.position, transform.right, attackRange);
                }
                else
                {
                    hit = Physics2D.Raycast(transform.position, -transform.right, attackRange);
                }
                if (hit)
                {
                    Debug.Log(hit.transform.gameObject.name);
                }
                if (hit && (hit.transform.gameObject.name == "Player" || hit.transform.name.Contains("Ice Wall")))
                {
                    StartCoroutine("AttackSFX");
                    MyAnim.SetTrigger("isAttacking");
                    attackTimer = 0;
                    cooldown = true;
                }
                v = Vector2.zero;
            }
            else
            {
                if (Player.thePlayer.transform.position.x > transform.position.x)
                {
                    v = transform.right * speed;
                }
                else
                {
                    v = -transform.right * speed;
                }
            }
        }
        else if (Mathf.Abs(Player.thePlayer.transform.position.x - transform.position.x) < attackRange/3)
        {
            escaping = true;
            if (Player.thePlayer.transform.position.x > transform.position.x)
            {
                v = -transform.right * runSpeed;
                transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else
            {
                v = transform.right * runSpeed;
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
        }
        else
        {
            escaping = false;

        }
        attackTimer += Time.deltaTime;
        if (attackTimer > attackInterval)
        {
            cooldown = false;
        }
    }
    public void Attack()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction == Facing.left ? -transform.right : transform.right, attackRange);
        if (hit)
        {
            if (hit.transform.gameObject.name == "Player")
            {
                Player.thePlayer.TakeDamage(damage);
            }
            else if (hit.transform.gameObject.name.Contains("Ice Wall"))
            {
                hit.transform.GetComponent<PlayerAbility>().TakeDamage();
            }
        }
    }

    public void TakeDamage(int d)
    {
            Debug.Log("isHit");
        health -= d;
        if (currentState == AIstate.patrolling)
        {
            currentState = AIstate.attacking;
        }
        if (health <= 0)
        {
            currentState = AIstate.dying;
            MyAnim.SetTrigger("isDead");
            SFXManager3.theManager.PlaySFX("Underling_Die");
        }
        else
        {
            MyAnim.SetTrigger("isHit");
            SFXManager3.theManager.PlaySFX("Underling_Hit");
        }
    }
    public void Die()
    {
        gameObject.SetActive(false);
    }
    private void Patrol()
    {
        Debug.DrawLine(transform.position, transform.position + new Vector3(raycastRange * (direction == Facing.left ? -1 : 1), 0), Color.red);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction == Facing.left ? -transform.right : transform.right, raycastRange);
        if (hit)
        {
            if (hit.transform.gameObject.name == "Player" || hit.transform.gameObject.name.Contains("Ice Wall"))
            {
                currentState = AIstate.attacking;
            }
        }
    }
    public IEnumerator AttackSFX()
    {
        SFXManager3.theManager.PlaySFX("Crossbow_Draw");
        yield return new WaitForSeconds(0.8f);
        SFXManager3.theManager.PlaySFX("Underling_Arrow");
    }
}
