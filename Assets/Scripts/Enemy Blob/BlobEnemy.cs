using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BlobEnemy: EnemyMelee
{
    [SerializeField] BoxCollider _myAoEExplosionCollider;
    [SerializeField] float _myMaxSizeLowHP; //Multiplier to a scale of 1,1,1
    [SerializeField] int _DeathRingsCount; //Multiplier to a scale of 1,1,1
    [SerializeField] GameObject _myRingAttackPrefab; //Multiplier to a scale of 1,1,1
    [SerializeField] float _attackAdvanceStep;
    float _myMaxHealth;
    Vector3 _myScale;
    float _myScaleMultiplier;

    public override void Start()
    {
        base.Start();
        _myMaxHealth = hp;
    }

    private float GetMultiplier(float currentHP, float maxHP)
    {
        //Range [1,maxSize]
        //=((maxSize-1)*(current-maxHP)/(minHPforMaxSize-maxHP))+1
        float scale = ((_myMaxSizeLowHP - 1) * (currentHP - maxHP) / (15 - maxHP)) + 1;

        return scale > _myMaxSizeLowHP ? _myMaxSizeLowHP : scale;
    }

    public override void EnemyDamageHandler(float Damage)
    {
        //Debug.Log("Entro");
        switch (enemyState)
        {
            case EnemyState.Dead: //estoy muerto no quiero recibir mas daño
                break;

            default:

                hp -= Damage;
                _mySoundHelper.PlaySound(_HurtAudioClip, 1f);

                _myScale = transform.localScale;

                _myScaleMultiplier = GetMultiplier(hp, _myMaxHealth);

                _myScale.x = _myScaleMultiplier;
                _myScale.y = _myScaleMultiplier;
                _myScale.z = _myScaleMultiplier;

                transform.localScale = _myScale;

                if (hp <= 0) //si me mori disparo animacion y cambio estado
                    EnemyDeadth();

                break;
        }
    }

    private void EnemyDeadth()
    {
        animator.SetTrigger("isDead");
        _mySoundHelper.PlaySound(_DeadAudioclip, 1f);
        enemyState = EnemyState.Dead;
        _myAgent.isStopped = true;
    }

    public void ExplosionAoE()
    {
        for (int i = 0; i < _DeathRingsCount; i++)
            Instantiate(_myRingAttackPrefab, transform.position + new Vector3(0, 2f*i/_DeathRingsCount + 0.2f, 6f), transform.rotation);
        //Instanceo el loot

        StartCoroutine(ShrinkToDead());

        for (int i = 0; i < DropList.Length; i++)
            Instantiate(DropList[i], transform.position + new Vector3(0, 1, 0), transform.rotation);
        //el vector 3 es para corregir un error con los boxColliders. El transform.position esta en el piso y clipean los boxcoliders
    }

    public IEnumerator AttackJumpAdvance()
    {
        float timer = 0;

        while (timer <= 0.7f)
        {
            timer += Time.deltaTime;
            this.transform.position +=  transform.forward * _attackAdvanceStep;
            yield return 0;
        }
    }

    public void StartAttackAdvance()
    {
        StartCoroutine(AttackJumpAdvance());
    }
}
