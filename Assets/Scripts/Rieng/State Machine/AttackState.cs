using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : State
{
    public RotateTowardsTargetState rotateTowardsTargetState;
    public CombatStanceState combatStanceState;
    public PursueTargetState pursueTargetState;
    public EnemyAttackAction currentAttack;

    bool willDoComBoOnNextAttack = false;
    public bool hasPerformedAttack = false;

    public override State Tick(EnemyManager enemy)
    {
        float distanceFromTarget = Vector3.Distance(enemy.currentTarget.transform.position, enemy.transform.position);

        RotateTowardsTargetWhilsAttacking(enemy);

        if (distanceFromTarget > enemy.maximumAggroRadius)
        {
            return pursueTargetState;
        }

        if (willDoComBoOnNextAttack && enemy.canDoCombo)
        {
            AttackTargetWithCombo(enemy);
        }

        if (!hasPerformedAttack)
        {
            AttackTarget(enemy);
            RollForComboChance(enemy);
        }

        if (willDoComBoOnNextAttack && hasPerformedAttack)
        {
            return this;
        }

        return rotateTowardsTargetState;
    }

    private void AttackTarget(EnemyManager enemy)
    {
        enemy.animator.SetBool("isUsingRightHand", currentAttack.isRightHandedAction);
        enemy.animator.SetBool("isUsingLeftHand", !currentAttack.isRightHandedAction);
        enemy.enemyAnimatorManager.PlayTargetAnimation(currentAttack.actionAnimation, true);
        enemy.currentRecoveryTime = currentAttack.recoveryTime;
        hasPerformedAttack = true;
    }

    private void AttackTargetWithCombo(EnemyManager enemy)
    {
        enemy.animator.SetBool("isUsingRightHand", currentAttack.isRightHandedAction);
        enemy.animator.SetBool("isUsingLeftHand", !currentAttack.isRightHandedAction);
        willDoComBoOnNextAttack = false;
        enemy.enemyAnimatorManager.PlayTargetAnimation(currentAttack.actionAnimation, true);
        enemy.currentRecoveryTime = currentAttack.recoveryTime;
        currentAttack = null;
    }

    private void RotateTowardsTargetWhilsAttacking(EnemyManager enemyManager)
    {
        if (enemyManager.canRotate && enemyManager.isInteracting)
        {
            Vector3 direction = enemyManager.currentTarget.transform.position - enemyManager.transform.position;
            direction.y = 0;
            direction.Normalize();

            if (direction == Vector3.zero)
            {
                direction = transform.position;
            }

            Quaternion targetRotation = Quaternion.LookRotation(direction);
            enemyManager.transform.rotation = Quaternion.Slerp(enemyManager.transform.rotation, targetRotation, enemyManager.rotationSpeed / Time.deltaTime);
        }
    }

    private void RollForComboChance(EnemyManager enemyManager)
    {
        float comboChance = Random.Range(0, 100);

        if (enemyManager.allowAIToPerformCombos && comboChance <= enemyManager.comboLikelyHood)
        {
            if (currentAttack.comboAction != null)
            {
                willDoComBoOnNextAttack = true;
                currentAttack = currentAttack.comboAction;
            }
            else
            {
                willDoComBoOnNextAttack = false;
                currentAttack = null;
            }
        }

    }

}
