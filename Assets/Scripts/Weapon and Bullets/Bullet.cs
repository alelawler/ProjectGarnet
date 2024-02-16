using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    protected Rigidbody myRigidBody;

    [SerializeField]
    protected int speed;

    [SerializeField]
    protected int damage;

    [SerializeField]
    protected int bulletTimer;

    protected float deathTimer;

    [SerializeField]
    public Transform enemyPosition;
    private Transform _enemyPosition;

    private void Awake()
    {
        myRigidBody.AddForce(transform.forward * speed, ForceMode.VelocityChange);
        _enemyPosition = this.transform;
    }
    private void Update()
    {
        deathTimer += Time.deltaTime;

        if (deathTimer >= bulletTimer)
            Destroy(this.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (this.CompareTag("EnemyBullet") && other.CompareTag("Player"))
        { 
            PlayerStats.myInstance.Damage(damage, enemyPosition);            
        }
        else if (this.CompareTag("PlayerBullet") && other.tag.IndexOf("Enemy") == 0)
        {
            other.GetComponent<Enemy>().EnemyDamageHandler(damage);
        }
        Destroy(this.gameObject);
    }

}
