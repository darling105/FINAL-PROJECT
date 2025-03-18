using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class DamageCollider : MonoBehaviour
{
    public CharacterManager characterManager;
    Collider damageCollider;

    public int currentWeaponDamage = 25;

    private void Awake()
    {
        damageCollider = GetComponent<Collider>();
        damageCollider.gameObject.SetActive(true);
        damageCollider.isTrigger = true;
        damageCollider.enabled = false;
    }

    public void EnableDamageCollider()
    {
        damageCollider.enabled = true;
    }

    public void DisableDamageCollider()
    {
        damageCollider.enabled = false;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.tag == "Player")
        {
            PlayerStats playerStats = collision.GetComponent<PlayerStats>();
            CharacterManager enemyCharacterManager = collision.GetComponent<CharacterManager>();

            if(enemyCharacterManager != null)
            {
                if(enemyCharacterManager.isParrying)
                {
                    characterManager.GetComponentInChildren<CharacterAnimatorManager>().PlayTargetAnimation("Parried", true);
                }
            }

            if (playerStats != null)
            {
                playerStats.TakeDamage(currentWeaponDamage);
            }
        }
        if (collision.tag == "Enemy")
        {
            EnemyStats enemyStats = collision.GetComponent<EnemyStats>();
            CharacterManager enemyCharacterManager = collision.GetComponent<CharacterManager>();

            if(enemyCharacterManager != null)
            {
                if(enemyCharacterManager.isParrying)
                {
                    characterManager.GetComponentInChildren<CharacterAnimatorManager>().PlayTargetAnimation("Parried", true);
                }
            }

            if (enemyStats != null)
            {
                enemyStats.TakeDamage(currentWeaponDamage);
            }
        }
    }
}
