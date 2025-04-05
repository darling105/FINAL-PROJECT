using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyManager : CharacterManager
{
    EnemyLocomotionManager enemyLocomotionManager;
    EnemyAnimatorManager enemyAnimatorManager;
    EnemyStatsManager enemyStatsManager;
    EnemyEffectsManager enemyEffectsManager;


    public NavMeshAgent navMeshAgent;
    public State currentState;
    public CharacterStatsManager currentTarget;
    public Rigidbody enemyRigidbody;


    public bool isPerformingAction;
    public float rotationSpeed = 15;
    public float maximumAggroRadius = 1.5f;

    [Header("AI Settings")]
    public float detectionRadius = 20;
    public float maximumDetectionAngle = 50;
    public float minimumDetectionAngle = -50;
    public float currentRecoveryTime = 0;

    [Header("AI Combat Settings")]
    public bool allowAIToPerformCombos;
    public bool isPhaseShifting;
    public float comboLikelyHood;

    protected override void Awake()
    {
        base.Awake();
        enemyLocomotionManager = GetComponent<EnemyLocomotionManager>();
        enemyAnimatorManager = GetComponent<EnemyAnimatorManager>();
        enemyStatsManager = GetComponent<EnemyStatsManager>();
        enemyEffectsManager = GetComponent<EnemyEffectsManager>();
        enemyRigidbody = GetComponent<Rigidbody>();
        navMeshAgent = GetComponentInChildren<NavMeshAgent>();
        navMeshAgent.enabled = false;
    }

    private void Start()
    {
        enemyRigidbody.isKinematic = false;
    }

    private void Update()
    {
        HandleRecoveryTimer();
        HandleStateMachine();

        isRotatingWithRootMotion = enemyAnimatorManager.animator.GetBool("isRotatingWithRootMotion");
        isInteracting = enemyAnimatorManager.animator.GetBool("isInteracting");
        isPhaseShifting = enemyAnimatorManager.animator.GetBool("isPhaseShifting");
        isInvulnerable = enemyAnimatorManager.animator.GetBool("isInvulnerable");
        canDoCombo = enemyAnimatorManager.animator.GetBool("canDoCombo");
        canRotate = enemyAnimatorManager.animator.GetBool("canRotate");
        enemyAnimatorManager.animator.SetBool("isDead", enemyStatsManager.isDead);
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        enemyEffectsManager.HandleAllBuildUpEffects();
    }

    private void LateUpdate()
    {
        navMeshAgent.transform.localPosition = Vector3.zero;
        navMeshAgent.transform.localRotation = Quaternion.identity;
    }

    private void HandleStateMachine()
    {
        if (currentState != null)
        {
            State nextState = currentState.Tick(this, enemyStatsManager, enemyAnimatorManager);

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

    private void HandleRecoveryTimer()
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
