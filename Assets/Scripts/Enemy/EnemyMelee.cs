using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMelee : Enemy
{
    bool isRunning;
    public bool isAttacking;

    public int damage;


    //variables used in jump control
    [SerializeField] float _startJumpFrameNormalized;
    [SerializeField] float _endJumpFrameNormalized;
    bool _isUsingLink;
    bool _isJumping;
    OffMeshLinkData _currentLink;
    Vector3 _nextPosJump;
    float _JumpAnimTime;
    [SerializeField] string _myJumpingAnimationName;
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        _myAgent.speed = speed;
        _myAgent.stoppingDistance = attackRange;
    }

    // Update is called once per frame
    public override void Update()
    {
        //Debug.Log(_myAgent.remainingDistance);
        if (!isAttacking && enemyState != EnemyState.Dead)
        {
            CheckRangeAndAggro();
        }

        switch (enemyState)
        {
            case EnemyState.Aggro:
                isRunning = true;
                animator.SetBool("isRunning", isRunning);
                MoveEnemy(playerPos.position);
                break;

            case EnemyState.Passive:
                if (transform.position != _myInitialPosition && _myAgent.remainingDistance > 2)
                    MoveEnemy(_myInitialPosition);
                else
                {
                    isRunning = false;
                    animator.SetBool("isRunning", isRunning);
                }
                break;

            case EnemyState.Stagger:
                _myAgent.isStopped = true;
                break;

            case EnemyState.Attack:
                _myAgent.isStopped = true;
                isRunning = false;
                animator.SetBool("isRunning", isRunning);
                isAttacking = true;
                animator.SetTrigger("isAttacking");
                break;
        }

    }

    private void MoveEnemy(Vector3 destination)
    {
        if (!_myAgent.isOnOffMeshLink)
        {
            if (_myAgent.isStopped)
                _myAgent.isStopped = false;

            _myAgent.destination = destination;

        }
        else
        {
            if (!_isUsingLink)
            {
                animator.SetTrigger("isJumping");
                _currentLink = _myAgent.currentOffMeshLinkData;
                _isUsingLink = true;
            }
            //Time to use for LERP
            _JumpAnimTime = GetJumpAnimTimeNormalized();

            //If anim started
            if (_JumpAnimTime > 0) // 0 to 0.39 prep jump - 0.36+ jump anim
            {
                //Lerp from start to end
                _nextPosJump = Vector3.Lerp(_currentLink.startPos, _currentLink.endPos, _JumpAnimTime);

                ////add the 'hop'
                _nextPosJump.y += 2f * Mathf.Sin(Mathf.PI * _JumpAnimTime);

                //Update transform position
                transform.position = _nextPosJump;
            }
            else
            {
                if (!_isJumping)
                {

                    Vector3 lookPos = _currentLink.endPos - transform.position;
                    lookPos.y = 0;
                    transform.rotation = Quaternion.LookRotation(lookPos);
                }



                if (_isJumping && _JumpAnimTime == -1)
                {
                    //move agent to endPos
                    transform.position = _currentLink.endPos;
                    //reset flag
                    _isUsingLink = false;
                    _isJumping = false;
                    //Resume agent
                    _myAgent.CompleteOffMeshLink();
                    _myAgent.isStopped = false;
                }
            }

        }
    }

    private float GetJumpAnimTimeNormalized()
    {
        AnimatorStateInfo animState = animator.GetCurrentAnimatorStateInfo(0);
        if (animState.IsName(_myJumpingAnimationName))
        {
            // is only jumping from 0.39f to 0.67f, there st is prep time
            if (animState.normalizedTime < _startJumpFrameNormalized)
                return 0f;
            else if (animState.normalizedTime > _endJumpFrameNormalized)
                return 1f;
            else
            {
                _isJumping = true;
                return (animState.normalizedTime - _startJumpFrameNormalized) / (_endJumpFrameNormalized - _startJumpFrameNormalized);
            }
        }
        else
            return -1; //is not the jumping anim
    }

    private void FixedUpdate()
    {
        switch (enemyState)
        {
            case EnemyState.Dead:
                _myRigidbody.freezeRotation = true;
                break;

            default:
                break;
        }
    }


}

