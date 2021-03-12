using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardScript : MonoBehaviour
{
    public GameObject NodeParent;
    public GameObject[] node;
    public GameObject alertIcon;
    private GameObject privObj;
    public float iconTime;
    public int currentNode;
    private Rigidbody2D MyRigid;
    private float waitTimer;
    public float waitInterval;
    private Vector2 temp;
    public float speed;
    public float chaseSpeed;
    private bool waiting;
    public float nodeRange;
    private Animator MyAnim;
    private bool wasUnlocked = false;
    private GameObject FOV;
    public float alertIconRange;
    internal GameManager1.Status localStatus;
    internal GameObject chaseTarget;

    void Start()
    {
        MyAnim = GetComponent<Animator>();
        MyRigid = GetComponent<Rigidbody2D>();
        waitTimer = waitInterval + 0.1f;
        node = new GameObject[6];
        for (int i = 0; i < NodeParent.transform.childCount; i++)
        {
            node[i] = NodeParent.transform.GetChild(i).gameObject;
        }
        transform.position = node[0].transform.position;
        currentNode = 0;
        waiting = false;
        MyAnim.SetFloat("facingY", -1f);
        MyAnim.SetFloat("facingX", 0f);
        FOV = transform.GetChild(0).gameObject;
        localStatus = GameManager1.Status.green;
    }
    void Update()
    {
        if (localStatus != GameManager1.Status.red)
        {
            #region Node Pathfinding
        if (Mathf.Abs(Vector2.Distance(transform.position, node[currentNode].transform.position)) <= nodeRange)
        {
            waiting = true;
            MyRigid.velocity = Vector2.zero;
            PassVelocity();
            MyAnim.SetBool("isIdle", true);
            transform.position = node[currentNode].transform.position;
            currentNode = NextNode(currentNode);
        }
        if (waiting)
        {
            waitTimer += Time.deltaTime;
        }
        if (waitTimer > waitInterval)
        {
            MyRigid.velocity = node[currentNode].transform.position - transform.position;
            temp = Vector3.Normalize(node[currentNode].transform.position - transform.position);
            MyRigid.velocity = new Vector2(temp.x * speed, temp.y * speed);
            MyAnim.SetBool("isIdle", false);
            MyAnim.SetFloat("facingX", MyRigid.velocity.x);
            MyAnim.SetFloat("facingY", MyRigid.velocity.y);
            if (MyRigid.velocity.x != 0)
            {

                if (MyRigid.velocity.x > 0)
                {
                    FOV.transform.rotation = Quaternion.Euler(0,0,90);
                }
                else
                {
                    FOV.transform.rotation = Quaternion.Euler(0, 0, -90);
                }
            }
            else
            {
                if (MyRigid.velocity.y > 0)
                {
                    FOV.transform.rotation = Quaternion.Euler(0, 0, 180);
                }
                else
                {
                    FOV.transform.rotation = Quaternion.Euler(0, 0, 0);
                }
            }
            PassVelocity();
            waitTimer = 0;
            waiting = false;
        }
        #endregion
        }
        else
        {
            Chase();
        }
    }
    private void PassVelocity()
    {
        MyAnim.SetFloat("vX", MyRigid.velocity.x);
        MyAnim.SetFloat("vY", MyRigid.velocity.y);
    }
    private int NextNode(int j)
    {
        int nextNode = 0;
        switch (j)
        {
            case 0:
                nextNode = 1;
                break;
            case 1:
                if (currentNode == 5)
                {
                    nextNode = 0;
                }
                else
                {
                    nextNode = 2;
                }
                break;
            case 2:
                nextNode = 3;
                break;
            case 3:
                nextNode = 4;
                break;
            case 4:
                nextNode = 5;
                break;
            case 5:
                nextNode = 1;
                break;
        }
        return nextNode;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Door"))
        {
            if (collision.gameObject.GetComponent<Animator>().GetBool("isLocked"))
            {
                wasUnlocked = false;
                collision.gameObject.GetComponent<Animator>().SetBool("isLocked", false);
            }
            else
            {
                wasUnlocked = true;
            }
            collision.gameObject.GetComponent<Animator>().SetBool("Opens", true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Door"))
        {
            if (Mathf.Abs(transform.position.x - collision.transform.position.x) > 0.4)
            {
                collision.gameObject.GetComponent<Animator>().SetBool("Opens", false);
                if (!wasUnlocked)
                {
                    collision.gameObject.GetComponent<Animator>().SetBool("isLocked", true);
                }
            }
        }
    }
    internal IEnumerator AlertIconToggle()
    {
        privObj = Instantiate(alertIcon, new Vector2(transform.position.x, transform.position.y + alertIconRange), Quaternion.identity);
        MyRigid.velocity = Vector2.zero;
        MyAnim.SetBool("isIdle", true);
        PassVelocity();
        yield return new WaitForSeconds(iconTime);
        privObj.SetActive(false);
        GameManager1.TheManager1.RedAlert();
        MyAnim.SetBool("isIdle", false);
    }
    private void Chase()
    {
        MyRigid.velocity = Vector3.Normalize(chaseTarget.transform.position - transform.position) * chaseSpeed;
        PassVelocity();
        if (Mathf.Abs(Vector2.Distance(transform.position, chaseTarget.transform.position)) < 0.2)
        {
            GameManager1.TheManager1.EndGame(GameManager1.GameState.lose);
        }
    }
    public bool RayCast(GameObject p)
    {
        bool hitPlayer = false;
        LayerMask pMask = LayerMask.GetMask("Player","Walls","Interactable");
        Debug.DrawLine(transform.position, p.transform.position, Color.red);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, p.transform.position - transform.position, 10f, pMask);
        if (hit && hit.transform.gameObject.CompareTag("Player"))
        {
            hitPlayer = true;
        }
        return hitPlayer;
    }
    public void LocalRedAlert()
    {
        localStatus = GameManager1.Status.red;
        MyRigid.velocity = Vector2.zero;
        PassVelocity();
        transform.GetChild(0).gameObject.SetActive(false);
    }
}
