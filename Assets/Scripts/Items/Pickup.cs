using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    private enum PickupType
    {
        health, armor, ammo, upgrade
    }

    [SerializeField] private int _amount;
    [SerializeField] private string _upgradeType;
    [SerializeField] private PickupType _pickupType;
    [SerializeField] private int _rotationSpeed;
    [SerializeField] float _totaltravelTime;
    [SerializeField] GameObject[] _myTrailRenderers;
    [SerializeField] private PlayerSound _playerSound;

    private GameObject _player;

    private float x, z;
    private Vector3 _finalPos;
    private Vector3 _startPos;
    [SerializeField] bool _isPlaced = false;
    float _acumulatedTravelTime;
    void Start()
    {
        if (_pickupType != PickupType.upgrade)
        {
            x = Random.Range(-3f, 3f);
            z = Random.Range(-3f, 3f);

            _startPos = transform.position;
            _finalPos = transform.position + new Vector3(x, 0.5f, z);

        }

        _player = GameObject.FindGameObjectWithTag("Player");
        _playerSound = _player.GetComponent<PlayerSound>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _playerSound.PlaySound(PlayerSound.Actions.Pickup, "");

            switch (_pickupType)
            {
                case PickupType.health:
                    PlayerStats.myInstance.HealHp(_amount);
                    break;
                case PickupType.armor:
                    PlayerStats.myInstance.HealArmor(_amount);
                    break;
                case PickupType.ammo:
                    PlayerStats.myInstance.AddAmmoAmount(_amount);
                    break;
                case PickupType.upgrade:
                    if (_upgradeType == "Rifle")
                    {
                        PlayerStats.myInstance.weaponSlot.hasRifle = true;
                        PlayerStats.myInstance.weaponSlot.WeaponChange(0);
                    }
                    if (_upgradeType == "Shotgun")
                    {
                        PlayerStats.myInstance.weaponSlot.hasShotgun = true;
                        PlayerStats.myInstance.weaponSlot.WeaponChange(1);

                    }
                    else if (_upgradeType == "DoubleJump")
                    {
                        PlayerStats.myInstance.hasDoubleJump = true;
                    }
                    break;
            }

            DestroySelf();
        }
    }

    private void Update()
    {
        if (_pickupType != PickupType.upgrade)
        {
            if (!_isPlaced)
            {
                _acumulatedTravelTime += Time.deltaTime;

                transform.position = Vector3.Slerp(_startPos, _finalPos, _acumulatedTravelTime / _totaltravelTime);
                if (Vector3.SqrMagnitude(transform.position - _finalPos) < 0.0001f) //Comparacion de vectores 3, los float a veces son imprecisos
                {
                    transform.rotation = Quaternion.Euler(0f, 0f, 0f);
                    foreach (GameObject trail in _myTrailRenderers)
                        trail.SetActive(true);
                    _isPlaced = true;
                }
            }

        }
        transform.Rotate(Vector3.up * _rotationSpeed * Time.deltaTime);


    }
    private void DestroySelf()
    {
        Destroy(gameObject);
    }
}
