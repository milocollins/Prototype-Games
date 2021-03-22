using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level2 : MonoBehaviour
{
    public enum Phase
    {
        Phase_0,
        Phase_1,
        Phase_2,
        Phase_3,
        Phase_4
    }
    [Header ("Boss Positions:")]
    public Vector2 Position_1;
    public Vector2 Position_2;
    public Vector2 Position_3;
    public Vector2 Position_4;
    public Vector2 Position_5;

    [Header("Phase Parameters:")]
    public float phaseInterval;
    public float attackInterval1;
    public float attackInterval2;
    public float attackInterval3;
    public float OrcTorque;
    public GameObject Barrier;
    public GameObject Orc;
    public int phase2Health;
    public int phase3Health;
    public float phase3Cooldown;
    public int maxProjectileCount;
    public Vector2 OrcForce;
    public float scaleIncrement;
    public float maxScale;
    public float phase4Interval;
    public float VFXInterval;
    public GameObject deathVFX;
    public GameObject level3;

    internal float cooldownInterval;
    internal static Level2 levelManager;
    internal bool cutScene = true;
    internal bool cooldown = false;
    internal Phase currentPhase;

    private bool phase3Attacking = true;
    private bool phase2Toggle;
    private bool shieldActive = false;
    private float cutSceneTimer = 5f;
    private bool cutSceneTrigger1 = false;
    private bool cutSceneTrigger2 = false;
    private bool finalcutScene = false;
    private float timer;
    internal int currentCount = 0;
    private BoxCollider2D MyCollider;
    public float cutScenePlayerSpeed;

    private void Awake()
    {
        levelManager = this;
    }

    private void Start()
    {
        timer = 0f;
        currentCount = 0;
        currentPhase = Phase.Phase_0;
        MyCollider = GetComponent<BoxCollider2D>();
        Player.thePlayer.inputLock = true;
        Player.thePlayer.x = cutScenePlayerSpeed;
        Player.thePlayer.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(5f, 0f);
        Boss.theBoss.gameObject.SetActive(false);
        level3.SetActive(false);
        cooldownInterval = attackInterval1;
        RandomPosition();
    }

    private void Update()
    {
        Debug.Log("Cooldown " + cooldown);
        if (currentPhase == Phase.Phase_0)
        {
            if (!MyCollider.isActiveAndEnabled)
            {
                timer += Time.deltaTime;
                if (timer > 1f && !cutSceneTrigger1)
                {
                    Barrier.SetActive(true);
                    SpawnBoss(Orc.transform.position);
                    cutSceneTrigger1 = true;
                }
                if (timer > 2.4f && !cutSceneTrigger2)
                {
                    Orc.GetComponent<Rigidbody2D>().AddTorque(OrcTorque);
                    Orc.GetComponent<Rigidbody2D>().AddForce(OrcForce);
                    Orc.GetComponent<BoxCollider2D>().enabled = false;
                    cutSceneTrigger2 = true;
                }
                if (timer > cutSceneTimer)
                {
                    timer = 0;
                    Player.thePlayer.inputLock = false;
                    Boss.theBoss.Despawn();
                    NextPhase();
                    cooldown = true;
                }
            }
        }
        else if (currentPhase == Phase.Phase_1)
        {
            Position_1 = new Vector2(Player.thePlayer.transform.position.x, Player.thePlayer.transform.position.y - 1.5f);
            if (!Boss.theBoss.isSpawned && !cooldown)
            {
                timer = 0f;
                SpawnBoss(Position_1);
                Boss.theBoss.Attack1();
            }
            if (Boss.theBoss.isSpawned)
            {
                if (timer > cooldownInterval)
                {
                    timer = 0f;
                    Boss.theBoss.Despawn();
                    cooldown = true;
                }
                else
                {
                    timer += Time.deltaTime;
                }
            }
            if (cooldown)
            {
                if (timer > cooldownInterval)
                {
                    cooldown = false;
                }
                else
                {
                    timer += Time.deltaTime;
                }
            }
        }
        else if (currentPhase == Phase.Phase_2)
        {
            if (!cooldown)
            {
                if (!Boss.theBoss.isSpawned)
                {
                    if (phase2Toggle)
                    {
                        SpawnBoss(Position_2);
                        phase2Toggle = false;
                    }
                    else
                    {
                        SpawnBoss(Position_3);
                        phase2Toggle = true;
                    }
                    timer = 0f;
                }
                else
                {
                    Debug.Log("Attack 2");
                    Boss.theBoss.Attack2();
                    timer = 0f;
                    cooldown = true;
                }
            }
            else
            {
                if (timer > cooldownInterval)
                {
                    cooldown = false;
                }
                else
                {
                    timer += Time.deltaTime;
                }
            }
        }
        else if (currentPhase == Phase.Phase_3)
        {
            if (!cooldown)
            {
                if (!Boss.theBoss.isSpawned)
                {
                    Debug.Log("Spawning");
                    if (shieldActive)
                    {
                        Boss.theBoss.shield.SetActive(true);
                        SpawnBoss(Position_4);
                        Debug.Log(Position_4);
                    }
                    else
                    {
                        Boss.theBoss.shield.SetActive(false);
                        SpawnBoss(Position_5);
                        Debug.Log(Position_5);
                    }
                    timer = 0f;
                }
                else
                {
                    if (shieldActive)
                    {
                        if (currentCount < maxProjectileCount)
                        {
                            Debug.Log("Attack 3");
                            Boss.theBoss.Attack3();
                            ++currentCount;
                        }
                        else
                        {
                            Boss.theBoss.Despawn();
                            shieldActive = false;
                            currentCount = 0;
                        }
                        timer = 0f;
                        cooldown = true;
                    }
                    else
                    {
                        timer += Time.deltaTime;
                        if (timer > phase3Cooldown)
                        {
                            timer = 0f;
                            Boss.theBoss.Despawn();
                            shieldActive = true;
                        }
                    }
                }
            }
            else
            {
                if (!finalcutScene)
                {
                    finalcutScene = true;
                    
                }
                if (timer > cooldownInterval)
                {
                    cooldown = false;
                }
                else
                {
                    timer += Time.deltaTime;
                }
            }
        }
        else
        {
            if (!cooldown)
            {
                if (Boss.theBoss.transform.position.y != Position_5.y)
                {
                    Boss.theBoss.Despawn();
                    timer = 0f;
                }
                if (!Boss.theBoss.isSpawned)
                {
                    SpawnBoss(Position_5);
                    cooldown = true;
                    timer = 0f;
                }
                else if (Boss.theBoss.transform.position.y == Position_5.y && Boss.theBoss.isSpawned)
                {
                    finalcutScene = true;
                    timer = 0f;
                }
                
                if (finalcutScene)
                {
                    timer += Time.deltaTime;
                    Debug.Log("Scaling: "+ Boss.theBoss.transform.localScale.y);
                    Boss.theBoss.transform.localScale = new Vector2(Boss.theBoss.transform.localScale.x, Boss.theBoss.transform.localScale.y + scaleIncrement*Time.deltaTime);
                    if (timer > VFXInterval)
                    {
                        timer = 0f;
                        Instantiate(deathVFX, transform.position, Quaternion.identity);
                    }
                    if (Boss.theBoss.transform.localScale.y > maxScale)
                    {
                        finalcutScene = false;
                        Boss.theBoss.Despawn();
                        level3.SetActive(true);
                        this.gameObject.SetActive(false);
                    }
                }
            }
            else
            {
                if (timer > cooldownInterval)
                {
                    cooldown = false;
                }
                else
                {
                    timer += Time.deltaTime;
                }
            }
        }
    }
    internal void NextPhase()
    {
        switch (currentPhase)
        {
            case Phase.Phase_0:
                currentPhase = Phase.Phase_1;
                StartCoroutine("CooldownChange", attackInterval1);
                break;
            case Phase.Phase_1:
                currentPhase = Phase.Phase_2;
                StartCoroutine("CooldownChange",attackInterval2);
                break;
            case Phase.Phase_2:
                currentPhase = Phase.Phase_3;
                shieldActive = true;
                StartCoroutine("CooldownChange", attackInterval3);
                break;
            case Phase.Phase_3:
                currentPhase = Phase.Phase_4;
                Boss.theBoss.shield.SetActive(false);
                foreach (var item in GameObject.FindGameObjectsWithTag("FlameBall"))
                {
                    item.SetActive(false);
                }
                StartCoroutine("CooldownChange", phase4Interval);
                break;
        }
    }
    private void RandomPosition()
    {
        int r = Random.Range(0, 2);
        Debug.Log(r);
        switch (r)
        {
            case 0:
                phase2Toggle = true;
                break;
            case 1:
                phase2Toggle = false;
                break;
        }
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            MyCollider.enabled = false;
            Player.thePlayer.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            Player.thePlayer.x = 0f;
        }
    }
    private void SpawnBoss(Vector2 vec)
    {
        Boss.theBoss.gameObject.SetActive(true);
        if (currentPhase != Phase.Phase_1)
        {
            Boss.theBoss.Spawn(vec);
        }
        else
        {
            Boss.theBoss.Spawn(Phase1SpawnCheck(vec));
        }
    }
    private Vector2 Phase1SpawnCheck(Vector2 vec)
    {
        Vector2 v;
        if (Player.thePlayer.transform.position.x <= -5.66)
        {
            if (Player.thePlayer.transform.position.y < 6.54)
            {
                v = new Vector2(-9.4f, 0.04793739f);
            }
            else
            {
                v = new Vector2(-9.4f, 8.047937f);
            }
        }
        else if (Player.thePlayer.transform.position.x >= 28.2)
        {
            if (Player.thePlayer.transform.position.y < 6.54)
            {
                v = new Vector2(31.27f, 0.04793739f);
            }
            else
            {
                v = new Vector2(31.27f, 8.047937f);
            }
        }
        else
        {
            if (Player.thePlayer.transform.position.y < 5.09)
            {
                v = new Vector2(Mathf.Clamp(Player.thePlayer.transform.position.x, -2, 24), -4.952063f);
            }
            else
            {
                if (Player.thePlayer.transform.position.x >= 11)
                {
                    v = new Vector2(22f, 4.047937f);
                }
                else
                {
                    v = new Vector2(0f, 4.047937f);
                }
            }
        }
        return v;
    }
    public IEnumerator CooldownChange(float f)
    {
        cooldownInterval = phaseInterval;
        yield return new WaitForSeconds(phaseInterval);
        cooldownInterval = f;
    }
}
