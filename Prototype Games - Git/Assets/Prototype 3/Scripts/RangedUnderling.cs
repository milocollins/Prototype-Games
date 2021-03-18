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
            MyRigid.velocity = Vector2.zero;
        }
        #endregion
    }

    private void Attacking()
    {
        if (!cooldown && !escaping)
        {
            RaycastHit2D hit;
            if (Player.thePlayer.transform.position.x > transform.position.x)
            {
                MyRigid.velocity = transform.right * speed;
                hit = Physics2D.Raycast(transform.position, transform.right, attackRange);
            }
            else
            {
                MyRigid.velocity = -transform.right * speed;
                hit = Physics2D.Raycast(transform.position, -transform.right, attackRange);
            }
            if (hit && hit.transform.gameObject.name == "Player")
            {
                MyAnim.SetTrigger("isAttacking");
                attackTimer = 0;
                cooldown = true;
                MyRigid.velocity = Vector2.zero;
            }
        }
        else if (Mathf.Abs(Player.thePlayer.transform.position.x - transform.position.x) < attackRange/2)
        {
            escaping = true;
            if (Player.thePlayer.transform.position.x > transform.position.x)
            {
                MyRigid.velocity = -transform.right * runSpeed;
                transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else
            {
                MyRigid.velocity = transform.right * runSpeed;
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
        }
        else
        {
            MyAnim.SetTrigger("isHit");
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
            if (hit.transform.gameObject.name == "Player")
            {
                currentState = AIstate.attacking;
            }
        }
    }
}
