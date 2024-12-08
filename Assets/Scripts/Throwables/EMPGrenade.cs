using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EMPGrenade : Throwable
{
    private bool hasCollided = false;

    void OnCollisionEnter(Collision collision) 
    {
        if (!hasCollided) {
            Debug.Log("EMP detonating now");
            GetComponent<MeshRenderer>().enabled = false;
            hasCollided = true;
            transform.GetChild(0).gameObject.SetActive(true);
            StartCoroutine(Duration(0.1f));
        }
    }

    IEnumerator Duration(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Debug.Log("destroying EMP");
        Destroy(gameObject);
    }
}
