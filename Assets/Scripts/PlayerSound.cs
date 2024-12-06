using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerSound : MonoBehaviour
{
    public static PlayerSound Instance { get; private set; }

    private GameObject soundCollider;

    private Dictionary<string, int> sounds = new();

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this); // can only be 1 instance
        else Instance = this; 

        soundCollider = transform.GetChild(1).gameObject;
        soundCollider.SetActive(false);
        soundCollider.transform.localScale = new(0.1f, 0.1f, 0.1f);
    }

    public void StartSound(string name, int radius)
    {  
        //if (sounds.ContainsKey(name) && sounds[name] == radius) return;   // optimisation but probably worsens performance for 1-2 sounds
        //Debug.Log("values being changed");

        sounds[name] = radius;
        soundCollider.SetActive(true);
        int max = sounds.Values.Max();  // uses largest value as collider radius
        soundCollider.transform.localScale = new(max, max, max);
    }

    public void StopSound(string name)
    {
        if (!sounds.ContainsKey(name)) return;  // sound being stopped is not active

        sounds.Remove(name);
        //Debug.Log("sound removed");

        if (sounds.Keys.Count == 0) {       // disables collider if no sounds active
            soundCollider.SetActive(false);
            soundCollider.transform.localScale = new(0.1f, 0.1f, 0.1f);
        }
        else {  // uses largest value as collider radius
            int max = sounds.Values.Max();
            soundCollider.transform.localScale = new(max, max, max);
        } 
    }

    IEnumerator PlaySoundForDurationCoroutine(string name, int radius, float duration)
    {
        StartSound(name, radius);
        Debug.Log("starting sound");
        yield return new WaitForSeconds(duration);
        Debug.Log("stopping sound");
        StopSound(name);
    }

    public void PlaySoundForDuration(string name, int radius, float duration)
    {
        StartCoroutine(PlaySoundForDurationCoroutine(name, radius, duration));
    }
}
