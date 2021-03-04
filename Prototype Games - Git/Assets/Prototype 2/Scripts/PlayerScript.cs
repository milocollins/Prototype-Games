using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public PlayerID ID;
    public float moveSpeed;
    private string IDString;
    private Animator MyAnim;
    private Rigidbody2D MyRigid;
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
        /*if (x > y)
        {
            y = 0;
        }
        else if (y > x)
        {
            x = 0;
        }*/
        MyRigid.velocity = new Vector2(x * moveSpeed, y * moveSpeed);
        MoveAnimation();
    }
    private void MoveAnimation()
    {
        MyAnim.SetFloat(Animator.StringToHash("vY"), y);
        MyAnim.SetFloat(Animator.StringToHash("vX"), x);
        if (x == 0 && y == 0)
        {
            MyAnim.SetBool(Animator.StringToHash("isIdle"), true);
        }
        else
        {
            MyAnim.SetBool(Animator.StringToHash("isIdle"), false);
        }
    }
}
