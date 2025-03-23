using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimatorManager : CharacterAnimatorManager
{
    EnemyManager enemyManager;
    EnemyStats enemyStats;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        enemyManager = GetComponentInParent<EnemyManager>();
        enemyStats = GetComponentInParent<EnemyStats>();
    }

    public void CanRotate()
    {
        anim.SetBool("canRotate", true);
    }

    public void StopRotation()
    {
        anim.SetBool("canRotate", false);
    }

    public void EnableCombo()
    {
        anim.SetBool("canDoCombo", true);
    }

    public void DisableCombo()
    {
        anim.SetBool("canDoCombo", false);
    }
    
    public void EnableIsParrying()
    {
        enemyManager.isParrying = true;
    }
    
    public void DisableIsParrying()
    {
        enemyManager.isParrying = false;
    }

    public void EnableCanBeRiposted()
    {
        enemyManager.canBeRiposted = true;
    }

    public void DisableCanBeRiposted()
    {
        enemyManager.canBeRiposted = false;
    }

    public override void TakeCriticalDamageAnimationEvent()
    {
        enemyStats.TakeDamageNoAnimation(enemyManager.pendingCriticalDamage);
        enemyManager.pendingCriticalDamage = 0;
    }

    public void AwardShadesOnDeath()
    {
        PlayerStats playerStats = FindObjectOfType<PlayerStats>();
        ShadeCountBar shadeCountBar = FindObjectOfType<ShadeCountBar>();

        if (playerStats != null)
        {
            playerStats.AddShades(enemyStats.shadesAwardedOnDeath);

            if (shadeCountBar != null)
            {
                shadeCountBar.SetShadeCountText(playerStats.shadeCount);
            }
        }


    }

    private void OnAnimatorMove()
    {
        float delta = Time.deltaTime;
        enemyManager.enemyRigidbody.drag = 0;
        Vector3 deltaPosition = anim.deltaPosition;
        deltaPosition.y = 0;
        Vector3 velocity = deltaPosition / delta;
        enemyManager.enemyRigidbody.velocity = velocity;

        if(enemyManager.isRotatingWithRootMotion)
        {
            enemyManager.transform.rotation *= anim.deltaRotation;
        }
    }

}
