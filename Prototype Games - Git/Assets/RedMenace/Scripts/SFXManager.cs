using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SFXManager : MonoBehaviour
{
    public AudioListener AL;
    public Dictionary<string, AudioClip> audioLibrary;
    public List<AudioClip> audioClips;
    public GameObject audioSourcePrefab;
    internal static SFXManager SFX = null;
    
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
        AudioSource AS = GO.transform.GetComponent<AudioSource>();
        AS.PlayOneShot(audioLibrary[name]);
        yield return new WaitForSeconds(audioLibrary[name].length);
        Destroy(GO);
    }
    public GameObject StartLoop(string name)
    {
        GameObject GO = Instantiate(audioSourcePrefab);
        AudioSource AS = GO.transform.GetComponent<AudioSource>();
        AS.PlayOneShot(audioLibrary[name]);
        AS.volume = loopVolume;
        AS.loop = true;
        return GO;
    }

}
