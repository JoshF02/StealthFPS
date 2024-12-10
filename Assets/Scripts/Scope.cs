using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scope : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private GameObject _scopeOverlay;
    [SerializeField] private GameObject _crosshair;
    [SerializeField] private GameObject _weaponCamera;
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private float _scopedFOV = 15f;
    [SerializeField] private int _sniperNumID;
    [SerializeField] private int _selectedWeapon;
    private float _normalFOV;
    private bool _isScoped = false;
    private WeaponSwitching _weaponSwitching;
    private PlayerActions _playerActions;

    private void Start()
    {
        _sniperNumID = 99; //sets it to high num in case no sniper found
        _weaponSwitching = GetComponent<WeaponSwitching>();
        _selectedWeapon = _weaponSwitching.SelectedWeapon;
        int i = 0;
        foreach (Transform weapon in transform)
        {
            if (weapon.CompareTag("Sniper")) _sniperNumID = i;
            i++;
        }

        _playerActions = transform.root.GetComponent<PlayerActions>();
    }

    private void Update()
    {
        _selectedWeapon = _weaponSwitching.SelectedWeapon;

        if (Input.GetButtonDown("Fire2"))
        {
            _isScoped = !_isScoped;
            _animator.SetBool("Scoped", _isScoped);

            if (_isScoped) StartCoroutine(OnScoped());
            else OnUnScoped();
        }
    }

    private void OnUnScoped()
    {
        _crosshair.SetActive(true);
        _playerActions.IsAiming = false;

        if(_selectedWeapon == _sniperNumID)
        {
            _scopeOverlay.SetActive(false);
            _weaponCamera.SetActive(true);
            _mainCamera.fieldOfView = _normalFOV;
        }
    }

    private IEnumerator OnScoped ()
    {
        _crosshair.SetActive(false);
        _playerActions.IsAiming = true;
        transform.parent.localPosition = Vector3.zero;
        //transform.parent.localRotation = Quaternion.Euler(0,0,0);
        
        if(_selectedWeapon == _sniperNumID)
        {
            yield return new WaitForSeconds(.25f);

            _scopeOverlay.SetActive(true);
            _weaponCamera.SetActive(false);

            _normalFOV = _mainCamera.fieldOfView;
            _mainCamera.fieldOfView = _scopedFOV;
        }
    }
}
