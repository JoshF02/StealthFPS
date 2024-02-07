using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StealInteractible : MonoBehaviour, IInteractible
{
    [SerializeField] private string interactText;

    public void Interact(Transform interactorTransform)
    {
        Debug.Log("Steal item! " + transform.gameObject);

        if (GameManager.Instance.GetObjective() == Objectives.Steal) {  // should always be true if this object exists
            GameManager.Instance.CompleteObjective();
            GameManager.Instance.IncreaseScoreBy(500);
            Destroy(gameObject);
        }
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
