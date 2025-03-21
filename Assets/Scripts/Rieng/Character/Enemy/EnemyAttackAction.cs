using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Enemy Actions/Attack Action")]
public class EnemyAttackAction : EnemyAction
{
    public bool canCombo;
    public EnemyAttackAction comboAction;
    public int attackScore = 3;
    public float recoveryTime = 2;

    public float maximumAttackAngle = 70;
    public float minimumAttackAngle = -70;

    public float minimumDistanceNeededToAttack = 0;
    public float maximumDistanceNeededToAttack = 3;

}
