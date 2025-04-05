using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatsManager : CharacterStatsManager
{
    PlayerManager playerManager;
    public HealthBar healthBar;
    StaminaBar staminaBar;
    FocusPointBar focusPointBar;
    PlayerAnimatorManager playerAnimatorManager;

    public float staminaRegenerationAmount = 1;
    public float staminaRegenTimer = 0;

    protected override void Awake()
    {
        base.Awake();
        playerManager = GetComponent<PlayerManager>();
        staminaBar = FindObjectOfType<StaminaBar>();
        focusPointBar = FindObjectOfType<FocusPointBar>();
        playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
    }

    void Start()
    {
        maxHealth = SetMaxHealthFromHealthLevel();
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        healthBar.SetCurrentHealth(currentHealth);

        maxStamina = SetMaxStaminaFromStaminaLevel();
        currentStamina = maxStamina;
        staminaBar.SetMaxStamina(maxStamina);
        staminaBar.SetCurrentStamina(currentStamina);

        maxFocusPoint = SetMaxFocusPointFromFocusLevel();
        currentFocusPoint = maxFocusPoint;
        focusPointBar.SetMaxFocusPoint(maxFocusPoint);
        focusPointBar.SetCurrentFocusPoint(currentFocusPoint);

    }

    public override void HandlePoiseResetTimer()
    {
        if (poiseResetTimer > 0)
        {
            poiseResetTimer = poiseResetTimer - Time.deltaTime;
        }
        else if (poiseResetTimer <= 0 && !playerManager.isInteracting)
        {
            totalPoiseDefence = armorPoiseBonus;
        }
    }

    private int SetMaxHealthFromHealthLevel()
    {
        maxHealth = healthLevel * 10;
        return maxHealth;
    }

    private float SetMaxStaminaFromStaminaLevel()
    {
        maxStamina = staminaLevel * 10;
        return maxStamina;
    }

    private float SetMaxFocusPointFromFocusLevel()
    {
        maxFocusPoint = focusLevel * 10;
        return maxFocusPoint;
    }

    public override void TakeDamage(int physicalDamage, int fireDamage, string damageAnimation)
    {
        if (playerManager.isInvulnerable)
            return;

        base.TakeDamage(physicalDamage, fireDamage, damageAnimation);
        healthBar.SetCurrentHealth(currentHealth);
        playerAnimatorManager.PlayTargetAnimation(damageAnimation, true);

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            isDead = true;
            playerAnimatorManager.PlayTargetAnimation("Death_01", true);
        }
    }

    public override void TakePoisonDamage(int damage)
    {
        if (isDead)
            return;

        base.TakePoisonDamage(damage);
        healthBar.SetCurrentHealth(currentHealth);

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            isDead = true;
            playerAnimatorManager.PlayTargetAnimation("Death_01", true);
        }
    }

    public override void TakeDamageNoAnimation(int physicalDamage, int fireDamage)
    {
        base.TakeDamageNoAnimation(physicalDamage, fireDamage);
        healthBar.SetCurrentHealth(currentHealth);
    }

    public void TakeStaminaDamage(int damage)
    {
        currentStamina = currentStamina - damage;
        staminaBar.SetCurrentStamina(currentStamina);
    }

    public void RegenerateStanima()
    {
        if (playerManager.isInteracting)
        {
            staminaRegenTimer = 0;
        }
        else
        {

            staminaRegenTimer += Time.deltaTime;
            if (currentStamina < maxStamina && staminaRegenTimer > 1f)
            {
                currentStamina += staminaRegenerationAmount * Time.deltaTime;
                staminaBar.SetCurrentStamina(Mathf.RoundToInt(currentStamina));
            }
        }
    }

    public void HealPlayer(int healAmount)
    {
        currentHealth = currentHealth + healAmount;

        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        healthBar.SetCurrentHealth(currentHealth);
    }

    public void DeductFocusPoint(int focusPoint)
    {
        currentFocusPoint = currentFocusPoint - focusPoint;

        if (currentFocusPoint < 0)
        {
            currentFocusPoint = 0;
        }

        focusPointBar.SetCurrentFocusPoint(currentFocusPoint);
    }

    public void AddShades(int shades)
    {
        shadeCount += shades;
    }

}