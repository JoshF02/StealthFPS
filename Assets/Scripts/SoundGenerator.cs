using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SoundGenerator : MonoBehaviour
{
    public static SoundGenerator Instance { get; private set; }
    [SerializeField] private GameObject _soundGO;

    private void Awake()
    {
        if ((Instance != null) && (Instance != this)) Destroy(this); // can only be 1 instance
        else Instance = this; 
    }

    private IEnumerator GenerateSoundCoroutine(Transform source, float volume, float duration)
    {
        GameObject generatedSound = Instantiate(_soundGO, source.position, Quaternion.identity);
        generatedSound.name = source.gameObject.name + " Sound";
        generatedSound.transform.localScale *= volume;
        generatedSound.transform.SetParent(source);

        yield return new WaitForSeconds(duration);
        Destroy(generatedSound);
    }

    public void GenerateSound(Transform source, float volume, float duration)
    {
        StartCoroutine(GenerateSoundCoroutine(source, volume, duration));
    }
}
