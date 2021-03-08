using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorLock : MonoBehaviour
{
    private Animator MyAnim;
    private bool isLocked;
    public GameObject unlockedChild;

    void Start()
    {
        MyAnim = GetComponent<Animator>();
        isLocked = MyAnim.GetBool("isLocked");
    }

    void Update()
    {
    }
    
    
    public void DoorInteract()
    {
        isLocked = MyAnim.GetBool("isLocked");
        if (!isLocked)
        {
            GameObject go = Instantiate(unlockedChild, transform.GetChild(0).transform.position, Quaternion.identity);
            transform.GetChild(0).gameObject.SetActive(false);
            go.transform.SetParent(transform);
            if (!MyAnim.GetBool("Opens"))
            {
                MyAnim.SetBool("Opens", true);
                SFXManager1.SFX.StartCoroutine("PlaySFX","door_open_SFX");
            }
            else if (MyAnim.GetBool("Opens"))
            {
                MyAnim.SetBool("Opens", false);
                SFXManager1.SFX.StartCoroutine("PlaySFX", "door_close_SFX");
            }
        }
        else
        {
            Debug.Log("Locked");
            SFXManager1.SFX.StartCoroutine("PlaySFX", "door_lock_SFX");
        }
    }
}
