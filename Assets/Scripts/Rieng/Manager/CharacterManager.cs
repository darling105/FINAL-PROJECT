using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
   CharacterAnimatorManager characterAnimatorManager;
   CharacterWeaponSlotManager characterWeaponSlotManager;

   [Header("Lock On Transform")]
   public Transform lockOnTransform;

   [Header("Combat Colliders")]
   public CriticalDamageCollider backStabCollider;
   public CriticalDamageCollider riposteCollider;

   [Header("Combat Flags")]
   public bool canBeRiposted;
   public bool canBeParried;
   public bool canDoCombo;
   public bool isParrying;
   public bool isBlocking;
   public bool isInvulnerable;
   public bool isUsingRightHand;
   public bool isUsingLeftHand;
   public bool isTwoHandingWeapon;

   [Header("Movement Flags")]
   public bool isInteracting;
   public bool isRotatingWithRootMotion;
   public bool canRotate;
   public bool isSprinting;
   public bool isInAir;
   public bool isGrounded;

   public int pendingCriticalDamage;

   public string characterName = "Character";

   protected virtual void Awake()
   {
      characterAnimatorManager = GetComponent<CharacterAnimatorManager>();
      characterWeaponSlotManager = GetComponent<CharacterWeaponSlotManager>();
   }

   protected virtual void FixedUpdate()
   {
      characterAnimatorManager.CheckHandIKWeight(characterWeaponSlotManager.rightHandIKTarget, characterWeaponSlotManager.leftHandIKTarget, isTwoHandingWeapon);
   }

   public virtual void UpdateWhichHandCharacterIsUsing(bool usingRightHand)
   {
      if (usingRightHand)
      {
         isUsingLeftHand = false;
         isUsingRightHand = true;
      }
      else
      {
         isUsingLeftHand = true;
         isUsingRightHand = false;
      }
   }

}
