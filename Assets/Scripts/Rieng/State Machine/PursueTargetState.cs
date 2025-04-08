using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PursueTargetState : State
{
    public CombatStanceState combatStanceState;
    public RotateTowardsTargetState rotateTowardsTargetState;

    public override State Tick(EnemyManager enemy)
    {
        Vector3 targetDirection = enemy.currentTarget.transform.position - enemy.transform.position;
        float distanceFromTarget = Vector3.Distance(enemy.currentTarget.transform.position, enemy.transform.position);
        float viewableAngle = Vector3.SignedAngle(targetDirection, enemy.transform.forward, Vector3.up);

        HandleRotateTowardsTarget(enemy);

        if (enemy.isInteracting)
            return this;

        if (enemy.isPerformingAction)
        {
            enemy.animator.SetFloat("Vertical", 0, 0.1f, Time.deltaTime);
            return this;
        }


        if (distanceFromTarget > enemy.maximumAggroRadius)
        {
            enemy.animator.SetFloat("Vertical", 1, 0.1f, Time.deltaTime);
        }


        if (distanceFromTarget <= enemy.maximumAggroRadius)
        {
            return combatStanceState;
        }
        else
        {
            return this;

        }
    }

    private void HandleRotateTowardsTarget(EnemyManager enemyManager)
    {
        if (enemyManager.isPerformingAction)
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
        else
        {
            Vector3 relativeDirection = transform.InverseTransformDirection(enemyManager.navMeshAgent.desiredVelocity);
            Vector3 targetVelocity = enemyManager.enemyRigidbody.velocity;

            enemyManager.navMeshAgent.enabled = true;
            enemyManager.navMeshAgent.SetDestination(enemyManager.currentTarget.transform.position);
            enemyManager.enemyRigidbody.velocity = targetVelocity;
            enemyManager.transform.rotation = Quaternion.Slerp(enemyManager.transform.rotation, enemyManager.navMeshAgent.transform.rotation, enemyManager.rotationSpeed / Time.deltaTime);
        }
    }
}
