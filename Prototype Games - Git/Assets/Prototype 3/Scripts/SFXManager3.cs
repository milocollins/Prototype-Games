using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SFXManager3 : MonoBehaviour
{
    public Dictionary<string, AudioClip> audioLibrary = new Dictionary<string, AudioClip>();
    public List<AudioClip> audioClips = new List<AudioClip>();
    public static SFXManager3 theManager;
    public GameObject audioPrefab;

    void Start()
    {
        theManager = this;
        foreach (var item in audioClips)
        {
            audioLibrary.Add(item.name, item);
        }
    }
    public void PlaySFX(string s)
    {
        if (audioLibrary.ContainsKey(s))
        {
            AudioSource sfxClip = Instantiate(audioPrefab).GetComponent<AudioSource>();
            sfxClip.PlayOneShot(audioLibrary[s]);
            Destroy(sfxClip.gameObject, audioLibrary[s].length);
        }
    }

}
