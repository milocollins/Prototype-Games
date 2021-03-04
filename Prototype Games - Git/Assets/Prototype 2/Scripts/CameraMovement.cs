using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform P_1;
    public Transform P_2;
    public Camera camera_1;
    public Camera camera_2;

    private Camera mainCamera;   
    private Vector3 average;
    private bool P1InView = true;
    private bool P2InView = true;
    private BoxCollider2D boundary;
    void Start()
    {
        boundary = GetComponent<BoxCollider2D>();
        mainCamera = GetComponent<Camera>();
        camera_1.enabled = false;
        camera_2.enabled = false;
    }

    void Update()
    {
        
        transform.position = Midpoint();
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.name == "Player_1")
        {
            P1InView = false;
        }
        else if (other.name == "Player_2")
        {
            P2InView = false;
        }
        camera_1.enabled = true;
        camera_2.enabled = true;
        mainCamera.enabled = false;
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Player_1")
        {
            P1InView = true;
        }
        else if (collision.name == "Player_2")
        {
            P2InView = true;
        }
        if (P1InView && P2InView)
        {
            camera_1.enabled = false;
            camera_2.enabled = false;
            mainCamera.enabled = true;
        }
    }
    private Vector3 Midpoint()
    {
        average = new Vector3((P_1.position.x + P_2.position.x) / 2, (P_1.position.y + P_2.position.y) / 2, transform.position.z);
        return average;
    }
}
