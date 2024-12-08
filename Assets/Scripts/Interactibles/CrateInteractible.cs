using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrateInteractible : MonoBehaviour, IInteractible
{
    [SerializeField] private string interactText;
    [SerializeField] private Transform weaponHolder;
    private WeaponSwitching weaponSwitching;

    [SerializeField] private GameObject sniper;
    [SerializeField] private GameObject shotgun;

    private void Awake()
    {
        weaponSwitching = weaponHolder.GetComponent<WeaponSwitching>();

        int rand = Random.Range(0, 2);

        GameObject storedWeapon = null;

        switch (rand) {
            case 0:
                storedWeapon = Instantiate(sniper);
                break;
            case 1:
                storedWeapon = Instantiate(shotgun);
                break;
        }

        storedWeapon.transform.SetParent(transform);
        storedWeapon.transform.SetLocalPositionAndRotation(new Vector3(0,0,0), Quaternion.identity);
        storedWeapon.SetActive(false);
    }

    public void Interact(Transform interactorTransform)
    {
        Debug.Log("Pick up weapon!");

        Transform storeWeapon = weaponHolder.GetChild(weaponSwitching.selectedWeapon);
        storeWeapon.SetParent(transform);
        storeWeapon.SetLocalPositionAndRotation(new Vector3(0,0,0), Quaternion.identity);
        storeWeapon.gameObject.SetActive(false);

        Transform pickupWeapon = transform.GetChild(0);
        pickupWeapon.SetParent(weaponHolder);
        pickupWeapon.SetLocalPositionAndRotation(new Vector3(0, pickupWeapon.tag == "Sniper" ? -0.06f : 0f,0), Quaternion.identity);
        pickupWeapon.gameObject.SetActive(true);
        pickupWeapon.SetSiblingIndex(weaponSwitching.selectedWeapon);
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
