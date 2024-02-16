using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMeleeAttackCollision : MonoBehaviour
{
    [SerializeField] private EnemyMelee _enemyMelee;

    [SerializeField]
    private Transform _enemyTransform;
    void OnTriggerEnter(Collider other)
    {
        //Output the name of the GameObject you collide with
        //Debug.Log("I hit the GameObject : " + other.gameObject.name);

        if (other.CompareTag("Player"))
            PlayerStats.myInstance.Damage(_enemyMelee.damage, _enemyTransform);
    }
}

