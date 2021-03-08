using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Double_Door : MonoBehaviour
{
    private Animator MyAnim;
    private void Start()
    {
        MyAnim = GetComponent<Animator>();
    }
    public void InteractToggle()
    {
        bool OD = MyAnim.GetBool("isOpening");
        OD = !OD;
        if (OD)
        {
            SFXManager1.SFX.StartCoroutine("PlaySFX", "door_open_SFX");
        }
        else
        {
            SFXManager1.SFX.StartCoroutine("PlaySFX", "door_close_SFX");
        }
        MyAnim.SetBool("isOpening", OD);
    }
}
