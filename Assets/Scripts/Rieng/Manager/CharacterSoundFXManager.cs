using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSoundFXManager : MonoBehaviour
{
    CharacterManager character;
    AudioSource audioSource;

    [Header("Taking Damage Sound")]
    public AudioClip[] takingDamageSounds;
    private List<AudioClip> potentialDamageSounds;
    private AudioClip lastDamageSoundPlayed;

    [Header("Weapon Whoosh Sound")]
    public List<AudioClip> potentialWeaponWhooshSounds;
    private AudioClip lastWeaponWhooshSoundPlayed;

    protected virtual void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        character = GetComponent<CharacterManager>();
    }
    public virtual void PlayRandomDamageSoundFX()
    {
        potentialDamageSounds = new List<AudioClip>();

        foreach (var damageSound in takingDamageSounds)
        {
            if (damageSound != lastDamageSoundPlayed)
            {
                potentialDamageSounds.Add(damageSound);
            }
        }
        int randomValue = Random.Range(0, potentialDamageSounds.Count);
        lastDamageSoundPlayed = takingDamageSounds[randomValue];
        audioSource.PlayOneShot(takingDamageSounds[randomValue], 0.4f);
    }

    public virtual void PlayRandomWeaponWhooshSoundFX()
    {
        potentialWeaponWhooshSounds = new List<AudioClip>();

        if (character.isUsingRightHand)
        {
            foreach (var whooshShound in character.characterInventoryManager.rightWeapon.weaponWhooshesSounds)
            {
                if (whooshShound != lastWeaponWhooshSoundPlayed)
                {
                    potentialWeaponWhooshSounds.Add(whooshShound);
                }
            }
            int randomValue = Random.Range(0, potentialWeaponWhooshSounds.Count);
            lastWeaponWhooshSoundPlayed = character.characterInventoryManager.rightWeapon.weaponWhooshesSounds[randomValue];
            audioSource.PlayOneShot(character.characterInventoryManager.rightWeapon.weaponWhooshesSounds[randomValue], 0.4f);
        }
        else
        {
            foreach (var whooshShound in character.characterInventoryManager.leftWeapon.weaponWhooshesSounds)
            {
                if (whooshShound != lastWeaponWhooshSoundPlayed)
                {
                    potentialWeaponWhooshSounds.Add(whooshShound);
                }
            }
            int randomValue = Random.Range(0, potentialWeaponWhooshSounds.Count);
            lastWeaponWhooshSoundPlayed = character.characterInventoryManager.leftWeapon.weaponWhooshesSounds[randomValue];
            audioSource.PlayOneShot(character.characterInventoryManager.leftWeapon.weaponWhooshesSounds[randomValue], 0.4f);
        }
    }

}