using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EMPGrenade : Throwable
{
    private bool hasCollided = false;

    void OnCollisionEnter(Collision collision) 
    {
        if (!hasCollided) {
            hasCollided = true;
            StartCoroutine(Detonation());
        }
    }

    IEnumerator Detonation()
    {
        //Debug.Log("EMP hit ground");

        yield return new WaitForSeconds(1.0f);
        Debug.Log("EMP detonating");
        GetComponent<MeshRenderer>().enabled = false; 
        transform.GetChild(0).gameObject.SetActive(true);
        
        yield return new WaitForSeconds(0.1f);
        //Debug.Log("destroying EMP");
        Destroy(gameObject);
    }
}
