using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager TheManager = null;
    public SpriteRenderer[] SiloMissiles;
    public GameObject Enemy;
    public GameObject Explosion;
    public GameObject Plane;
    public GameObject Silo;
    public GameObject SiloPlus;
    public SpriteRenderer[] upgradedSilo;
    public Controller controller;
    private Explosion ex;
    private GameObject EnemyRef;
    private Coroutine se;
    private GameObject loop;

    public int upgradeFreshold;
    public int maxMissilesActive = 3;
    public int enemyScore;
    public int nukeMult;
    internal int score = 0;
    internal int missileCount = 0;
    internal int CityCount = 6;
    private bool GameOver = false;
    private int totalHealth =0;
    private int missileI;
    private bool upgradeToggled;

    public int MaxEnemyCount;
    public float Interval;
    public float minInterval;
    public float SpawnRate;
    private float CurrentTimer = 0;
    private int EnemyCount = 0;
    private int randNuke;

    public int maxPowerupCount;
    public float powerupInterval;
    internal int powerupCount = 0;
    private float powerupTimer = 0;
    public float powerupMInterval;
    internal bool powerupM = false;
    private float powerupMTimer =0;
    public bool shieldToggle;
    public float shieldTimerInterval;
    private float shieldTimer = 0;

    private void Awake()
    {
        //maxMissilesActive = controller.GetComponent<Controller>().maxMissileCount;
        missileI = maxMissilesActive - 1;
        ex = Explosion.GetComponent<Explosion>();
    }
    //ADDING DYNAMIC MISSILE SILO
    void Start()
    {
        TheManager = this;
        loop = SFXManager.SFX.StartLoop("Loop");
        randNuke = Random.Range(MaxEnemyCount/2, MaxEnemyCount);
        Debug.Log(randNuke);
    }

    [System.Obsolete]
    void Update()
    {
        if (!GameOver)
        {
            CurrentTimer += Time.deltaTime;
            powerupTimer += Time.deltaTime;
            //Enemy Spawning
            if (CurrentTimer > Interval && EnemyCount < MaxEnemyCount)
            {
                SpawnEnemy();
                CurrentTimer = 0;
                ++EnemyCount;
                if (Interval > minInterval)
                {
                    Interval = Interval * (1 - SpawnRate);
                }
            }
            //Powerup Spawning
            if (powerupTimer > powerupInterval)
            {
                if (powerupCount != maxPowerupCount)
                {
                    ++powerupCount;
                    SpawnPlane();
                    powerupTimer = 0;
                }
            }
            //Game Win Condition
            if (MaxEnemyCount == EnemyCount && !GameOver)
            {
                if (GameObject.FindGameObjectsWithTag("Enemy").Length == 0)
                {
                    ++EnemyCount;
                    GameWin();
                }
            }
            //Missile Powerup Timer
            if (powerupM == true)
            {
                powerupMTimer += Time.deltaTime;
                if (powerupMTimer > powerupMInterval)
                {
                    powerupM = false;
                    powerupMTimer = 0;
                }
            }
            //Shiled Powerup Timer
            if (shieldToggle == true)
            {
                shieldTimer += Time.deltaTime;
                if (shieldTimer > shieldTimerInterval)
                {
                    shieldToggle = false;
                    shieldTimer = 0;
                    GameObject[] cityArray = GameObject.FindGameObjectsWithTag("City");
                    foreach (GameObject obj in cityArray)
                    {
                        obj.GetComponent<City>().shield = false;
                    }
                }
            }
        }
    }

    private void LateUpdate()
    {
        if (score >= upgradeFreshold && !upgradeToggled)
        {
            upgradeToggled = true;
            Upgrade();
        }
    }
    public void EnemyKill(bool b)
    {
        if (!b)
        {
            score += enemyScore;
        }
        else
        {
            score += nukeMult * enemyScore;
        }
    }
    public void GameWin()
    {
        Destroy(loop);
        GameObject[] powerups = GameObject.FindGameObjectsWithTag("Powerup");
        foreach (GameObject item in powerups)
        {
            Destroy(item);
        }


        SFXManager.SFX.StartCoroutine("PlaySFX", "Level_Win");
        GameOver = true;
        UIManager.UI.GameWon();
        controller.inputLock = true;
        controller.GetComponent<SpriteRenderer>().enabled = false;
        
        GameObject[] cityArray = GameObject.FindGameObjectsWithTag("City");


        foreach (GameObject obj in cityArray)
        {
            totalHealth += obj.GetComponent<City>().Health;
        }
        score += totalHealth * 1000;

    }
    public void GameLoss()
    {
        --CityCount;
        if(CityCount == 0)
        {
            GameObject[] powerups = GameObject.FindGameObjectsWithTag("Powerup");
            foreach (GameObject item in powerups)
            {
                Destroy(item);
            }
            Destroy(loop);
            GameOver = true;
            SFXManager.SFX.StartCoroutine("PlaySFX", "Level_Lose");
            UIManager.UI.GameOver();
            controller.inputLock = true;
            controller.GetComponent<SpriteRenderer>().enabled = false;
        }
    }
    private void SpawnEnemy()
    {
        if(EnemyCount != randNuke)
        {
            float xPos = Random.Range(-9, 9);
            Instantiate(Enemy, new Vector3 (xPos, 5, 0), Quaternion.identity);
        }
        else
        {
            GameObject gon = Instantiate(Plane, new Vector2(RandomBinary() * 9, 4), Quaternion.identity);
            gon.GetComponent<Plane>().enemy = true;
        }
    }
    private void SpawnPlane()
    {
        float yPos = Random.Range(2, 4);
        int xPos = RandomBinary();
        Instantiate(Plane, new Vector3(xPos*9, yPos, 0), Quaternion.identity);
    }

    private int RandomBinary()
    {
        int r = Random.Range(-1, 1);
        if (r == 0)
        {
            r = 1;
        }
        return r;
    }

    public void SiloRemove()
    {
        SiloMissiles[missileI].enabled = false;
        --missileI;
    }
    public void SiloAdd()
    {
        SiloMissiles[missileI+1].enabled = true;
        ++missileI;
    }
    public IEnumerator ExplosionTimer()
    {
        yield return new WaitForSeconds(ex.defaultTicks*ex.Interval);
        --missileCount;
        SiloAdd();
    }
    public void Upgrade()
    {
        Silo.SetActive(false);
        SiloMissiles = new SpriteRenderer[5];
        SiloPlus.SetActive(true);
        maxMissilesActive = 5;
        missileI = (GameObject.FindGameObjectsWithTag("SiloMissile").Length - missileI) - 1;
        for (int i = 0; i < upgradedSilo.Length; i++)
        {
            SiloMissiles[i] = upgradedSilo[i];
        }
    }
}
