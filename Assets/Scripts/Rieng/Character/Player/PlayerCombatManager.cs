using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatManager : MonoBehaviour
{
    PlayerCamera playerCamera;
    PlayerAnimatorManager playerAnimatorManager;
    PlayerEquipmentManager playerEquipmentManager;
    PlayerManager playerManager;
    PlayerStatsManager playerStatsManager;
    PlayerInventoryManager playerInventoryManager;
    PlayerInputManager playerInputManager;
    PlayerWeaponSlotManager playerWeaponSlotManager;
    PlayerEffectsManager playerEffectsManager;

    [Header("Attack Animations")]
    string oh_light_attack_01 = "OH_Light_Attack_01";
    string oh_light_attack_02 = "OH_Light_Attack_02";

    string th_light_attack_01 = "TH_Light_Attack_01";
    string th_light_attack_02 = "TH_Light_Attack_02";
    string th_light_attack_03 = "TH_Light_Attack_03";

    string oh_heavy_attack_01 = "OH_Heavy_Attack_01";

    string weaponArt = "Weapon Art";

    public string lastAttack;

    LayerMask backStabLayer = 1 << 12;
    LayerMask riposteLayer = 1 << 13;

    private void Awake()
    {
        playerCamera = FindObjectOfType<PlayerCamera>();
        playerInputManager = GetComponent<PlayerInputManager>();
        playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
        playerEquipmentManager = GetComponent<PlayerEquipmentManager>();
        playerManager = GetComponent<PlayerManager>();
        playerStatsManager = GetComponent<PlayerStatsManager>();
        playerInventoryManager = GetComponent<PlayerInventoryManager>();
        playerWeaponSlotManager = GetComponent<PlayerWeaponSlotManager>();
        playerEffectsManager = GetComponent<PlayerEffectsManager>();
    }

    public void HandleRBAction()
    {
        playerAnimatorManager.EraseHandIKForWeapon();
        if (playerInventoryManager.rightWeapon.weaponType == WeaponType.StraightSword
        || playerInventoryManager.rightWeapon.weaponType == WeaponType.Unarmed)
        {
            PerformRBMeleeAction();
        }
        else if (playerInventoryManager.rightWeapon.weaponType == WeaponType.SpellCaster
        || playerInventoryManager.rightWeapon.weaponType == WeaponType.FaithCaster)
        {
            PerformRBSpellAction(playerInventoryManager.rightWeapon);
        }
    }

    public void HandleLBAction()
    {
        PerformLBBlockingAction();
    }

    public void HandleLTAction()
    {
        if (playerInventoryManager.leftWeapon.weaponType == WeaponType.Shield
        || playerInventoryManager.rightWeapon.weaponType == WeaponType.Unarmed)
        {
            PerformLTWeaponArt(playerInputManager.twoHandFlag);
        }
        else if (playerInventoryManager.leftWeapon.weaponType == WeaponType.StraightSword)
        {
            //PerformMeleeAction();
        }
    }


    public void HandleWeaponCombo(WeaponItem weapon)
    {
        if (playerStatsManager.currentStamina <= 0)
            return;

        if (playerInputManager.comboFlag)
        {
            playerAnimatorManager.animator.SetBool("canDoCombo", false);

            if (lastAttack == oh_light_attack_01)
            {
                playerAnimatorManager.PlayTargetAnimation(oh_light_attack_02, true);
            }

            if (lastAttack == th_light_attack_01)
            {
                playerAnimatorManager.PlayTargetAnimation(th_light_attack_02, true);
                lastAttack = th_light_attack_02;
            }
            else if (lastAttack == th_light_attack_02)
            {
                playerAnimatorManager.PlayTargetAnimation(th_light_attack_03, true);
            }
        }
    }

    public void HandleLightAttack(WeaponItem weapon)
    {
        if (playerStatsManager.currentStamina <= 0)
            return;

        playerWeaponSlotManager.attackingWeapon = weapon;

        if (playerInputManager.twoHandFlag)
        {
            playerAnimatorManager.PlayTargetAnimation(th_light_attack_01, true);
            lastAttack = th_light_attack_01;
        }
        else
        {
            playerAnimatorManager.PlayTargetAnimation(oh_light_attack_01, true);
            lastAttack = oh_light_attack_01;
        }
    }

    public void HandleHeavyAttack(WeaponItem weapon)
    {
        if (playerStatsManager.currentStamina <= 0)
            return;

        playerWeaponSlotManager.attackingWeapon = weapon;

        if (playerInputManager.twoHandFlag)
        {

        }
        else
        {
            playerAnimatorManager.PlayTargetAnimation(oh_heavy_attack_01, true);
            lastAttack = oh_heavy_attack_01;
        }


    }


    private void PerformRBMeleeAction()
    {
        if (playerManager.canDoCombo)
        {
            playerInputManager.comboFlag = true;
            HandleWeaponCombo(playerInventoryManager.rightWeapon);
            playerInputManager.comboFlag = false;
        }
        else
        {
            if (playerManager.isInteracting)
                return;
            if (playerManager.canDoCombo)
                return;

            playerAnimatorManager.animator.SetBool("isUsingRightHand", true);
            HandleLightAttack(playerInventoryManager.rightWeapon);
        }

        playerEffectsManager.PlayWeaponEffect(false);
    }

    private void PerformRBSpellAction(WeaponItem weapon)
    {
        if (playerManager.isInteracting)
            return;

        if (weapon.weaponType == WeaponType.FaithCaster)
        {
            if (playerInventoryManager.currentSpell != null && playerInventoryManager.currentSpell.isFaithSpell)
            {
                if (playerStatsManager.currentFocusPoint >= playerInventoryManager.currentSpell.focusPointCost)
                {
                    playerInventoryManager.currentSpell.AttemptToCastSpell(playerAnimatorManager, playerStatsManager);
                }
                else
                {
                    playerAnimatorManager.PlayTargetAnimation("Shrug", true);
                }
            }
        }
    }

    private void PerformLTWeaponArt(bool isTwoHanding)
    {
        if (playerManager.isInteracting)
            return;

        if (isTwoHanding)
        {

        }
        else
        {
            playerAnimatorManager.PlayTargetAnimation(weaponArt, true);

        }
    }

    private void PerformLBBlockingAction()
    {
        if (playerManager.isInteracting)
            return;

        if (playerManager.isBlocking)
            return;

        playerAnimatorManager.PlayTargetAnimation("Block_Start", false, true);
        playerEquipmentManager.OpenBlockingCollider();
        playerManager.isBlocking = true;
    }


    private void SuccessfullyCastSpell()
    {
        playerInventoryManager.currentSpell.SuccessfullyCastSpell(playerAnimatorManager, playerStatsManager);
    }

    public void AttempBackStabOrRiposte()
    {
        if (playerStatsManager.currentStamina <= 0)
            return;

        RaycastHit hit;

        if (Physics.Raycast(playerInputManager.criticalAttackRayCastStartPoint.position,
         transform.TransformDirection(Vector3.forward), out hit, 0.5f, backStabLayer))
        {
            CharacterManager enemyCharacterManager = hit.transform.gameObject.GetComponentInParent<CharacterManager>();
            DamageCollider rightWeapon = playerWeaponSlotManager.rightHandDamageCollider;

            if (enemyCharacterManager != null)
            {
                playerManager.transform.position = enemyCharacterManager.backStabCollider.criticalDamageStandPoint.position;

                Vector3 rotationDirection = playerManager.transform.root.eulerAngles;
                rotationDirection = hit.transform.position - playerManager.transform.position;
                rotationDirection.y = 0;
                rotationDirection.Normalize();
                Quaternion tr = Quaternion.LookRotation(rotationDirection);
                Quaternion targetRotation = Quaternion.Slerp(playerManager.transform.rotation, tr, 500 * Time.deltaTime);
                playerManager.transform.rotation = targetRotation;

                int criticalDamage = playerInventoryManager.rightWeapon.criticalDamageMultiplier * rightWeapon.physicalDamage;
                enemyCharacterManager.pendingCriticalDamage = criticalDamage;

                playerAnimatorManager.PlayTargetAnimation("Back Stab", true);
                enemyCharacterManager.GetComponentInChildren<CharacterAnimatorManager>().PlayTargetAnimation("Back Stabbed", true);
            }
        }
        else if (Physics.Raycast(playerInputManager.criticalAttackRayCastStartPoint.position,
         transform.TransformDirection(Vector3.forward), out hit, 0.5f, riposteLayer))
        {
            CharacterManager enemyCharacterManager = hit.transform.gameObject.GetComponentInParent<CharacterManager>();
            DamageCollider rightWeapon = playerWeaponSlotManager.rightHandDamageCollider;

            if (enemyCharacterManager != null && enemyCharacterManager.canBeRiposted)
            {
                playerManager.transform.position = enemyCharacterManager.riposteCollider.criticalDamageStandPoint.position;

                Vector3 rotationDirection = playerManager.transform.root.eulerAngles;
                rotationDirection = hit.transform.position - playerManager.transform.position;
                rotationDirection.y = 0;
                rotationDirection.Normalize();
                Quaternion tr = Quaternion.LookRotation(rotationDirection);
                Quaternion targetRotation = Quaternion.Slerp(playerManager.transform.rotation, tr, 500 * Time.deltaTime);
                playerManager.transform.rotation = targetRotation;

                int criticalDamage = playerInventoryManager.rightWeapon.criticalDamageMultiplier * rightWeapon.physicalDamage;
                enemyCharacterManager.pendingCriticalDamage = criticalDamage;

                playerAnimatorManager.PlayTargetAnimation("Riposte", true);
                enemyCharacterManager.GetComponentInChildren<CharacterAnimatorManager>().PlayTargetAnimation("Riposted", true);
            }
        }
    }

}
