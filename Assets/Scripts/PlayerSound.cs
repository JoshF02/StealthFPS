using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerSound : MonoBehaviour
{
    public static PlayerSound Instance { get; private set; }

    private GameObject _soundCollider;
    private Dictionary<string, int> _sounds = new();

    private void Awake()
    {
        if ((Instance != null) && (Instance != this)) Destroy(this); // can only be 1 instance
        else Instance = this; 

        _soundCollider = transform.GetChild(1).gameObject;
        _soundCollider.SetActive(false);
        _soundCollider.transform.localScale = new(0.1f, 0.1f, 0.1f);
    }

    public void StartSound(string name, int radius)
    {  
        _sounds[name] = radius;
        _soundCollider.SetActive(true);
        int max = _sounds.Values.Max();  // uses largest value as collider radius
        _soundCollider.transform.localScale = new(max, max, max);
    }

    public void StopSound(string name)
    {
        if (!_sounds.ContainsKey(name)) return;  // sound being stopped is not active

        _sounds.Remove(name);

        if (_sounds.Keys.Count == 0)    // disables collider if no sounds active
        {       
            _soundCollider.SetActive(false);
            _soundCollider.transform.localScale = new(0.1f, 0.1f, 0.1f);
        }
        else    // uses largest value as collider radius
        {  
            int max = _sounds.Values.Max();
            _soundCollider.transform.localScale = new(max, max, max);
        } 
    }

    private IEnumerator PlaySoundForDurationCoroutine(string name, int radius, float duration)
    {
        StartSound(name, radius);
        yield return new WaitForSeconds(duration);
        StopSound(name);
    }

    public void PlaySoundForDuration(string name, int radius, float duration)
    {
        StartCoroutine(PlaySoundForDurationCoroutine(name, radius, duration));
    }
}
