using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBossManager : MonoBehaviour
{
    EnemyManager enemy;
    public string bossName;
    UIBossHealthBar bossHealthBar;
    BossCombatStanceState bossCombatStanceState;

    [Header("Second Phase FX")]
    public GameObject particleFX;

    private void Awake()
    {
        enemy = GetComponent<EnemyManager>();
        bossHealthBar = FindObjectOfType<UIBossHealthBar>();
        bossCombatStanceState = GetComponentInChildren<BossCombatStanceState>();
    }

    private void Start()
    {
        bossHealthBar.SetBossName(bossName);
        bossHealthBar.SetBossMaxHealth(enemy.enemyStatsManager.maxHealth);
    }

    public void UpdateBossHealthBar(int currentHealth, int maxHealth)
    {
        bossHealthBar.SetBossCurrentHealth(currentHealth);
        if (currentHealth <= maxHealth / 2 && !bossCombatStanceState.hasPhaseShifted)
        {
            bossCombatStanceState.hasPhaseShifted = true;
            ShiftToSecondPhase();
        }
    }

    public void ShiftToSecondPhase()
    {
        enemy.animator.SetBool("isInvulnerable", true);
        enemy.animator.SetBool("isPhaseShifting", true);
        enemy.enemyAnimatorManager.PlayTargetAnimation("Phase_Shift", true);
        bossCombatStanceState.hasPhaseShifted = true;
    }

}
