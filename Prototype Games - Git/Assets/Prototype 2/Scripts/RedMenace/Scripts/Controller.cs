using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    public Rigidbody2D RB;
    public GameObject Missile;
    public GameObject MissilePlus;
    public Vector3 launchPad;
    private GameObject Obj;
    private List<GameObject> Missiles = new List<GameObject>();
    private bool isPaused = false;

    public float aimSpeed;
    public int maxMissileCount;
    internal bool inputLock = false;

    void Start()
    {
    }

    void Update()
    {
        if (!inputLock)
        {
            RB.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * aimSpeed, Input.GetAxisRaw("Vertical")*aimSpeed);
            if (Input.GetKeyDown(KeyCode.Space) && GameManager.TheManager.missileCount < GameManager.TheManager.maxMissilesActive)
            {
                ++GameManager.TheManager.missileCount;
                GameManager.TheManager.SiloRemove();
                if (!GameManager.TheManager.powerupM)
                {
                    Obj = Instantiate(Missile, launchPad, Quaternion.identity);
                }
                else
                {
                    Obj = Instantiate(MissilePlus, launchPad, Quaternion.identity);
                }
            }
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (!isPaused)
                {
                    Time.timeScale = 0f;
                    isPaused = true;
                }
                else
                {
                    Time.timeScale = 1f;
                    isPaused = false;
                }
                UIManager.UI.PauseToggle();
            }
        }
    }

    void LateUpdate()
    {
        if(transform.position.y < -4)
        {
            transform.position = new Vector3 (transform.position.x, -4);
        }
        if (transform.position.y > 5)
        {
            transform.position = new Vector3(transform.position.x, 5);
        }
        if (transform.position.x < -9)
        {
            transform.position = new Vector3(-9, transform.position.y);
        }
        if (transform.position.x > 9)
        {
            transform.position = new Vector3(9, transform.position.x);
        }
    }
}
