using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMeleeAttackAnimController : MonoBehaviour
{
    [SerializeField]
    BoxCollider _myAttackBoxCollider;

    //For BlobEnemy
    [SerializeField]
    GameObject _BlobAoEAttack;

    [SerializeField]
    NavMeshAgent _myAgent;
    [SerializeField]
    EnemyMelee _myEnemyMeleeInstance;
    [SerializeField] GameObject _JumpParticle;

    [SerializeField]
    Animator _myAnimator;
    public void EndMeleeAttackAnim()
    {
        _myAnimator.ResetTrigger("isAttacking");
        _myAgent.isStopped = false;
        _myEnemyMeleeInstance.isAttacking = false;
        _myEnemyMeleeInstance.enemyState = Enemy.EnemyState.Aggro;
    }

    public void MeleeAttackCollisionOn()
    {
        _myAttackBoxCollider.enabled = true;


        //Physics.IgnoreLayerCollision(13, 11, false);
        //Physics.IgnoreLayerCollision(13, 12, false);
    }

    public void MeleeAttackCollisionOff()
    {
        _myAttackBoxCollider.enabled = false;
        //Physics.IgnoreLayerCollision(13, 11);
        //Physics.IgnoreLayerCollision(13, 12);
    }

    public void ResetJumpTrigger()
    {
        _myAnimator.ResetTrigger("isJumping");
    }

    public void BurstJumpParticle()
    {
        GameObject.Instantiate(_JumpParticle, transform.position, transform.rotation * Quaternion.Euler(90f, 0f, 0f));
    }

    public void BlobEnemyAoEAttack()
    {
        //Vecotr 3 in 0.6f to start attack infront of blob
        GameObject.Instantiate(_BlobAoEAttack, transform.position+new Vector3(0f,0.2f,0.6f), transform.rotation);
    }

}

