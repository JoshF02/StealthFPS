using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleporterThrowable : Throwable
{
    [SerializeField] private GameObject teleporterPrefab;
    private PlayerActions playerActions = null;

    public void SetPlayerActions(PlayerActions playerActions)
    {
        this.playerActions = playerActions;
    }

    void OnCollisionEnter(Collision collision)  // only spawns teleporter once it hits ground
    {
        if (collision.transform.tag != "Ground") return;

        Debug.Log("spawning teleporter now");
        GameObject teleporterObj = Instantiate(teleporterPrefab, transform.position, Quaternion.identity);
        playerActions.SetActiveTeleporter(teleporterObj.transform);
        Destroy(gameObject);
    }
}
