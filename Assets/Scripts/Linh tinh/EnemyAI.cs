using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    private NavMeshAgent _agent;
    public float ViewRadius;
    public float ViewAngle;
    public LayerMask PlayerMask;
    public LayerMask ObstacleMask;
    private bool _playerInside;
    private Vector3 _nextPosition;
    private void Awake() {
        _agent = GetComponent<NavMeshAgent>();
    }
    private void Start()
    {
        _playerInside = false;
    }
    private void Update()
    {
        FindPlayerInView();
        if(_playerInside){
            _agent.Move(_nextPosition);
        }

    }

    private void FindPlayerInView()
    {
        Collider[] playerInRange = Physics.OverlapSphere(transform.position, ViewRadius, PlayerMask);
        foreach (Collider col in playerInRange)
        {
            Transform player = col.transform;
            Vector3 dirToPlayer = (player.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, dirToPlayer) < ViewAngle / 2)
            {
                float distanceToPlayer = Vector3.Distance(player.position, transform.position);
                if (!Physics.Raycast(transform.position, dirToPlayer, distanceToPlayer, ObstacleMask))
                {
                    _playerInside = true;
                }
                else{
                    _playerInside = false;
                    return;
                }
            }
            if(Vector3.Distance(transform.position,player.position) > ViewRadius){
                _playerInside = false;
                return;
            }
            if(_playerInside){
                _nextPosition = player.position;
            }
        }
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position,ViewRadius);
    }
}
