using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrateInteractible : MonoBehaviour, IInteractible
{
    [SerializeField] private string _interactText;
    [SerializeField] private Transform _weaponHolder;
    [SerializeField] private GameObject _sniper;
    [SerializeField] private GameObject _shotgun;
    private WeaponSwitching _weaponSwitching;

    private void Awake()
    {
        _weaponSwitching = _weaponHolder.GetComponent<WeaponSwitching>();

        GameObject storedWeapon = null;
        int rand = Random.Range(0, 2);
        
        switch (rand)
        {
            case 0:
                storedWeapon = Instantiate(_sniper);
                break;
            case 1:
                storedWeapon = Instantiate(_shotgun);
                break;
        }

        storedWeapon.transform.SetParent(transform);
        storedWeapon.transform.SetLocalPositionAndRotation(new Vector3(0,0,0), Quaternion.identity);
        storedWeapon.SetActive(false);
    }

    public void Interact(Transform interactorTransform)
    {
        Debug.Log("Pick up weapon!");

        Transform storeWeapon = _weaponHolder.GetChild(_weaponSwitching.SelectedWeapon);
        storeWeapon.SetParent(transform);
        storeWeapon.SetLocalPositionAndRotation(new Vector3(0,0,0), Quaternion.identity);
        storeWeapon.gameObject.SetActive(false);

        Transform pickupWeapon = transform.GetChild(0);
        pickupWeapon.SetParent(_weaponHolder);
        pickupWeapon.SetLocalPositionAndRotation(new Vector3(0, pickupWeapon.tag == "Sniper" ? -0.06f : 0f,0), Quaternion.identity);
        pickupWeapon.gameObject.SetActive(true);
        pickupWeapon.SetSiblingIndex(_weaponSwitching.SelectedWeapon);
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
