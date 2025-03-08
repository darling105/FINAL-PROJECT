// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class PlayerManager : MonoBehaviour
// {
//     [HideInInspector] public PlayerLocomotionManager playerLocomotionManager;
//     [HideInInspector] public PlayerManager playerManager;
//     [HideInInspector] public PlayerStatManager playerStatManager;
//     protected override void Awake()
//     {
//         base.Awake();

//         playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
//         characterAnimatorManager = GetComponent<CharacterAnimatorManager>();
//         playerManager = GetComponent<PlayerManager>();
//         playerStatManager = GetComponent<PlayerStatManager>();

//     }
//     protected override void Update()
//     {
//         base.Update();
//         playerLocomotionManager.HandleAllMovement();
//        //playerManager.currentStamina.OnValueChanged += PlayerUIManager.instance.playerUIHudManager.SetNewStaminaValue;

//     }
//     protected override void LateUpdate()
//     {
//         base.LateUpdate();
//         PlayerCamera.instance.HandleAllCameraActions();
//     }

// }
