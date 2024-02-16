using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSlot : MonoBehaviour
{
    public GameObject equippedWeapon;
    public Weapon currentWeapon;

    public Animator cameraAnimator;

    //Armas
    public bool hasRifle = false;
    public bool hasShotgun = false;

    public GameObject[] weaponSlots;

    //UI Element
    [SerializeField]
    private GameObject ammoUI;

    private void Start()
    {
        ammoUI.SetActive(false);
    }

    /* Para usar en el futuro
    public void WeaponChange(Weapon.weaponType weaponType)
    {
        switch (weaponType)
        {
            case Weapon.weaponType.rifle:
                equippedWeapon = weaponSlots[0];
                UnequipAll();
                Equip();
                break;
            case Weapon.weaponType.shotgun:
                UnequipAll();
                Equip();
                equippedWeapon = weaponSlots[1];
                break;
        }

        currentWeapon = equippedWeapon.GetComponent<Weapon>();
    }
    */

    public void WeaponChange(int weaponIndex)
    {
        switch (weaponIndex)    
        {
            case 0:
                if (hasRifle)
                {
                    equippedWeapon = weaponSlots[0];
                    UnequipAll();
                    Equip();
                }
                break;
            case 1:
                if (hasShotgun)
                {
                    equippedWeapon = weaponSlots[1];
                    UnequipAll();
                    Equip();
                }  
                break;
        }

        currentWeapon = equippedWeapon.GetComponent<Weapon>();
    }

    private void UnequipAll()
    {
        for (int i = 0; i < weaponSlots.Length; i++)
        {
            weaponSlots[i].SetActive(false);
        }

        ammoUI.SetActive(false);
        cameraAnimator.SetBool("isHoldingGun", false);
    }

    private void Equip()
    {
        equippedWeapon.SetActive(true);
        ammoUI.SetActive(true);
        cameraAnimator.SetBool("isHoldingGun", true);
    }
}
