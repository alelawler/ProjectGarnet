using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAnimHelper : MonoBehaviour
{
    private PlayerStats _playerStats;
    private PlayerWeaponController _weaponController;
    private Animator _animator;
    [SerializeField]private PlayerCharacterController _myPlayerCharacterController;

    private void Start()
    {
        _playerStats = PlayerStats.myInstance;
        _animator = GetComponent<Animator>();
        //_weaponController = _playerStats.weaponSlot.equippedWeapon.GetComponent<PlayerWeaponController>();
    }
    private void Reload()
    {
        if (_weaponController != null)
        {
            _weaponController.Reload();
        }
        else
        {
            _weaponController = _playerStats.weaponSlot.equippedWeapon.GetComponent<PlayerWeaponController>();
            _weaponController.Reload();
        }
        _playerStats.Reload();
    }

    private void FinishReload()
    {
        _myPlayerCharacterController.FinishReload();
    }

}
