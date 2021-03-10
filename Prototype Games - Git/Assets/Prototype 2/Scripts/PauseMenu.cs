using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    private void Start()
    {
        Time.timeScale = 0f;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Joystick1Button2) || Input.GetKeyDown(KeyCode.Joystick2Button2))
        {
            Time.timeScale = 1f;
            gameObject.SetActive(false);
        }
        if (Input.GetKeyDown(KeyCode.Joystick1Button3) || Input.GetKeyDown(KeyCode.Joystick2Button3))
        {
            Time.timeScale = 1f;
            SceneNav.NavTo("Prototype_2_Start");
        }
    }
}
