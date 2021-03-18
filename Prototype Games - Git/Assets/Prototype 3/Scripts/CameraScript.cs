using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public GameObject Player;
    void Update()
    {
        transform.position = new Vector2(Player.transform.position.x, Player.transform.position.y); 
    }
}
