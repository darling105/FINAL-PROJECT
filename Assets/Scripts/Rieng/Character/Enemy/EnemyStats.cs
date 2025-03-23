using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : CharacterStats
{
    EnemyAnimatorManager enemyAnimatorManager;
    EnemyBossManager enemyBossManager;
    public UIEnemyHealthBar enemyHealthBar;

    public int shadesAwardedOnDeath = 100;

    public bool isBoss;

    private void Awake()
    {
        maxHealth = SetMaxHealthFromHealthLevel();
        currentHealth = maxHealth;
        enemyAnimatorManager = GetComponentInChildren<EnemyAnimatorManager>();
        enemyBossManager = GetComponent<EnemyBossManager>();
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

    public void TakeDamageNoAnimation(int damage)
    {

        currentHealth = currentHealth - damage;
        enemyHealthBar.SetHealth(currentHealth);

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            isDead = true;
        }
    }

    public override void TakeDamage(int damage, string damageAnimation = "Damage_01")
    {
        base.TakeDamage(damage, damageAnimation = "Damage_01");

        if (!isBoss)
        {
            enemyHealthBar.SetHealth(currentHealth);
        }
        else if (isBoss && enemyBossManager != null)
        {
            enemyBossManager.UpdateBossHealthBar(currentHealth);
        }
        enemyAnimatorManager.PlayTargetAnimation(damageAnimation, true);

        if (currentHealth <= 0)
        {
            HandleDead();
        }
    }

    private void HandleDead()
    {
        currentHealth = 0;
        enemyAnimatorManager.PlayTargetAnimation("Death_01", true);
        isDead = true;
    }

}
