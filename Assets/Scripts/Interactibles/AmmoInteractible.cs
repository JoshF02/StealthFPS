using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoInteractible : MonoBehaviour, IInteractible
{
    [SerializeField] private string _interactText;
    [SerializeField] private Transform _weaponHolder;

    public void Interact(Transform interactorTransform)
    {
        foreach (Transform weapon in _weaponHolder) // refill all weapons
        {        
            Gun weaponGun = weapon.GetComponent<Gun>();
            weaponGun.RefillAmmo();
        }

        //Destroy(gameObject);
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
