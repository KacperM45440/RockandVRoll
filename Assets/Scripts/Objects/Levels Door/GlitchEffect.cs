using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlitchEffect : MonoBehaviour
{
    [SerializeField] bool levelStartScreen;
    [Header("Glitch effect")]
    [SerializeField] float maxGlitchTime;
    [SerializeField] Material glitchMaterial;
    [Header("Glitch Values")]
    [SerializeField] float noiseAmount = 150f;
    [SerializeField] float glitchStrength = 20f;
    [Range(0.0f, 1.0f)]
    [SerializeField] float scanLinesStrength = 0.95f;
    [Header("Glitch Sound")]
    [SerializeField] AudioSource glitchSound;

    private void Awake()
    {
        if (levelStartScreen)
        {
            StartCoroutine(GlitchScreenEffect(false));
        }
        else
        {
            SetMaterialValues(0, 0, 1);
        }

    }

    float percent;
    float elapsedGlitchTime;

    public IEnumerator GlitchScreenEffect(bool playerInRange)
    {
        glitchSound.Play();
        if (playerInRange)
        {
            elapsedGlitchTime = 0f;
            while (elapsedGlitchTime < maxGlitchTime)
            {
                elapsedGlitchTime += Time.deltaTime;
                percent = elapsedGlitchTime / maxGlitchTime;
                SetMaterialValues(noiseAmount * percent, glitchStrength * percent, scanLinesStrength * (1 - percent));
                if(percent < 0.5f)
                {
                    glitchSound.volume = 0.5f;
                }
                else glitchSound.volume = percent;
                yield return null;
            }
            SetMaterialValues(noiseAmount, glitchStrength, scanLinesStrength);
            yield return null;
        }
        else if (!playerInRange)
        {
            elapsedGlitchTime = maxGlitchTime;
            while (elapsedGlitchTime > 0)
            {
                elapsedGlitchTime -= Time.deltaTime * 20;
                percent = elapsedGlitchTime / maxGlitchTime;
                SetMaterialValues(noiseAmount * percent, glitchStrength * percent, scanLinesStrength * (1 - percent));
                glitchSound.volume = percent;

                yield return null;
            }
            glitchSound.volume = 0f;
            SetMaterialValues(0, 0, 1);
            yield return null;
        }
        yield return null;
    }

    public void SetMaterialValues(float noiseAmountValue, float glitchStrengthValue, float scanLinesStrengthValue)
    {
        glitchMaterial.SetFloat("_NoiseAmount", noiseAmountValue);
        glitchMaterial.SetFloat("_GlitchStrength", glitchStrengthValue);
        glitchMaterial.SetFloat("_ScanLinesStrength", scanLinesStrengthValue);
    }
}
