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
    internal float x;
    internal int facing;
    public float cameraHeight;
    public float dashDistance;
    private bool dashCooldown = false;
    public float dashCooldownTime;
    private float startingScale;
    internal bool inputLock = false;
    public int meleeDamage;
    public int rangedDamage;

    internal Animator MyAnim;
    public static Player thePlayer;
    public int maxMana;
    public int maxHealth;
    internal int mana;
    internal int health;
    public bool level2;
    public GameObject iceWall;
    public GameObject iceSpike;
    public GameObject meleeAttack;
    public UIManager3 HUD;
    public int spikeManaCost;
    public int wallManaCost;
    public int meleeManaCost;
    public float meleeRange;
    public List<AnimationClip> clips;
    public int manaRegen;
    public float regenTimer;
    public float currentRegenTime;

    private void Awake()
    {
        thePlayer = this;
        MyRigid = GetComponent<Rigidbody2D>();
        MyAnim = GetComponent<Animator>();
    }
    private void Start()
    {
        health = maxHealth;
        mana = maxMana;
        UIManager3.theManager.UpdatePlayerStats();
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
    private void FixedUpdate()
    {
        if (IsGrounded())
        {
            jumpCount = 0;
        }
        if (inputLock)
        {
            if (level2 && Level2.levelManager.cutScene)
            {
                MyRigid.velocity = new Vector2(x*speed, MyRigid.velocity.y);
            }
            else
            {
                MyRigid.velocity = new Vector2(0f, MyRigid.velocity.y);
            }
        }
        else
        {
            MyRigid.velocity = new Vector2(x * speed, MyRigid.velocity.y);
        }
    }


    private void Update()
    {
        if (mana != maxMana)
        {
            if (currentRegenTime > regenTimer)
            {
                currentRegenTime = 0f;
                Mathf.Clamp(mana += manaRegen, 0, 10);
                UIManager3.theManager.UpdatePlayerStats();
            }
            else
            {
                currentRegenTime += Time.deltaTime;
            }
        }
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
            SFXManager3.theManager.PlaySFX("Player_Die");
        }
        else
        {
            MyAnim.SetTrigger("isHit");
            SFXManager3.theManager.PlaySFX("Player_Hit");
        }
        UIManager3.theManager.UpdatePlayerStats();
    }
    internal void PassAnim()
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
    public void PlayStep()
    {
        SFXManager3.theManager.PlaySFX("Footstep1");
    }
    public void DeathScreen()
    {
        inputLock = true;
        Camera.main.transform.parent = null;
        StartCoroutine("FadeToWhite");
        foreach (var item in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            if (item.GetComponent<MeleeUnderling>())
            {
                item.GetComponent<MeleeUnderling>().enabled = false;
            }
            else if (item.GetComponent<RangedUnderling>())
            {
                item.GetComponent<RangedUnderling>().enabled = false;
            }
            else
            {
                item.GetComponent<Boss>().enabled = false;
            }
        }
    }
    public IEnumerator FadeToWhite()
    {
        UIManager3.theManager.Die();
        yield return new WaitForSeconds(4f);
        StartCoroutine("ReloadScene");
    }
    public IEnumerator ReloadScene()
    {
        UIManager3.theManager.Die1();
        yield return new WaitForSeconds(3f);
        GameManager3.theManager.ReloadScene();
    }
}
