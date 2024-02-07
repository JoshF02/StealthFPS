using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class Gun : MonoBehaviour
{
    [SerializeField] private float damage = 10f;
    [SerializeField] private float range = 100f;
    [SerializeField] private float fireRate = 15f;
    private bool isReloading = false;
    private float nextTimeToFire = 0f;

    [SerializeField] private TextMeshProUGUI ammoText;
    [SerializeField] private TextMeshProUGUI carriedAmmoText;

    [SerializeField] private Camera fpsCam;

    //[SerializeField] private ParticleSystem muzzleFlash;
    //[SerializeField] private GameObject impactEffect;

    [SerializeField] private int maxAmmo;
    [SerializeField] private int currentAmmo;

    [SerializeField] private int maxCarriedAmmo;
    [SerializeField] private int currentCarriedAmmo;

    [SerializeField] float reloadTime;
    private WaitForSeconds reloadWait;


    private float maxRecoil = -20f;
    private float recoilSpeed = 10f;
    private float recoil = 0f;

    [SerializeField] private LayerMask shootingLayerMask;

    [SerializeField] private Transform partToRecoil;
    [SerializeField] private float partRecoilSpeed = 20f;
    [SerializeField] private float partRecoilDistance;
    private Vector3 originalPosition;
    private Vector3 recoiledPosition;




    private void Awake()
    {
        Transform canvas = GameObject.FindWithTag("Canvas").transform;
        ammoText = canvas.GetChild(2).GetComponent<TextMeshProUGUI>();
        carriedAmmoText = canvas.GetChild(3).GetComponent<TextMeshProUGUI>();
        fpsCam = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();

        reloadWait = new WaitForSeconds(reloadTime);
        currentAmmo = maxAmmo;
        currentCarriedAmmo = maxCarriedAmmo;

        originalPosition = partToRecoil.localPosition;
        recoiledPosition = new Vector3(partToRecoil.localPosition.x, partToRecoil.localPosition.y, partToRecoil.localPosition.z - partRecoilDistance);
    }


    private void Update()
    {
        ammoText.text = currentAmmo.ToString();
        carriedAmmoText.text = currentCarriedAmmo.ToString();

        if (CanShoot()) { 
            if (Input.GetButtonDown("Fire1") && Time.time >= nextTimeToFire) {  //change to GetButton for auto fire
                nextTimeToFire = Time.time + 1f / fireRate;
                Shoot();
                recoil += 0.1f;
            }
        }

        ApplyWeaponRecoil();
            
        if (Input.GetKeyDown(KeyCode.R)) StartCoroutine(Reload());
    }

    void Shoot()
    {
        currentAmmo--;
        //muzzleFlash.Play();

        RaycastHit hit;
        if(Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range, shootingLayerMask)) {
            string tag = hit.collider.gameObject.tag;
            float multiplier = 1f;

            if (tag == "Head") multiplier = 2f;
            else if (tag == "Limb") multiplier = 0.5f;
            
            //Debug.Log(hit.collider.transform.name); 
            Target target = hit.transform.root.GetComponent<Target>();
            if (target != null) target.TakeDamage(damage * multiplier);

            //GameObject impactGO = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            //Destroy(impactGO, 2f);
        }
    }

    bool CanShoot()
    {
        if (isReloading) return false;

        return (currentAmmo > 0);
    }

    IEnumerator Reload()
    {
        if (currentAmmo == maxAmmo || currentCarriedAmmo == 0) {
            yield return null;
        }
        else {
            isReloading = true;
            yield return reloadWait;
            if (currentCarriedAmmo < maxAmmo) {
                currentAmmo = currentCarriedAmmo;
                currentCarriedAmmo = 0;
            }
            else {
                currentCarriedAmmo -= (maxAmmo - currentAmmo);
                currentAmmo = maxAmmo;
            }
            isReloading = false;
        }
    }

    private void ApplyWeaponRecoil()
    {
        if(recoil > 0) {
            transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(maxRecoil, 0, 0), Time.deltaTime * recoilSpeed);
            recoil -= Time.deltaTime;

            partToRecoil.localPosition = Vector3.Lerp(partToRecoil.localPosition, recoiledPosition, Time.deltaTime * partRecoilSpeed);
        }
        else {
            transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(0, 0, 0), Time.deltaTime * recoilSpeed / 2);
            recoil = 0;

            partToRecoil.localPosition = Vector3.Lerp(partToRecoil.localPosition, originalPosition, Time.deltaTime * partRecoilSpeed);
        }
    }

    public void RefillAmmo()
    {
        currentAmmo = maxAmmo;
        currentCarriedAmmo = maxCarriedAmmo;
    }
}
