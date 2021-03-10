using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CityHealth : MonoBehaviour
{
    public GameObject parent;
    public Sprite full;
    public Sprite medium;
    public Sprite critical;
    public Sprite spriteFull;
    public Sprite spriteMedium;
    public Sprite spriteCritical;
    private RectTransform rt;

    private bool fixedState = true;
    private State healthState;
        
    public enum State
    {
        Full,
        Medium,
        Critical,
        Dead
    }
    void Awake()
    {
        rt = gameObject.GetComponent<RectTransform>();
        rt.position = new Vector2(parent.transform.position.x, parent.transform.position.y + 1);
    }
    void Start()
    {
        healthState = State.Full;

    }
    void Update()
    {
        if (!fixedState)
        {
            switch (healthState)
            {
                case State.Full:
                    gameObject.GetComponent<Image>().sprite = full;
                    break;
                case State.Medium:
                    gameObject.GetComponent<Image>().sprite = medium;
                    break;
                case State.Critical:
                    gameObject.GetComponent<Image>().sprite = critical;
                    break;
                case State.Dead:
                    Destroy(gameObject);
                    break;
                default:
                    break;
            }
            fixedState = true;
        }
    }
    public void ChangeState(int i)
    {
        fixedState = false;
        switch (i)
        {
            case 1:
                healthState = State.Critical;
                parent.GetComponent<SpriteRenderer>().sprite = spriteCritical;
                break;
            case 2:
                healthState = State.Medium;
                parent.GetComponent<SpriteRenderer>().sprite = spriteMedium;
                break;
            case 3:
                healthState = State.Full;
                parent.GetComponent<SpriteRenderer>().sprite = spriteFull;
                break;
            default:
                healthState = State.Dead;
                break;
        }
    }
}
