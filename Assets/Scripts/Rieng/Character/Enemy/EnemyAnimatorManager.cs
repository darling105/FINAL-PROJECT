using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimatorManager : CharacterAnimatorManager
{
    EnemyManager enemyManager;
    EnemyBossManager enemyBossManager;

    protected override void Awake()
    {
        base.Awake();
        animator = GetComponent<Animator>();
        enemyManager = GetComponent<EnemyManager>();
        enemyBossManager = GetComponent<EnemyBossManager>();
    }

    public void AwardShadesOnDeath()
    {
        PlayerStatsManager playerStats = FindObjectOfType<PlayerStatsManager>();
        ShadeCountBar shadeCountBar = FindObjectOfType<ShadeCountBar>();

        if (playerStats != null)
        {
            playerStats.AddShades(characterStatsManager.shadesAwardedOnDeath);

            if (shadeCountBar != null)
            {
                shadeCountBar.SetShadeCountText(playerStats.shadeCount);
            }
        }
    }

    public void InstantiateBossParticleFX()
    {
        BossFXTransform bossFXTransform = GetComponentInChildren<BossFXTransform>();
        GameObject phaseFX = Instantiate(enemyBossManager.particleFX, bossFXTransform.transform);
    }

    private void OnAnimatorMove()
    {
        float delta = Time.deltaTime;
        enemyManager.enemyRigidbody.drag = 0;
        Vector3 deltaPosition = animator.deltaPosition;
        deltaPosition.y = 0;
        Vector3 velocity = deltaPosition / delta;
        enemyManager.enemyRigidbody.velocity = velocity;

        if (enemyManager.isRotatingWithRootMotion)
        {
            enemyManager.transform.rotation *= animator.deltaRotation;
        }
    }

}
