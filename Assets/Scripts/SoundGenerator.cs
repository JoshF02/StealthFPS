using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SoundGenerator : MonoBehaviour
{
    public static SoundGenerator Instance { get; private set; }

    [SerializeField] private GameObject soundGO;

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this); // can only be 1 instance
        else Instance = this; 
    }

    IEnumerator GenerateSoundCoroutine(Transform source, float volume, float duration)
    {
        GameObject generatedSound = Instantiate(soundGO, source.position, Quaternion.identity);
        generatedSound.name = source.gameObject.name + " Sound";
        generatedSound.transform.localScale *= volume;
        generatedSound.transform.SetParent(source);

        //Debug.Log("GENERATED " + generatedSound);
        yield return new WaitForSeconds(duration);

        //Debug.Log("destroying sound object");
        Destroy(generatedSound);
    }

    public void GenerateSound(Transform source, float volume, float duration)
    {
        StartCoroutine(GenerateSoundCoroutine(source, volume, duration));
    }
}
