using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScroller : MonoBehaviour
{
    void Update()
    {
        transform.position = new Vector2(transform.parent.position.x, transform.position.y);
    }
}
