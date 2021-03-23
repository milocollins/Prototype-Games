using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathVFX : MonoBehaviour
{
    private void Start()
    {
        float i = Random.Range(0, 2);
        switch (i)
        {
            case 0:
                SFXManager3.theManager.PlaySFX("Win0");
                break;
            case 1:
                SFXManager3.theManager.PlaySFX("Win1");
                break;
        }
    }
    public void SelfDestruct()
    {
        Destroy(gameObject);
    }
}
