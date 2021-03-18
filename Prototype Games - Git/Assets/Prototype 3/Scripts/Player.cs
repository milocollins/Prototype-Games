using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D MyRigid;
    public float speed;
    public Vector2 jumpForce;
    public float raycastDistance;
    private int jumpCount = 0;
    private float x;
    internal int facing;
    public float cameraHeight;
    public float dashDistance;
    private bool dashCooldown = false;
    public float dashCooldownTime;
    private float startingScale;
    private bool inputLock = false;
    public int meleeDamage;
    public int rangedDamage;

    private Animator MyAnim;
    public static Player thePlayer;
    public int maxMana;
    public int maxHealth;
    private int mana;
    private int health;
    public GameObject iceWall;
    public GameObject iceSpike;
    public GameObject meleeAttack;
    public UIManager3 HUD;
    public int spikeManaCost;
    public int wallManaCost;
    public int meleeManaCost;
    public float meleeRange;
    public List<AnimationClip> clips;   

    private void Start()
    {
        thePlayer = this;
        MyRigid = GetComponent<Rigidbody2D>();
        MyAnim = GetComponent<Animator>();
        mana = maxMana;
        health = maxHealth;
        HUD.manaBar.value = mana;
        HUD.healthBar.value = health;
        startingScale = transform.localScale.x;
        if (transform.localScale.x >= 0)
        {
            facing = 1;
        }
        else
        {
            facing = -1;
        }
    }
    public void Die()
    {
        gameObject.SetActive(false);
    }
    private void FixedUpdate()
    {
        if (IsGrounded())
        {
            jumpCount = 0;
        }
        if (inputLock)
        {
            MyRigid.velocity = new Vector2(0f, MyRigid.velocity.y);
        }
        else
        {
            MyRigid.velocity = new Vector2(x * speed, MyRigid.velocity.y);
        }
        HUD.manaBar.value = mana;
        HUD.healthBar.value = health;
    }


    private void Update()
    {
        if (!inputLock)
        {
            Debug.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.y - raycastDistance, transform.position.z), Color.red);
            x = Input.GetAxisRaw("Horizontal");
            if (Input.GetButtonDown("Jump") && jumpCount < 1)
            {
                if (jumpCount == 1)
                {
                    MyRigid.AddForce(new Vector2(jumpForce.x, jumpForce.y/2));
                }
                else
                {
                    MyRigid.AddForce(jumpForce);
                }
                ++jumpCount;
            }
            if (Mathf.Abs(x) > 0.01 && !dashCooldown && Input.GetKeyDown(KeyCode.LeftShift))
            {
                Debug.Log("Dash");
                Dash();
            }
            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                if (mana - spikeManaCost >= 0)
                {
                    Instantiate(iceSpike);
                    mana -= spikeManaCost;
                    MyAnim.SetTrigger("Attack2");
                    StartCoroutine("InputLockToggle", clips[0].length);
                }
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (mana - wallManaCost >= 0 && IsGrounded())
                {
                    Instantiate(iceWall);
                    mana -= wallManaCost;
                    MyAnim.SetTrigger("Attack1");
                    StartCoroutine("InputLockToggle", clips[1].length);
                }
            }
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                if (mana - meleeManaCost >= 0)
                {
                    Instantiate(meleeAttack);
                    mana -= meleeManaCost;
                    MyAnim.SetTrigger("Attack2");
                    StartCoroutine("InputLockToggle", clips[0].length);
                }
            }
            if (x > 0.01)
            {
                facing = 1;
                transform.localScale = new Vector3(startingScale, transform.localScale.y, transform.localScale.z);
            }
            else if (x < -0.01)
            {
                facing = -1;
                transform.localScale = new Vector3(-startingScale, transform.localScale.y, transform.localScale.z);
            }
        }
        PassAnim();
    }
    private bool IsGrounded()
    {
        bool tempBool = false;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, raycastDistance);
        if (hit)
        {
            tempBool = true;
        }
        return tempBool;
    }
    private void Dash()
    {
        MyRigid.AddForce(new Vector2(facing*dashDistance, 0));
        StartCoroutine("DashCooldown");
    }
    private IEnumerator DashCooldown()
    {
        dashCooldown = true;
        yield return new WaitForSeconds(dashCooldownTime);
        dashCooldown = false;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Death"))
        {
            transform.position = Vector2.zero;
        }
    }
    public void TakeDamage(int d)
    {
        health -= d;
        if (health <= 0)  
        {
            MyAnim.SetTrigger("isDead");
        }
        else
        {
            MyAnim.SetTrigger("isHit");
        }
    }
    private void PassAnim()
    {
        if (Mathf.Abs(x) > 0.01)
        {
            MyAnim.SetBool("isIdle", false);
        }
        else
        {
            MyAnim.SetBool("isIdle", true);
        }
        MyAnim.SetBool("isGrounded", IsGrounded());
        MyAnim.SetFloat("Vertical", MyRigid.velocity.y);
    }
    private IEnumerator InputLockToggle(float f)
    {
        inputLock = true;
        yield return new WaitForSeconds(f);
        inputLock = false;
    }
}
