using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimatorManager : CharacterAnimatorManager
{
    EnemyManager enemy;

    protected override void Awake()
    {
        base.Awake();
        enemy = GetComponent<EnemyManager>();
    }

    public void AwardShadesOnDeath()
    {
        PlayerStatsManager playerStats = FindObjectOfType<PlayerStatsManager>();
        ShadeCountBar shadeCountBar = FindObjectOfType<ShadeCountBar>();

        if (playerStats != null)
        {
            playerStats.AddShades(enemy.enemyStatsManager.shadesAwardedOnDeath);

            if (shadeCountBar != null)
            {
                shadeCountBar.SetShadeCountText(playerStats.currentShadesCount);
            }
        }
    }

    public void InstantiateBossParticleFX()
    {
        BossFXTransform bossFXTransform = GetComponentInChildren<BossFXTransform>();
        GameObject phaseFX = Instantiate(enemy.enemyBossManager.particleFX, bossFXTransform.transform);
    }

    private void OnAnimatorMove()
    {
        float delta = Time.deltaTime;
        enemy.enemyRigidbody.drag = 0;
        Vector3 deltaPosition = enemy.animator.deltaPosition;
        deltaPosition.y = 0;
        Vector3 velocity = deltaPosition / delta;
        enemy.enemyRigidbody.velocity = velocity;

        if (enemy.isRotatingWithRootMotion)
        {
            enemy.transform.rotation *= enemy.animator.deltaRotation;
        }
    }

}
