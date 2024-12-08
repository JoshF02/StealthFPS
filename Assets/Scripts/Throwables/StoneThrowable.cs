using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneThrowable : Throwable
{
    private bool hasCollided = false;

    void OnCollisionEnter(Collision collision)  // only generates sound once it hits ground/object
    {
        if (!hasCollided) {
            Debug.Log("generating noise now");
            SoundGenerator.Instance.GenerateSound(transform, 20, 0.5f);
            hasCollided = true;
        }
    }
}
