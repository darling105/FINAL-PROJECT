using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonSurface : MonoBehaviour
{
    public float poisonBuildupAmount = 7;

    public List<CharacterEffectsManager> charactersInsidePoisonSurface;

    private void OnTriggerEnter(Collider other)
    {
        CharacterEffectsManager character = other.GetComponent<CharacterEffectsManager>();

        if (character != null)
        {
            charactersInsidePoisonSurface.Add(character);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        CharacterEffectsManager character = other.GetComponent<CharacterEffectsManager>();

        if (character != null)
        {
            charactersInsidePoisonSurface.Remove(character);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        foreach (CharacterEffectsManager character in charactersInsidePoisonSurface)
        {
            if (character.isPoisoned)
                continue;
                
            character.poisonBuildup = character.poisonBuildup + poisonBuildupAmount * Time.deltaTime;
        }
    }
}
