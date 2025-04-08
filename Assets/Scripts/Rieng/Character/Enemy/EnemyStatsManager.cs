using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatsManager : CharacterStatsManager
{
    EnemyManager enemy;
    public UIEnemyHealthBar enemyHealthBar;

    public bool isBoss;

    protected override void Awake()
    {
        base.Awake();
        enemy = GetComponent<EnemyManager>();
        maxHealth = SetMaxHealthFromHealthLevel();
        currentHealth = maxHealth;
    }

    void Start()
    {
        if (!isBoss)
        {
            enemyHealthBar.SetMaxHealth(maxHealth);
        }
    }

    private int SetMaxHealthFromHealthLevel()
    {
        maxHealth = healthLevel * 10;
        return maxHealth;
    }

    public override void TakeDamageNoAnimation(int physicalDamage, int fireDamage)
    {
        base.TakeDamageNoAnimation(physicalDamage, fireDamage);

        if (!isBoss)
        {
            enemyHealthBar.SetHealth(currentHealth);
        }
        else if (isBoss && enemy.enemyBossManager != null)
        {
            enemy.enemyBossManager.UpdateBossHealthBar(currentHealth, maxHealth);
        }
    }

    public void BreakGuard()
    {
        enemy.enemyAnimatorManager.PlayTargetAnimation("Break_Guard", true);
    }

    public override void TakeDamage(int physicalDamage, int fireDamage, string damageAnimation)
    {

        base.TakeDamage(physicalDamage, fireDamage, damageAnimation);

        if (!isBoss)
        {
            enemyHealthBar.SetHealth(currentHealth);
        }
        else if (isBoss && enemy.enemyBossManager != null)
        {
            enemy.enemyBossManager.UpdateBossHealthBar(currentHealth, maxHealth);
        }

        enemy.enemyAnimatorManager.PlayTargetAnimation(damageAnimation, true);

        if (currentHealth <= 0)
        {
            Debug.Log("Enemy is dead");
            HandleDead();
        }
    }

    public override void TakePoisonDamage(int damage)
    {
        if (enemy.isDead)
            return;

        base.TakePoisonDamage(damage);

        if (!isBoss)
        {
            enemyHealthBar.SetHealth(currentHealth);
        }
        else if (isBoss && enemy.enemyBossManager != null)
        {
            enemy.enemyBossManager.UpdateBossHealthBar(currentHealth, maxHealth);
        }

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            enemy.isDead = true;
            enemy.enemyAnimatorManager.PlayTargetAnimation("Death_01", true);
        }
    }

    private void HandleDead()
    {
        currentHealth = 0;
        enemy.enemyAnimatorManager.PlayTargetAnimation("Death_01", true);
        enemy.isDead = true;

    }

}
