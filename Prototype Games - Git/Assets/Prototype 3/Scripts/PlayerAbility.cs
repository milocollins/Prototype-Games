using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbility : MonoBehaviour
{
    public enum Type
    {
        melee,
        ranged,
        shield
    }
    public Type myType;
    public float wallDistance;
    internal int wallHealth;
    public int maxWallHealth;
    public float spikeDistance;
    public float meleeRange;
    private int direction;

    private Rigidbody2D myRigid;
    public Animator MyAnim;
    public int spikeSpeed;
    public float wallHeight;

    private void Start()
    {
        UIManager3.theManager.UpdatePlayerStats();
        direction = Player.thePlayer.facing;
        transform.localScale = new Vector3(direction * transform.localScale.x, transform.localScale.y, transform.localScale.z);
        myRigid = GetComponent<Rigidbody2D>();
        switch (myType)
        {
            case Type.melee:
                SFXManager3.theManager.PlaySFX("Ice_Melee");
                RaycastHit2D hit = Physics2D.Raycast(Player.thePlayer.transform.position, transform.right * direction, meleeRange);
                if (hit)
                {
                    if (hit.transform.gameObject.GetComponent<Door>())
                    {
                        hit.transform.gameObject.GetComponent<Door>().TakeDamage();
                    }
                    else if (hit.transform.CompareTag("Enemy"))
                    {
                        if (hit.transform.gameObject.name.Contains("Ranged"))
                        {
                            hit.transform.GetComponent<RangedUnderling>().TakeDamage(Player.thePlayer.meleeDamage);
                        }
                        else if (hit.transform.gameObject.name.Contains("Melee"))
                        {
                            hit.transform.GetComponent<MeleeUnderling>().TakeDamage(Player.thePlayer.meleeDamage);
                        }
                        else
                        {
                            hit.transform.GetComponent<Boss>().TakeDamage(Player.thePlayer.meleeDamage);
                        }
                    }
                    else if (hit.transform.gameObject.name.Contains("Ice Wall"))
                    {
                        hit.transform.gameObject.GetComponent<PlayerAbility>().TakeDamage();
                    }
                }
                gameObject.SetActive(false);
                break;
            case Type.ranged:
                SFXManager3.theManager.PlaySFX("Ice_Ranged");
                transform.position = new Vector2(Player.thePlayer.transform.position.x + direction * spikeDistance, Player.thePlayer.transform.position.y);
                myRigid.velocity = new Vector2(spikeSpeed * direction, 0);
                MyAnim.Play("Spike_1");
                break;
            case Type.shield:
                SFXManager3.theManager.PlaySFX("Ice_Wall");
                wallHealth = maxWallHealth;
                transform.position = new Vector2(Player.thePlayer.transform.position.x + direction * wallDistance, Player.thePlayer.transform.position.y + wallHeight);
                StartCoroutine("WallSpawnFX");
                break;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(collision.gameObject.name);
        //0.7f
        int d = 0;
        if (myType == Type.ranged)
        {
            MyAnim.SetTrigger("Hit");
            d = Player.thePlayer.rangedDamage;
        }
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (collision.gameObject.GetComponent<MeleeUnderling>())
            {
                collision.gameObject.GetComponent<MeleeUnderling>().TakeDamage(d);
            }
            else if (collision.gameObject.GetComponent<RangedUnderling>())
            {
                collision.gameObject.GetComponent<RangedUnderling>().TakeDamage(d);
            }
            else if (collision.gameObject.GetComponent<Boss>())
            {
                if (!Boss.theBoss.shieldActive)
                {
                    collision.gameObject.GetComponent<Boss>().TakeDamage(d);
                }
            }
        }
        else if (collision.gameObject.GetComponent<Door>())
        {
            collision.gameObject.GetComponent<Door>().TakeDamage();
        }
        else if (collision.gameObject.name.Contains("Ice Wall"))
        {
            collision.gameObject.GetComponent<PlayerAbility>().TakeDamage();
        }
    }
    public void StopAnim()
    {
        MyAnim.gameObject.SetActive(false);
    }
    public IEnumerator WallSpawnFX()
    {
        transform.GetChild(0).gameObject.SetActive(true);
        yield return new WaitForSeconds(0.9f);
        transform.GetChild(0).gameObject.SetActive(false);
    }
    public void TakeDamage()
    {
        if (myType == Type.shield)
        {
            --wallHealth;
            if (wallHealth<= 0)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
