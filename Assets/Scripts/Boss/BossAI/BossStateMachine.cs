using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossStateMachine : MonoBehaviour
{
    private BossStateFactory _state;
    public BossBaseState CurrentState;
    public NavMeshAgent _bossAgent;
    public TurnOnCollider ArmCollider;
    public TurnOnCollider BodyCollider;
    public TurnOnCollider RushInCollider;
    public bool _canDoMeleeAttack = false;
    public bool _canRushInAttack = false;
    public Transform PlayerPosition;
    public HandleAnimator TheAnimator;
    void Awake()
    {
        _state = new BossStateFactory(this);
    }
    void Start()
    {
        TheAnimator = GetComponentInChildren<HandleAnimator>();
        BodyCollider.PlayerHasDetected += PlayerInNormalAttackRange;
        ArmCollider.PlayerHasDetected += HitPlayer;
        RushInCollider.PlayerHasDetected += CanRushInPlayer;
        CurrentState = _state.Idle();
        CurrentState.EnterState();
    }
    void Update()
    {
        CurrentState.UpdateState();
    }

    private void PlayerInNormalAttackRange(bool detect)
    {
        _canDoMeleeAttack = detect;
    }
    private void HitPlayer(bool hasHit)
    {
        if (hasHit)
        {
            // cause dame to player
            Debug.Log("hit the player");
        }
    }

    private void CanRushInPlayer(bool canRushIn)
    {
        _canRushInAttack = canRushIn;
    }

    public void TurnOffNavMesh()
    {
        _bossAgent.isStopped = true;
        _bossAgent.ResetPath(); // Clears the current path
        _bossAgent.velocity = Vector3.zero; // Stops any remaining movement
        //_bossAgent.enabled = false; // Now disable the agent
    }
    public void TurnOnNavMesh()
    {
        _bossAgent.enabled = true;
        
    }
}
