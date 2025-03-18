using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : CharacterStats
{
    Animator animator;
    EnemyAnimatorManager enemyAnimatorManager;

    public int shadesAwardedOnDeath = 100;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        enemyAnimatorManager = GetComponentInChildren<EnemyAnimatorManager>();
    }

    void Start()
    {
        maxHealth = SetMaxHealthFromHealthLevel();
        currentHealth = maxHealth;
    }

    private int SetMaxHealthFromHealthLevel()
    {
        maxHealth = healthLevel * 10;
        return maxHealth;
    }

    public void TakeDamageNoAnimation(int damage)
    {

        currentHealth = currentHealth - damage;

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            isDead = true;
        }
    }

    public void TakeDamage(int damage, string damageAnimation = "Damage_01")
    {
        if (isDead)
            return;

        currentHealth = currentHealth - damage;

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
