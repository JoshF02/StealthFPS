using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneThrowable : Throwable
{
    private bool _hasCollided = false;

    private void OnCollisionEnter(Collision collision)  // only generates sound once it hits ground/object
    {
        if (!_hasCollided)
        {
            Debug.Log("generating noise now");
            SoundGenerator.Instance.GenerateSound(transform, 20, 0.5f);
            _hasCollided = true;
        }
    }
}
