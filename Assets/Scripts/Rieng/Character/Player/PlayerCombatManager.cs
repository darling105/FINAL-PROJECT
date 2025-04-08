using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatManager : MonoBehaviour
{
    PlayerManager player;

    [Header("Attack Animations")]
    public string oh_light_attack_01 = "OH_Light_Attack_01";
    public string oh_light_attack_02 = "OH_Light_Attack_02";

    public string th_light_attack_01 = "TH_Light_Attack_01";
    public string th_light_attack_02 = "TH_Light_Attack_02";
    public string th_light_attack_03 = "TH_Light_Attack_03";

    public string oh_heavy_attack_01 = "OH_Heavy_Attack_01";
    public string th_heavy_attack_01 = "TH_Heavy_Attack_01";

    public string weaponArt = "Weapon Art";

    public string lastAttack;

    LayerMask backStabLayer = 1 << 12;
    LayerMask riposteLayer = 1 << 13;

    private void Awake()
    {
        player = GetComponent<PlayerManager>();
    }

    private void SuccessfullyCastSpell()
    {
        player.playerInventoryManager.currentSpell.SuccessfullyCastSpell(player.playerAnimatorManager, player.playerStatsManager, player.playerWeaponSlotManager, player.isUsingLeftHand);
    }

    public void AttempBackStabOrRiposte()
    {
        if (player.playerStatsManager.currentStamina <= 0)
            return;

        RaycastHit hit;

        if (Physics.Raycast(player.playerInputManager.criticalAttackRayCastStartPoint.position,
         transform.TransformDirection(Vector3.forward), out hit, 0.5f, backStabLayer))
        {
            CharacterManager enemyCharacterManager = hit.transform.gameObject.GetComponentInParent<CharacterManager>();
            DamageCollider rightWeapon = player.playerWeaponSlotManager.rightHandDamageCollider;

            if (enemyCharacterManager != null)
            {
                player.transform.position = enemyCharacterManager.backStabCollider.criticalDamageStandPoint.position;

                Vector3 rotationDirection = player.transform.root.eulerAngles;
                rotationDirection = hit.transform.position - player.transform.position;
                rotationDirection.y = 0;
                rotationDirection.Normalize();
                Quaternion tr = Quaternion.LookRotation(rotationDirection);
                Quaternion targetRotation = Quaternion.Slerp(player.transform.rotation, tr, 500 * Time.deltaTime);
                player.transform.rotation = targetRotation;

                int criticalDamage = player.playerInventoryManager.rightWeapon.criticalDamageMultiplier * rightWeapon.physicalDamage;
                enemyCharacterManager.pendingCriticalDamage = criticalDamage;

                player.playerAnimatorManager.PlayTargetAnimation("Back Stab", true);
                enemyCharacterManager.GetComponentInChildren<CharacterAnimatorManager>().PlayTargetAnimation("Back Stabbed", true);
            }
        }
        else if (Physics.Raycast(player.playerInputManager.criticalAttackRayCastStartPoint.position,
         transform.TransformDirection(Vector3.forward), out hit, 0.5f, riposteLayer))
        {
            CharacterManager enemyCharacterManager = hit.transform.gameObject.GetComponentInParent<CharacterManager>();
            DamageCollider rightWeapon = player.playerWeaponSlotManager.rightHandDamageCollider;

            if (enemyCharacterManager != null && enemyCharacterManager.canBeRiposted)
            {
                player.transform.position = enemyCharacterManager.riposteCollider.criticalDamageStandPoint.position;

                Vector3 rotationDirection = player.transform.root.eulerAngles;
                rotationDirection = hit.transform.position - player.transform.position;
                rotationDirection.y = 0;
                rotationDirection.Normalize();
                Quaternion tr = Quaternion.LookRotation(rotationDirection);
                Quaternion targetRotation = Quaternion.Slerp(player.transform.rotation, tr, 500 * Time.deltaTime);
                player.transform.rotation = targetRotation;

                int criticalDamage = player.playerInventoryManager.rightWeapon.criticalDamageMultiplier * rightWeapon.physicalDamage;
                enemyCharacterManager.pendingCriticalDamage = criticalDamage;

                player.playerAnimatorManager.PlayTargetAnimation("Riposte", true);
                enemyCharacterManager.GetComponentInChildren<CharacterAnimatorManager>().PlayTargetAnimation("Riposted", true);
            }
        }
    }

}
