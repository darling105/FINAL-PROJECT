using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
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

   [Header("Movement Flags")]
   public bool isInteracting;
   public bool isRotatingWithRootMotion;
   public bool canRotate;
   public bool isSprinting;
   public bool isInAir;
   public bool isGrounded;
   public bool isUsingRightHand;
   public bool isUsingLeftHand;

   public int pendingCriticalDamage;


}
