using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoInteractible : MonoBehaviour, IInteractible
{
    [SerializeField] private string interactText;
    [SerializeField] private Transform weaponHolder;
    /*private WeaponSwitching weaponSwitching;

    private void Awake()
    {
        weaponSwitching = weaponHolder.GetComponent<WeaponSwitching>();
    }*/

    public void Interact(Transform interactorTransform)
    {
        //Gun heldWeapon = weaponHolder.GetChild(weaponSwitching.selectedWeapon).GetComponent<Gun>();   // refill only current weapon
        //heldWeapon.RefillAmmo();

        foreach (Transform weapon in weaponHolder) {        // refill all weapons
            Gun weaponGun = weapon.GetComponent<Gun>();
            weaponGun.RefillAmmo();
        }

        //Destroy(gameObject);
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
