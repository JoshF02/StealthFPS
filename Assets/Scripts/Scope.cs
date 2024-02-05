using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scope : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject scopeOverlay;
    [SerializeField] private GameObject crosshair;
    [SerializeField] private GameObject weaponCamera;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private float scopedFOV = 15f;
    private float normalFOV;
    private bool isScoped = false;
    [SerializeField] private int sniperNumID;
    [SerializeField] private int selectedWeapon;

    private WeaponSwitching weaponSwitching;
    private PlayerActions playerActions;

    void Start ()
    {
        sniperNumID = 99; //sets it to high num in case no sniper found
        weaponSwitching = GetComponent<WeaponSwitching>();
        selectedWeapon = weaponSwitching.selectedWeapon;
        int i = 0;
        foreach (Transform weapon in transform) {
            if (weapon.CompareTag("Sniper")) sniperNumID = i;
            i++;
        }

        playerActions = transform.root.GetComponent<PlayerActions>();
    }

    void Update ()
    {
        selectedWeapon = weaponSwitching.selectedWeapon;

        if (Input.GetButtonDown("Fire2")) {
            isScoped = !isScoped;
            animator.SetBool("Scoped", isScoped);

            if (isScoped) StartCoroutine(OnScoped());
            else OnUnScoped();
        }
    }

    void OnUnScoped ()
    {
        crosshair.SetActive(true);
        playerActions.isAiming = false;

        if(selectedWeapon == sniperNumID) {
            scopeOverlay.SetActive(false);
            weaponCamera.SetActive(true);

            mainCamera.fieldOfView = normalFOV;
        }
    }

    IEnumerator OnScoped ()
    {
        crosshair.SetActive(false);
        playerActions.isAiming = true;
        //Debug.Log(transform.parent.localPosition);
        transform.parent.localPosition = Vector3.zero;
        //transform.parent.localRotation = Quaternion.Euler(0,0,0);
        
        if(selectedWeapon == sniperNumID) {
            yield return new WaitForSeconds(.25f);

            scopeOverlay.SetActive(true);
            weaponCamera.SetActive(false);

            normalFOV = mainCamera.fieldOfView;
            mainCamera.fieldOfView = scopedFOV;
        }
    }
}
