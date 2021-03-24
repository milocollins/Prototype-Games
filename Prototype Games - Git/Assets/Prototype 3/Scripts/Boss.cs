using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [Header("Prefabs:")]
    public GameObject Tendril;
    public GameObject Phase2Projectile;
    public GameObject Phase3Projectile;

    [Header("Parameters:")]
    public int contactDamage;
    public float tendrilDistance;
    public float tendrilTimer;
    public int maxHealth;

    internal static Boss theBoss;
    internal bool isIdle;
    internal bool isSpawned;
    internal bool isHit;
    //
    public int currentHealth;
    internal Vector2 Target;

    private float damageTimer = 0f;
    private float damageInterval = 4f;
    private bool damageToggle = false;
    private Animator MyAnim;
    internal GameObject shield;
    private GameObject tendril1;
    private GameObject tendril2;
    internal PolygonCollider2D Body;
    internal bool shieldActive = false;

    private void Awake()
    {
        theBoss = this;
    }
    private void Start()
    {
        Body = GetComponent<PolygonCollider2D>();
        currentHealth = maxHealth;
        Body = gameObject.GetComponent<PolygonCollider2D>();
        MyAnim = gameObject.GetComponent<Animator>();
        shield = transform.GetChild(0).gameObject;
        shield.SetActive(false);
    }
    public void Spawn(Vector2 vec)
    {
        transform.position = vec;
        isSpawned = true;
        SFXManager3.theManager.PlaySFX("Boss_Spawn");
    }
    public void Despawn()
    {
        StartCoroutine("DespawnAnim");
    }
    public IEnumerator DespawnAnim()
    {
        yield return new WaitForSeconds(0.3f);
        Level2.levelManager.cooldown = true;
        MyAnim.SetBool("isDespawning", true);
        SFXManager3.theManager.PlaySFX("Boss_Despawn");
    }
    public void Idle()
    {

    }
    public void Attack1()
    {
        tendril1 = Instantiate(Tendril);
        tendril1.transform.position = new Vector2(transform.position.x + tendrilDistance, transform.position.y);
        tendril2 = Instantiate(Tendril);
        tendril2.transform.position = new Vector2(transform.position.x - tendrilDistance, transform.position.y);
        StartCoroutine("TendrilDespawn");
    }
    public void Attack2()
    {
        Instantiate(Phase2Projectile);
        SFXManager3.theManager.PlaySFX("Attack2");
    }
    public void Attack3()
    {
        Instantiate(Phase3Projectile);
        SFXManager3.theManager.PlaySFX("Attack3");
    }

    public void ToggleActive()
    {
        MyAnim.SetBool("isDespawning", false);
        isSpawned = false;
        gameObject.SetActive(false);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Player.thePlayer.TakeDamage(contactDamage);
        }
    }
    public IEnumerator TendrilDespawn()
    {
        yield return new WaitForSeconds(tendrilTimer);
        tendril1.GetComponent<Animator>().SetBool("isDespawning", true);
        tendril2.GetComponent<Animator>().SetBool("isDespawning", true);
    }
    public void TakeDamage(int d)
    {
        currentHealth -= d;
        BossHealth.bossHealth.UpdateHealth();
        SFXManager3.theManager.PlaySFX("Boss_Hit");
        MyAnim.SetTrigger("isHit");
        if (Level2.levelManager.currentPhase == Level2.Phase.Phase_1 && currentHealth <= Level2.levelManager.phase2Health)
        {
            Despawn();
            Level2.levelManager.cooldown = true;
            Level2.levelManager.NextPhase();
        }
        else if (Level2.levelManager.currentPhase == Level2.Phase.Phase_2)
        {
            if (currentHealth <= Level2.levelManager.phase3Health)
            {
                Despawn();
                Level2.levelManager.cooldown = true;
                Level2.levelManager.NextPhase();
            }
            else
            {
                Despawn();
                Level2.levelManager.cooldown = true;
            }
        }
        if (currentHealth <= 0 && Level2.levelManager.currentPhase == Level2.Phase.Phase_3)
        {
            Level2.levelManager.cooldown = true;
            Level2.levelManager.NextPhase();
        }
    }
    public void SpawnSFX()
    {
        SFXManager3.theManager.PlaySFX("Attack1");
    }
}
