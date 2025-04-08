using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEffectsManager : CharacterEffectsManager
{
    PlayerManager player;

    PoisonBuildUpBar poisonBuildUpBar;
    PoisonAmountBar poisonAmountBar;

    public GameObject currentParticleFX;
    public GameObject instantiatedFXModel;
    public int amountToBeHealed;

    protected override void Awake()
    {
        base.Awake();
        player = GetComponent<PlayerManager>();

        poisonBuildUpBar = FindObjectOfType<PoisonBuildUpBar>();
        poisonAmountBar = FindObjectOfType<PoisonAmountBar>();
    }

    public void HealPlayerFromEffect()
    {
        player.playerStatsManager.HealPlayer(amountToBeHealed);
        GameObject healParticles = Instantiate(currentParticleFX, player.playerStatsManager.transform);
        Destroy(instantiatedFXModel.gameObject, 1);
        StartCoroutine(DelayLoadWeapons());
    }
    private IEnumerator DelayLoadWeapons()
    {
        yield return new WaitForSeconds(1f);
        if (player.playerWeaponSlotManager != null)
        {
            player.playerWeaponSlotManager.LoadBothWeaponOnSlot();
        }
    }

    protected override void HandlePoisonBuildUp()
    {
        if (poisonBuildup <= 0)
        {
            poisonBuildUpBar.gameObject.SetActive(false);
        }
        else
        {
            poisonBuildUpBar.gameObject.SetActive(true);
        }
        base.HandlePoisonBuildUp();
        poisonBuildUpBar.SetPoisonBuildUp(Mathf.RoundToInt(poisonBuildup));
    }

    protected override void HandleIsPoisonEffect()
    {
        if (isPoisoned == false)
        {
            poisonAmountBar.gameObject.SetActive(false);
        }
        else
        {
            poisonAmountBar.gameObject.SetActive(true);
        }

        base.HandleIsPoisonEffect();

        poisonAmountBar.SetPoisonAmount(Mathf.RoundToInt(poisonAmount));
    }

}
