using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class Gun : MonoBehaviour
{
    [SerializeField] private float _damage = 10f;
    [SerializeField] private float range = 100f;
    [SerializeField] private float _fireRate = 15f;
    private TextMeshProUGUI _ammoText;
    private TextMeshProUGUI _carriedAmmoText;
    private GameObject _hitmarker;
    private Camera _fpsCam;
    //[SerializeField] private ParticleSystem _muzzleFlash;
    //[SerializeField] private GameObject _impactEffect;
    [SerializeField] private int _maxAmmo;
    [SerializeField] private int _currentAmmo;
    [SerializeField] private int _maxCarriedAmmo;
    [SerializeField] private int _currentCarriedAmmo;
    [SerializeField] float _reloadTime;
    [SerializeField] private LayerMask _shootingLayerMask;
    [SerializeField] private Transform _partToRecoil;
    [SerializeField] private float _partRecoilSpeed = 20f;
    [SerializeField] private float _partRecoilDistance;
    private Vector3 _originalPosition;
    private Vector3 _recoiledPosition;
    private WaitForSeconds _reloadWait;
    private float _maxRecoil = -5f;
    private float _recoilSpeed = 10f;
    private float _recoil = 0f;
    private float _hitmarkerTimer = 0f;
    private bool _isReloading = false;
    private float _nextTimeToFire = 0f;

    private void Awake()
    {
        Transform canvas = GameObject.FindWithTag("Canvas").transform;
        _ammoText = canvas.GetChild(2).GetComponent<TextMeshProUGUI>();
        _carriedAmmoText = canvas.GetChild(3).GetComponent<TextMeshProUGUI>();
        _fpsCam = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        _hitmarker = canvas.GetChild(6).gameObject;

        _reloadWait = new WaitForSeconds(_reloadTime);
        _currentAmmo = _maxAmmo;
        _currentCarriedAmmo = _maxCarriedAmmo;

        _originalPosition = _partToRecoil.localPosition;
        _recoiledPosition = new Vector3(_partToRecoil.localPosition.x, _partToRecoil.localPosition.y, _partToRecoil.localPosition.z - _partRecoilDistance);
    }


    private void Update()
    {
        _ammoText.text = _currentAmmo.ToString();
        _carriedAmmoText.text = _currentCarriedAmmo.ToString();

        if (CanShoot() && Input.GetButtonDown("Fire1") && (Time.time >= _nextTimeToFire))   //change to GetButton for auto fire
        {  
            _nextTimeToFire = Time.time + 1f / _fireRate;
            Shoot();
            _recoil += 0.1f;
        }

        ApplyWeaponRecoil();
            
        if (Input.GetKeyDown(KeyCode.R)) StartCoroutine(Reload());

        if (_hitmarkerTimer > 0)
        {
            _hitmarkerTimer -= Time.deltaTime;
            if (_hitmarkerTimer < 0) _hitmarker.SetActive(false);
        }
    }

    private void Shoot()
    {
        _currentAmmo--;
        //muzzleFlash.Play();

        if (Physics.Raycast(_fpsCam.transform.position, _fpsCam.transform.forward, out RaycastHit hit, range, _shootingLayerMask))
        {
            string tag = hit.collider.gameObject.tag;
            float multiplier = 1f;

            //if (tag == "Head") multiplier = 2f;
            //else if (tag == "Limb") multiplier = 0.5f;
            if (tag == "Weakness") multiplier = 10000f;
 
            Target target = hit.transform.root.GetComponent<Target>();

            if ((target == null) && (hit.transform.root.childCount > 0)) target = hit.transform.root.GetChild(0).GetComponent<Target>();   // enemy no longer root so gets child

            if (target != null)
            {
                target.TakeDamage(_damage * multiplier);
                _hitmarker.SetActive(true);
                _hitmarkerTimer = 0.1f;
            }

            //GameObject impactGO = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            //Destroy(impactGO, 2f);
        }

        PlayerSound.Instance.PlaySoundForDuration("shoot", 40, 0.1f);
    }

    private bool CanShoot()
    {
        if (_isReloading) return false;

        return (_currentAmmo > 0);
    }

    private IEnumerator Reload()
    {
        if ((_currentAmmo == _maxAmmo) || (_currentCarriedAmmo == 0))
        {
            yield return null;
        }
        else
        {
            _isReloading = true;
            yield return _reloadWait;

            if (_currentCarriedAmmo < _maxAmmo)
            {
                _currentAmmo = _currentCarriedAmmo;
                _currentCarriedAmmo = 0;
            }
            else
            {
                _currentCarriedAmmo -= (_maxAmmo - _currentAmmo);
                _currentAmmo = _maxAmmo;
            }
            _isReloading = false;
        }
    }

    private void ApplyWeaponRecoil()
    {
        if(_recoil > 0)
        {
            transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(_maxRecoil, 0, 0), Time.deltaTime * _recoilSpeed);
            _recoil -= Time.deltaTime;
            _partToRecoil.localPosition = Vector3.Lerp(_partToRecoil.localPosition, _recoiledPosition, Time.deltaTime * _partRecoilSpeed);
        }
        else
        {
            transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(0, 0, 0), Time.deltaTime * _recoilSpeed / 2);
            _recoil = 0;
            _partToRecoil.localPosition = Vector3.Lerp(_partToRecoil.localPosition, _originalPosition, Time.deltaTime * _partRecoilSpeed);
        }
    }

    public void RefillAmmo()
    {
        _currentAmmo = _maxAmmo;
        _currentCarriedAmmo = _maxCarriedAmmo;
    }
}
