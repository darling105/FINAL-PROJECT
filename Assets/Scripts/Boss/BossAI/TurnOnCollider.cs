using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOnCollider : MonoBehaviour
{
    //public bool DetectedPlayer {get; private set;}
    [SerializeField] private SphereCollider _collider;
    public event Action<bool> PlayerHasDetected;
    void Awake()
    {
        _collider = GetComponent<SphereCollider>();
    }
    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player")){
            PlayerHasDetected?.Invoke(true);
            // cause dame to player if this is arm collider
        }
    }
    void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player")){
            PlayerHasDetected?.Invoke(false);
        }
    }
    void OnDisable()
    {
        PlayerHasDetected?.Invoke(false);
    }
}
