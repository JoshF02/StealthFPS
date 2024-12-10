using UnityEngine;

public class WeaponSwitching : MonoBehaviour
{
    [field:SerializeField] public int SelectedWeapon { get; private set; } = 0;

    private void Start()
    {
        SelectWeapon();
    }

    private void Update()
    {
        int previousSelectedWeapon = SelectedWeapon;

        if ((Input.GetAxis("Mouse ScrollWheel") > 0f) || Input.GetKeyDown(KeyCode.Y))
        {
            if (SelectedWeapon >= transform.childCount - 1) SelectedWeapon = 0;
            else SelectedWeapon++;
        }
        if ((Input.GetAxis("Mouse ScrollWheel") < 0f) || Input.GetKeyDown(KeyCode.H))
        {
            if (SelectedWeapon <= 0) SelectedWeapon = transform.childCount - 1;
            else SelectedWeapon--;
        }

        if (Input.GetKeyDown(KeyCode.Alpha1)) SelectedWeapon = 0;
        if (Input.GetKeyDown(KeyCode.Alpha2) && (transform.childCount >= 2)) SelectedWeapon = 1;
        if (Input.GetKeyDown(KeyCode.Alpha3) && (transform.childCount >= 3)) SelectedWeapon = 2;
        if (Input.GetKeyDown(KeyCode.Alpha4) && (transform.childCount >= 4))  SelectedWeapon = 3;

        if (previousSelectedWeapon != SelectedWeapon) SelectWeapon();
    }

    void SelectWeapon()
    {
        int i = 0;
        foreach (Transform weapon in transform)
        {
            if ((i == SelectedWeapon)) weapon.gameObject.SetActive(true);
            else weapon.gameObject.SetActive(false);
            
            i++;
        }
    }
}
