using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public int healthLevel = 10;
    public int maxHealth;
    public int currentHealth;

    public HealthBar healthBar;


    PlayerAnimatorManager playerAnimatorManager;

    private void Awake()
    {
        playerAnimatorManager = GetComponentInChildren<PlayerAnimatorManager>();
    }

    void Start()
    {
        maxHealth = SetMaxHealthFromHealthLevel();
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    private int SetMaxHealthFromHealthLevel()
    {
        maxHealth = healthLevel * 10;
        return maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth = currentHealth - damage;

        healthBar.SetCurrentHealth(currentHealth);

        playerAnimatorManager.PlayTargetAnimation("Damage_01", true);

        if(currentHealth <= 0)
        {
            currentHealth = 0;
            playerAnimatorManager.PlayTargetAnimation("Death_01", true);
            //handle dead
        }
    }
}
