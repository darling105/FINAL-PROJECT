using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldEventManager : MonoBehaviour
{
    public List<FogWall> fogWalls;
    public UIBossHealthBar bossHealthBar;
    public EnemyBossManager boss;

    public bool bossFightIsActive; // danh boss
    public bool bossHasBeenAwakened; // danh thuc boss/ cut scene nhung co the se chet khi danh boss
    public bool bossHasBeenDefeated; // boss da chet

    private void Awake()
    {
        bossHealthBar = FindObjectOfType<UIBossHealthBar>();
    }

    public void ActiveBossFight()
    {
        bossFightIsActive = true;
        bossHasBeenAwakened = true;
        bossHealthBar.SetUIHealthBarToActive();

        foreach (var fogWall in fogWalls)
        {
            fogWall.ActivateFogWall();
        }
    }

    public void BossHasBeenDefeated()
    {
        bossHasBeenDefeated = true;
        bossFightIsActive = false;
        foreach (var fogWall in fogWalls)
        {
            fogWall.DeactivateFogWall();
        }
    }

}
