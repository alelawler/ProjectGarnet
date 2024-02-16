using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [Header("Total Ammo in Current Clip")]
    public int currentAmmo;

    [Header("MMax Clip Ammo")]
    public int maxClipAmmo;

    [Header("Total Ammo Counting All Mags")]
    public int totalCurrentAmmo;

    [Header("Max Ammo For a Given Weapon")]
    public int totalCurrentAmmoMax;

    [Header("Timer between bullets")]
    public float shootCD;

    [Header("Weapon Recoil")]
    public Vector3 upRecoil;

    [Header("Weapon Sounds")]
    [SerializeField] protected GameObject _myBulletPrefab;
    [SerializeField] protected AudioClip _ShotAudioClip;
    [SerializeField] protected AudioClip _reloadAudioClip;
    [SerializeField] public SoundHelper _mySoundHelper;

    [Header("Projectile for the Current Weapon")]
    [SerializeField] protected int projectilesShot;

    public Transform _myBulletSpawnPoint;

    public enum weaponType
    {
        rifle, shotgun
    }

    [Header("Type of Weapon")]
    [SerializeField] public weaponType myWeaponType;

    private Recoil _recoil;

    private void Start()
    {
        gameObject.AddComponent<Recoil>();
        _recoil = GetComponent<Recoil>();
        _mySoundHelper = GetComponent<SoundHelper>();
    }

    public void Shoot()
    {
        for (int i = 0; i < projectilesShot; i++)
        {
            switch (myWeaponType)
            {
                case weaponType.rifle:
                    var bulletRifle = Instantiate(_myBulletPrefab, _myBulletSpawnPoint.position, _myBulletSpawnPoint.rotation);
                    bulletRifle.GetComponent<Bullet>().enemyPosition = _myBulletSpawnPoint;
                    break;
                case weaponType.shotgun:
                    if (i == 0)
                    { 
                        var bulletShotgun = Instantiate(_myBulletPrefab, _myBulletSpawnPoint.position, _myBulletSpawnPoint.rotation);
                        bulletShotgun.GetComponent<Bullet>().enemyPosition = _myBulletSpawnPoint;
                    }
                    else
                    { 
                        Instantiate(_myBulletPrefab, _myBulletSpawnPoint.position, _myBulletSpawnPoint.rotation * Quaternion.Euler(Random.Range(-3f, 3f), Random.Range(-3f, 3f), 0));
                    }
                    break;
                default:
                    break;
            }

            //_recoil.OnShootRecoil(upRecoil);
        }

        float pitch = Random.Range(.7f, 1.4f);
        _mySoundHelper.PlaySound(_ShotAudioClip, 1f, pitch);
    }
}
