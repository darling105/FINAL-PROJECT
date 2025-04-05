using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpUI : MonoBehaviour
{
    [Header("Player Level")]
    public int currentPlayerLevel;
    public int projectedPlayerLevel;
    public Text currentPlayerLevelText;
    public Text projectedPlayerLevelText;

    [Header("Shades")]
    public Text currentShades;
    public Text shadesRequiredToLevelUp;

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

}
