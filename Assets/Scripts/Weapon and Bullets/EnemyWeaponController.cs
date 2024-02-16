using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeaponController : Weapon
{
    [SerializeField] Enemy _myEnemyInstance;
    private void Update()
    {
        _myBulletSpawnPoint.LookAt(_myEnemyInstance.playerPos);
    }

}
