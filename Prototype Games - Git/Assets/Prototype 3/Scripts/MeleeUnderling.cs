using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeUnderling : MonoBehaviour
{
    private Rigidbody2D MyRigid;
    private Animator MyAnim;
    public float raycastRange;
    public float speed;
    private int health;
    public int maxHealth;
    private bool cooldown = false;

    private float attackTimer = 0;
    public float attackInterval;
    public int damage;
    public float attackRange;

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

    void Start()
    {
        health = maxHealth;
        MyRigid = GetComponent<Rigidbody2D>();
        MyAnim = GetComponent<Animator>();
        currentState = AIstate.patrolling;
        if (direction == Facing.left)
        {
            Debug.Log("");
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
    }

    void Update()
    {
        if (Mathf.Abs(MyRigid.velocity.x) >0.01)
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
    public void TakeDamage(int d)
    {
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
            else
            {
                if (direction == Facing.left)
                {
                    direction = Facing.right;
                    transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
                }
                else
                {
                    direction = Facing.left;
                    transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
                }
            }
        }
        MyRigid.velocity = new Vector2(speed * (direction == Facing.left ? -1 : 1), 0f);
    }
    internal void Attacking()
    {
        if (!cooldown)
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
        else
        {
            attackTimer += Time.deltaTime;
            if (attackTimer > attackInterval)
            {
                cooldown = false;
            }
        }
    }
    private void Attack()
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
}
