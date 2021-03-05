using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public PlayerID ID;
    public float moveSpeed;
    public float range;
    private string IDString;
    private Animator MyAnim;
    private Rigidbody2D MyRigid;
    private Vector2 direction;
    internal float x = 0;
    internal float y = 0;

    public enum PlayerID
    {
        _1,
        _2
    }
    void Start()
    {
        MyAnim = GetComponent<Animator>();
        MyRigid = GetComponent<Rigidbody2D>();
        switch (ID)
        {
            case PlayerID._1:
                IDString = "_1";
                break;
            case PlayerID._2:
                IDString = "_2";
                break;
            default:
                break;
        }
    }

    void Update()
    {
        x = Input.GetAxis("Horizontal" + IDString);
        y = Input.GetAxis("Vertical" + IDString);
        if (Input.GetButtonDown("Interact" + IDString))
        {
            Debug.Log(IDString + " interacted");
            var Hits = Physics2D.RaycastAll(transform.position, -transform.right, range);
            foreach (var item in Hits)
            {
                if (item.transform.CompareTag("Door"))
                {
                }
            }
        }

    }
    private void FixedUpdate()
    {
        MyRigid.velocity = new Vector2(x * moveSpeed, y * moveSpeed);
        MoveAnimation();
        DirectionCheck();
    }
    private void DirectionCheck()
    {
        if (Mathf.Abs(MyRigid.velocity.x) > Mathf.Abs(MyRigid.velocity.y))
        {
            if (MyRigid.velocity.x < 0)
            {
                direction = -transform.right;
            }
            else
            {
                direction = transform.right;
            }
        }
        else if (Mathf.Abs(MyRigid.velocity.x) < Mathf.Abs(MyRigid.velocity.y))
        {
            if (MyRigid.velocity.y < 0)
            {
                direction = -transform.up;
            }
            else
            {
                direction = transform.up;
            }

        }
    }
    private void MoveAnimation()
    {
        MyAnim.SetFloat("vY", y);
        MyAnim.SetFloat("vX", x);
        if (x == 0 && y == 0)
        {
            MyAnim.SetBool("isIdle", true);
        }
        else
        {
            MyAnim.SetBool("isIdle", false);
        }
    }
}
