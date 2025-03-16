using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyManager : CharacterManager
{
    EnemyLocomotionManager enemyLocomotionManager;
    EnemyAnimatorManager enemyAnimatorManager;
    EnemyStats enemyStats;


    public NavMeshAgent navMeshAgent;
    public State currentState;
    public CharacterStats currentTarget;
    public Rigidbody enemyRigidbody;


    public bool isPerformingAction;
    public bool isInteracting;
    public float rotationSpeed = 15;
    public float maximumAttackRange = 1.5f;

    [Header("AI Settings")]
    public float detectionRadius = 20;
    public float maximumDetectionAngle = 50;
    public float minimumDetectionAngle = -50;

    public float currentRecoveryTime = 0;

    private void Awake()
    {
        enemyLocomotionManager = GetComponent<EnemyLocomotionManager>();
        enemyAnimatorManager = GetComponentInChildren<EnemyAnimatorManager>();
        enemyStats = GetComponent<EnemyStats>();
        enemyRigidbody = GetComponent<Rigidbody>();
        backStabCollider = GetComponentInChildren<BackStabCollider>();
        navMeshAgent = GetComponentInChildren<NavMeshAgent>();
        navMeshAgent.enabled = false;
    }

    private void Start()
    {
        enemyRigidbody.isKinematic = false;
    }

    private void Update()
    {
        handleRecoveryTimer();

        isInteracting = enemyAnimatorManager.anim.GetBool("isInteracting");
    }

    private void FixedUpdate()
    {
        HandleStateMachine();
    }

    private void HandleStateMachine()
    {
        if (currentState != null)
        {
            State nextState = currentState.Tick(this, enemyStats, enemyAnimatorManager);

            if (nextState != null)
            {
                SwitchToNextState(nextState);
            }
        }
    }

    private void SwitchToNextState(State state)
    {
        currentState = state;
    }

    private void handleRecoveryTimer()
    {
        if (currentRecoveryTime > 0)
        {
            currentRecoveryTime -= Time.deltaTime;
        }
        if (isPerformingAction)
        {
            if (currentRecoveryTime <= 0)
            {
                isPerformingAction = false;
            }
        }
    }

    #region Attacks
    private void AttackTarget()
    {
        // if (isPerformingAction)
        //     return;

        // if (currentAttack == null)
        // {
        //     GetNewAttack();
        // }
        // else
        // {
        //     isPerformingAction = true;
        //     currentRecoveryTime = currentAttack.recoveryTime;
        //     enemyAnimatorManager.PlayTargetAnimation(currentAttack.actionAnimation, true);
        //     currentAttack = null;
        // }
    }
    private void GetNewAttack()
    {
        // Vector3 targetsDirection = enemyLocomotionManager.currentTarget.transform.position - transform.position;
        // float viewableAngle = Vector3.Angle(targetsDirection, transform.position);
        // enemyLocomotionManager.distanceFromTarget = Vector3.Distance(enemyLocomotionManager.currentTarget.transform.position, transform.position);

        // int maxScore = 0;
        // for (int i = 0; i < enemyAttacks.Length; i++)
        // {
        //     EnemyAttackAction enemyAttackAction = enemyAttacks[i];

        //     if (enemyLocomotionManager.distanceFromTarget <= enemyAttackAction.maximumDistanceNeededToAttack
        //     && enemyLocomotionManager.distanceFromTarget >= enemyAttackAction.minimumDistanceNeededToAttack)
        //     {
        //         if (viewableAngle <= enemyAttackAction.maximumAttackAngle
        //         && viewableAngle >= enemyAttackAction.minimumAttackAngle)
        //         {
        //             maxScore += enemyAttackAction.attackScore;
        //         }
        //     }
        // }

        // int randomValue = Random.Range(0, maxScore);
        // int temporaryScore = 0;

        // for (int i = 0; i < enemyAttacks.Length; i++)
        // {
        //     EnemyAttackAction enemyAttackAction = enemyAttacks[i];

        //     if (enemyLocomotionManager.distanceFromTarget <= enemyAttackAction.maximumDistanceNeededToAttack
        //     && enemyLocomotionManager.distanceFromTarget >= enemyAttackAction.minimumDistanceNeededToAttack)
        //     {
        //         if (viewableAngle <= enemyAttackAction.maximumAttackAngle
        //         && viewableAngle >= enemyAttackAction.minimumAttackAngle)
        //         {
        //             if (currentAttack != null)
        //                 return;

        //             temporaryScore += enemyAttackAction.attackScore;

        //             if (temporaryScore > randomValue)
        //             {
        //                 currentAttack = enemyAttackAction;
        //             }
        //         }
        //     }
        // }
    }
    #endregion
}
