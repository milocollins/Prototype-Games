using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallShatter : MonoBehaviour
{
    public void SelfDestruct()
    {
        Destroy(gameObject);
    }
}
