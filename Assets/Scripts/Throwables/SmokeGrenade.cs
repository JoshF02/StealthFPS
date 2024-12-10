using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeGrenade : Throwable
{
    [SerializeField] private GameObject _smoke;
    private GameObject _smokeObj;
    private float _timer = 0f;
    private bool _hasSpawned = false;

    private void Update()
    {
        _timer += Time.deltaTime;

        if (_timer > 3.0f && !_hasSpawned)
        {
            _smokeObj = Instantiate(_smoke, transform.position, Quaternion.identity);  // will need to change smoke texture
            GetComponent<MeshRenderer>().enabled = false;
            _hasSpawned = true;
        }

        if (_timer > 18.0f)
        {
            Destroy(_smokeObj);
            Destroy(gameObject);
        }
    }
}
