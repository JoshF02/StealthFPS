using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleporterThrowable : Throwable
{
    [SerializeField] private GameObject _teleporterPrefab;
    private PlayerActions _playerActions;

    private void Awake()
    {
        _playerActions = GameObject.FindWithTag("Player").GetComponent<PlayerActions>();
    }

    private void OnCollisionEnter(Collision collision)  // only spawns teleporter once it hits ground
    {
        if (collision.transform.tag != "Ground") return;

        Debug.Log("spawning teleporter now");
        GameObject teleporterObj = Instantiate(_teleporterPrefab, transform.position, Quaternion.identity);
        _playerActions.SetActiveTeleporter(teleporterObj.transform);
        Destroy(gameObject);
    }
}
