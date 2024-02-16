using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public enum EnemyState { Aggro, Passive, Stagger, Attack, Dead };
    [SerializeField] protected float speed;
    [SerializeField] protected float hp;
    [SerializeField] private float staggerHp;
    public EnemyState enemyState;
    [SerializeField] protected Animator animator;
    [HideInInspector] public Transform playerPos;
    private Vector3 directionToPlayer;
    private float distanceFromPlayer;
    [SerializeField] private float aggroRange;
    [SerializeField] protected float attackRange;
    [SerializeField] protected float rotationSpeed;
    [SerializeField] protected Rigidbody _myRigidbody;
    [SerializeField] protected NavMeshAgent _myAgent;

    [SerializeField]
    protected GameObject[] DropList;

    [SerializeField]
    protected AudioClip _HurtAudioClip;

    [SerializeField]
    protected AudioClip _DeadAudioclip;

    [SerializeField] protected SoundHelper _mySoundHelper;

    [SerializeField] float _aggroToPassiveTimer;
    float _timerToPasive;
    [SerializeField] protected Vector3 _myInitialPosition;
    // Start is called before the first frame update
    public virtual void Start()
    {
        _myInitialPosition = transform.position;
        enemyState = EnemyState.Passive;
        playerPos = GameObject.FindGameObjectWithTag("Player").transform;
        //_EnemyMeleeMovementController = new EnemyMeleeMovementController(this);
    }

    public virtual void Update()
    {
    }

    //recibir da;o public void handleDamage
    public void CheckRangeAndAggro()
    {
        distanceFromPlayer = Vector3.Distance(playerPos.position, transform.position);

        if (distanceFromPlayer <= aggroRange && distanceFromPlayer >= attackRange)
        {
            _timerToPasive = 0f;
            enemyState = EnemyState.Aggro;
        }
        else if (distanceFromPlayer < attackRange)
        {
            _timerToPasive = 0f;
            enemyState = EnemyState.Attack;
        }
        else if (_timerToPasive >= _aggroToPassiveTimer)
        {
            _timerToPasive = 0f;
            enemyState = EnemyState.Passive;
        }
        else
            _timerToPasive += Time.deltaTime;

        directionToPlayer = playerPos.position - transform.position;
        directionToPlayer.Normalize();
    }
    public virtual void EnemyDamageHandler(float Damage)
    {
        //Debug.Log("Entro");
        switch (enemyState)
        {
            case EnemyState.Dead: //estoy muerto no quiero recibir mas daño
                break;

            default:

                hp -= Damage;
                _mySoundHelper.PlaySound(_HurtAudioClip, 1f);
                if (hp <= 0) //si me mori disparo animacion y cambio estado
                {
                    EnemyDeadth();
                }
                break;
        }
    }

    private void EnemyDeadth()
    {
        if (_myAgent != null)
        _myAgent.isStopped = true;

        animator.SetTrigger("isDead");
        _mySoundHelper.PlaySound(_DeadAudioclip, 1f);
        enemyState = EnemyState.Dead;
        StartCoroutine(ShrinkToDead());

        //Instanceo el loot
        for (int i = 0; i < DropList.Length; i++)
            Instantiate(DropList[i], transform.position + new Vector3(0, 1, 0), transform.rotation);
        //el vector 3 es para corregir un error con los boxColliders. El transform.position esta en el piso y clipean los boxcoliders

    }

    protected IEnumerator ShrinkToDead()
    {
        Vector3 startScale = transform.localScale;
        float timer = 0.0f;
        float duration = 5f;
        _myRigidbody.isKinematic = true;
        while (timer < duration)
        {
            //se achica mientras muere
            timer += Time.deltaTime;
            float t = timer / duration;

            //smoother step algorithm
            t = t * t * t *t * (t * (6f * t - 15f) + 10f);
            transform.localScale = Vector3.Lerp(startScale, new Vector3(0.1f,0.1f,0.1f), t);
            yield return null;
        }

        Destroy(this.gameObject);
        yield return null;

    }

}
