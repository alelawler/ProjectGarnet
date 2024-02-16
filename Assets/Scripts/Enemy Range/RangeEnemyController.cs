using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeEnemyController : Enemy
{
    [SerializeField]
    EnemyWeaponController _myRifleController;
    Vector3 _VectorToPlayer;
    Quaternion _rotationToPlayer;
    Quaternion _forearmsRotation;
    RaycastHit _vision;
    Transform _myBulletSpawnPoint;

    [SerializeField]
    GameObject _mySpine;
    [SerializeField]
    GameObject[] _myForearms;


    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        _myBulletSpawnPoint = _myRifleController._myBulletSpawnPoint.transform;
    }

    // Update is called once per frame
    public override void Update()
    {
        if (enemyState != EnemyState.Dead)
            base.CheckRangeAndAggro();

        switch (enemyState)
        {
            case EnemyState.Aggro:
                FollowPlayer();
                break;

            case EnemyState.Passive:
                break;

            case EnemyState.Stagger:
                break;

            case EnemyState.Attack:
                FollowPlayer();
                if (CheckAttack())
                    animator.SetTrigger("Attack");
                break;
        }

    }

    private bool CheckAttack()
    {
       //Debug.DrawRay(_myBulletSpawnPoint.position, _myBulletSpawnPoint.forward * attackRange,Color.red,0.1f);
        if (Physics.Raycast(_myBulletSpawnPoint.position, _myBulletSpawnPoint.forward * attackRange,out _vision,attackRange))
            if (_vision.collider.CompareTag("Player"))
                return true;
        return false;
    }

    private void LateUpdate()
    {
        //Sigo al player despues de que se animo
        switch (enemyState)
        {
            case EnemyState.Aggro:
                BonesRotationToPlayer();
                break;

            case EnemyState.Passive:
                break;

            case EnemyState.Stagger:
                break;

            case EnemyState.Attack:
                BonesRotationToPlayer();
                break;
        }
    }

    private void BonesRotationToPlayer()
    {

        //I need to rotate spine in X and forearm in z
        _VectorToPlayer = playerPos.position - _mySpine.transform.position;

        //guardo la rotacion
        _rotationToPlayer = Quaternion.LookRotation(_VectorToPlayer);
        _forearmsRotation = _rotationToPlayer;

        _rotationToPlayer.y = 0;
        _rotationToPlayer.z = 0;

        _forearmsRotation.x = 0;
        _forearmsRotation.y = 0;
        _forearmsRotation.z = 360 -_forearmsRotation.z;
        _mySpine.transform.rotation *= _rotationToPlayer;

        //foreach (GameObject forearm in _myForearms)
        //    forearm.transform.rotation *= _forearmsRotation;
    }

    internal void FollowPlayer()
    {
        _VectorToPlayer = playerPos.position - transform.position;
        _VectorToPlayer.y = 0;

        _rotationToPlayer = Quaternion.LookRotation(_VectorToPlayer);

        //Ajusto la rotacion por la animacion
        _rotationToPlayer *= Quaternion.Euler(0, 15, 0);
        
        transform.rotation = Quaternion.Slerp(transform.rotation, _rotationToPlayer, Time.deltaTime * rotationSpeed);
    }

    void Shoot()
    {
        _myRifleController.Shoot();
        animator.ResetTrigger("Attack");
    }
}
