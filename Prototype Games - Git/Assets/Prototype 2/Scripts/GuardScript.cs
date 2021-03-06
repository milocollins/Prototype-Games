using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardScript : MonoBehaviour
{
    public GameObject NodeParent;
    public GameObject[] node;
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
    }

    void Update()
    {
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
            PassVelocity();
            waitTimer = 0;
            waiting = false;
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
            Debug.Log("TRUE");
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Door"))
        {
            collision.gameObject.GetComponent<Animator>().SetBool("Opens", false);
            if (!wasUnlocked)
            {
                collision.gameObject.GetComponent<Animator>().SetBool("isLocked", true);
            }
        }
    }
}
