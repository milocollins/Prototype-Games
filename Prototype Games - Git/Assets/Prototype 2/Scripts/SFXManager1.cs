using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager1 : MonoBehaviour
{
    public AudioListener AL;
    public Dictionary<string, AudioClip> audioLibrary;
    public List<AudioClip> audioClips;
    public GameObject audioSourcePrefab;
    public static SFXManager1 SFX;

    public float loopVolume;


    private void Awake()
    {
        SFX = this;
        audioLibrary = new Dictionary<string, AudioClip>();

        foreach (AudioClip item in audioClips)
        {
            audioLibrary.Add(item.name, item);
        }

    }

    public IEnumerator PlaySFX(string name)
    {
        GameObject GO = Instantiate(audioSourcePrefab);
        GO.transform.SetParent(GameManager1.TheManager1.MainCamera.transform);
        GO.transform.position = Vector2.zero;
        AudioSource AS = GO.transform.GetComponent<AudioSource>();
        AS.PlayOneShot(audioLibrary[name]);
        if (name == "chase")
        {
            AS.volume = 0.1f;
        }
        yield return new WaitForSeconds(audioLibrary[name].length);
        Debug.Log("w");
        Destroy(GO);
    }
    public GameObject StartLoop(string name)
    {
        GameObject GO = Instantiate(audioSourcePrefab);
        GO.transform.SetParent(GameObject.FindGameObjectWithTag("MainCamera").transform);
        GO.transform.localPosition = Vector2.zero;
        AudioSource AS = GO.transform.GetComponent<AudioSource>();
        AS.clip = audioLibrary[name];
        AS.volume = loopVolume;
        AS.loop = true;
        AS.Play();
        Debug.Log("z");
        return GO;
    }
}
