using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereInteractible : MonoBehaviour, IInteractible
{
    // to make new interactible object type, create a script like this with different name + text + logic
    [SerializeField] private string interactText;

    public void Interact(Transform interactorTransform)
    {
        Debug.Log("Interact! " + transform.gameObject);
        // put interaction logic here
        Destroy(gameObject);
    }

    public string GetInteractText()
    {
        return interactText;
    }

    public Transform GetTransform()
    {
        return transform;
    }
}
