using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereInteractible : MonoBehaviour, IInteractible
{
    [SerializeField] private string _interactText;

    public void Interact(Transform interactorTransform)
    {
        Debug.Log("Interact! " + transform.gameObject);
        Destroy(gameObject);
    }

    public string GetInteractText()
    {
        return _interactText;
    }

    public Transform GetTransform()
    {
        return transform;
    }
}
