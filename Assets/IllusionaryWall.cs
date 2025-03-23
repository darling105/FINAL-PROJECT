using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IllusionaryWall : MonoBehaviour
{
    public bool wallWasBeenHit;
    public Material illusionaryWallMaterial;
    public MeshRenderer illusionaryWallMeshRenderer;
    public float alpha;
    public float fadeTimer = 2.5f;
    public BoxCollider wallCollider;

    public AudioSource audioSource;
    public AudioClip illusionaryWallSound;

    private void Awake()
    {
        illusionaryWallMaterial = Instantiate(illusionaryWallMaterial);
        illusionaryWallMeshRenderer.material = illusionaryWallMaterial;
    }

    private void Update()
    {
        if(wallWasBeenHit)
        {
            FadeIllutionaryWall();
        }
    }

    public void FadeIllutionaryWall()
    {
        alpha = illusionaryWallMeshRenderer.material.color.a;
        alpha = alpha - (Time.deltaTime / fadeTimer);
        Color fadeWallColor = new Color(1, 1, 1, alpha);
        illusionaryWallMeshRenderer.material.color = fadeWallColor;

        if (wallCollider.enabled)
        {
            wallCollider.enabled = false;
            audioSource.PlayOneShot(illusionaryWallSound);
        }

        if (alpha <= 0)
        {
            Destroy(this);
        }
    }
}
