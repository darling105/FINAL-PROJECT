using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpUI : MonoBehaviour
{
    public PlayerManager playerManager;
    public Button confirmLevelUpButton;

    [Header("Player Level")]
    public int currentPlayerLevel;
    public int projectedPlayerLevel;
    public Text currentPlayerLevelText;
    public Text projectedPlayerLevelText;

    [Header("Shades")]
    public Text currentShadesText;
    public Text shadesRequiredToLevelUpText;
    private int shadesRequiredToLevelUp;
    public int baseLevelUpCost = 5;

    [Header("Health")]
    public Slider healthSlider;
    public Text currentHealthLevelText;
    public Text projectedHealthLevelText;

    [Header("Focus")]
    public Slider focusSlider;
    public Text currentFocusLevelText;
    public Text projectedFocusLevelText;

    [Header("Stamina")]
    public Slider staminaSlider;
    public Text currentStaminaLevelText;
    public Text projectedStaminaLevelText;

    [Header("Poise")]
    public Slider poiseSlider;
    public Text currentPoiseLevelText;
    public Text projectedPoiseLevelText;

    [Header("Strength")]
    public Slider strengthSlider;
    public Text currentStrengthLevelText;
    public Text projectedStrengthLevelText;

    [Header("Dexterity")]
    public Slider dexteritySlider;
    public Text currentDexterityLevelText;
    public Text projectedDexterityLevelText;

    [Header("Intelligence")]
    public Slider intelligenceSlider;
    public Text currentIntelligenceLevelText;
    public Text projectedIntelligenceLevelText;

    [Header("Faith")]
    public Slider faithSlider;
    public Text currentFaithLevelText;
    public Text projectedFaithLevelText;

    //cap nhat tat ca chi so tren ui

    private void OnEnable()
    {
        currentPlayerLevel = playerManager.playerStatsManager.playerLevel;
        currentPlayerLevelText.text = currentPlayerLevel.ToString();

        projectedPlayerLevel = playerManager.playerStatsManager.playerLevel;
        projectedPlayerLevelText.text = projectedPlayerLevel.ToString();

        healthSlider.value = playerManager.playerStatsManager.healthLevel;
        healthSlider.minValue = playerManager.playerStatsManager.healthLevel;
        healthSlider.maxValue = 99;
        currentHealthLevelText.text = playerManager.playerStatsManager.healthLevel.ToString();
        projectedHealthLevelText.text = playerManager.playerStatsManager.healthLevel.ToString();


        focusSlider.value = playerManager.playerStatsManager.focusLevel;
        focusSlider.minValue = playerManager.playerStatsManager.focusLevel;
        focusSlider.maxValue = 99;
        currentFocusLevelText.text = playerManager.playerStatsManager.focusLevel.ToString();
        projectedFocusLevelText.text = playerManager.playerStatsManager.focusLevel.ToString();


        staminaSlider.value = playerManager.playerStatsManager.staminaLevel;
        staminaSlider.minValue = playerManager.playerStatsManager.staminaLevel;
        staminaSlider.maxValue = 99;
        currentStaminaLevelText.text = playerManager.playerStatsManager.staminaLevel.ToString();
        projectedStaminaLevelText.text = playerManager.playerStatsManager.staminaLevel.ToString();


        poiseSlider.value = playerManager.playerStatsManager.poiseLevel;
        poiseSlider.minValue = playerManager.playerStatsManager.poiseLevel;
        poiseSlider.maxValue = 99;
        currentPoiseLevelText.text = playerManager.playerStatsManager.poiseLevel.ToString();
        projectedPoiseLevelText.text = playerManager.playerStatsManager.poiseLevel.ToString();


        strengthSlider.value = playerManager.playerStatsManager.strengthLevel;
        strengthSlider.minValue = playerManager.playerStatsManager.strengthLevel;
        strengthSlider.maxValue = 99;
        currentStrengthLevelText.text = playerManager.playerStatsManager.strengthLevel.ToString();
        projectedStrengthLevelText.text = playerManager.playerStatsManager.strengthLevel.ToString();


        dexteritySlider.value = playerManager.playerStatsManager.dexterityLevel;
        dexteritySlider.minValue = playerManager.playerStatsManager.dexterityLevel;
        dexteritySlider.maxValue = 99;
        currentDexterityLevelText.text = playerManager.playerStatsManager.dexterityLevel.ToString();
        projectedDexterityLevelText.text = playerManager.playerStatsManager.dexterityLevel.ToString();


        intelligenceSlider.value = playerManager.playerStatsManager.intelligenceLevel;
        intelligenceSlider.minValue = playerManager.playerStatsManager.intelligenceLevel;
        intelligenceSlider.maxValue = 99;
        currentIntelligenceLevelText.text = playerManager.playerStatsManager.intelligenceLevel.ToString();
        projectedIntelligenceLevelText.text = playerManager.playerStatsManager.intelligenceLevel.ToString();


        faithSlider.value = playerManager.playerStatsManager.faithLevel;
        faithSlider.minValue = playerManager.playerStatsManager.faithLevel;
        faithSlider.maxValue = 99;
        currentFaithLevelText.text = playerManager.playerStatsManager.faithLevel.ToString();
        projectedFaithLevelText.text = playerManager.playerStatsManager.faithLevel.ToString();

        currentShadesText.text = playerManager.playerStatsManager.currentShadesCount.ToString();


        UpdateProjectedPlayerLevel();

    }

    public void ConfirmPlayerLevelUpStats()
    {
        playerManager.playerStatsManager.playerLevel = projectedPlayerLevel;
        playerManager.playerStatsManager.healthLevel = Mathf.RoundToInt(healthSlider.value);
        playerManager.playerStatsManager.focusLevel = Mathf.RoundToInt(focusSlider.value);
        playerManager.playerStatsManager.staminaLevel = Mathf.RoundToInt(staminaSlider.value);
        playerManager.playerStatsManager.poiseLevel = Mathf.RoundToInt(poiseSlider.value);
        playerManager.playerStatsManager.strengthLevel = Mathf.RoundToInt(strengthSlider.value);
        playerManager.playerStatsManager.dexterityLevel = Mathf.RoundToInt(dexteritySlider.value);
        playerManager.playerStatsManager.intelligenceLevel = Mathf.RoundToInt(intelligenceSlider.value);
        playerManager.playerStatsManager.faithLevel = Mathf.RoundToInt(faithSlider.value);

        playerManager.playerStatsManager.currentHealth = playerManager.playerStatsManager.SetMaxHealthFromHealthLevel();
        playerManager.playerStatsManager.currentFocusPoint = playerManager.playerStatsManager.SetMaxFocusPointFromFocusLevel();
        playerManager.playerStatsManager.currentStamina = playerManager.playerStatsManager.SetMaxStaminaFromStaminaLevel();

        playerManager.playerStatsManager.currentShadesCount = playerManager.playerStatsManager.currentShadesCount - shadesRequiredToLevelUp;
        playerManager.uiManager.shadesCount.text = playerManager.playerStatsManager.currentShadesCount.ToString();

        gameObject.SetActive(false);
    }

    private void CalculateShadesRequiredToLevelUp()
    {
        for (int i = 0; i < projectedPlayerLevel; i++)
        {
            shadesRequiredToLevelUp = shadesRequiredToLevelUp + Mathf.RoundToInt((projectedPlayerLevel * baseLevelUpCost) * 1.5f);
        }
    }

    private void UpdateProjectedPlayerLevel()
    {
        shadesRequiredToLevelUp = 0;

        projectedPlayerLevel = currentPlayerLevel;
        projectedPlayerLevel = projectedPlayerLevel + Mathf.RoundToInt(healthSlider.value) - playerManager.playerStatsManager.healthLevel;
        projectedPlayerLevel = projectedPlayerLevel + Mathf.RoundToInt(focusSlider.value) - playerManager.playerStatsManager.focusLevel;
        projectedPlayerLevel = projectedPlayerLevel + Mathf.RoundToInt(staminaSlider.value) - playerManager.playerStatsManager.staminaLevel;
        projectedPlayerLevel = projectedPlayerLevel + Mathf.RoundToInt(poiseSlider.value) - playerManager.playerStatsManager.poiseLevel;
        projectedPlayerLevel = projectedPlayerLevel + Mathf.RoundToInt(strengthSlider.value) - playerManager.playerStatsManager.strengthLevel;
        projectedPlayerLevel = projectedPlayerLevel + Mathf.RoundToInt(dexteritySlider.value) - playerManager.playerStatsManager.dexterityLevel;
        projectedPlayerLevel = projectedPlayerLevel + Mathf.RoundToInt(intelligenceSlider.value) - playerManager.playerStatsManager.intelligenceLevel;
        projectedPlayerLevel = projectedPlayerLevel + Mathf.RoundToInt(faithSlider.value) - playerManager.playerStatsManager.faithLevel;
        projectedPlayerLevelText.text = projectedPlayerLevel.ToString();

        CalculateShadesRequiredToLevelUp();
        shadesRequiredToLevelUpText.text = shadesRequiredToLevelUp.ToString();

        if (playerManager.playerStatsManager.currentShadesCount < shadesRequiredToLevelUp)
        {
            confirmLevelUpButton.interactable = false;
        }
        else
        {
            confirmLevelUpButton.interactable = true;
        }
    }

    public void UpdateHealthLevelSlider()
    {
        projectedHealthLevelText.text = healthSlider.value.ToString();
        UpdateProjectedPlayerLevel();
    }

    public void UpdateFocusLevelSlider()
    {
        projectedFocusLevelText.text = focusSlider.value.ToString();
        UpdateProjectedPlayerLevel();
    }

    public void UpdateStaminaLevelSlider()
    {
        projectedStaminaLevelText.text = staminaSlider.value.ToString();
        UpdateProjectedPlayerLevel();
    }

    public void UpdatePoiseLevelSlider()
    {
        projectedPoiseLevelText.text = poiseSlider.value.ToString();
        UpdateProjectedPlayerLevel();
    }

    public void UpdateStrengthLevelSlider()
    {
        projectedStrengthLevelText.text = strengthSlider.value.ToString();
        UpdateProjectedPlayerLevel();
    }

    public void UpdateDexterityLevelSlider()
    {
        projectedDexterityLevelText.text = dexteritySlider.value.ToString();
        UpdateProjectedPlayerLevel();
    }

    public void UpdateIntelligenceLevelSlider()
    {
        projectedIntelligenceLevelText.text = intelligenceSlider.value.ToString();
        UpdateProjectedPlayerLevel();
    }

    public void UpdateFaithLevelSlider()
    {
        projectedFaithLevelText.text = faithSlider.value.ToString();
        UpdateProjectedPlayerLevel();
    }

}
