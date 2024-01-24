using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;


public class PostProcessManager : MonoBehaviour
{
    public Volume volume;
    private ChromaticAberration chromaticAberration;

    #region Singleton

    private static PostProcessManager instance = null;
    public static PostProcessManager Instance => instance;

    #endregion
    
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;
        }
    }
    private void Start()
    {
        // Récupérer le composant Chromatic Aberration du Volume Profile
        if(volume.profile.TryGet<ChromaticAberration>(out chromaticAberration))
        {
            // L'aberration chromatique est disponible
        }
    }

    // Fonction publique pour démarrer l'ajustement de l'aberration chromatique
    public void AdjustChromaticAberration(float targetIntensity, float duration)
    {
        StartCoroutine(AdjustChromaticAberrationCoroutine(targetIntensity, duration));
    }

    // Coroutine pour ajuster progressivement l'intensité
    private IEnumerator AdjustChromaticAberrationCoroutine(float targetIntensity, float duration)
    {
        float time = 0;
        float initialIntensity = chromaticAberration.intensity.value;

        while (time < duration)
        {
            // Interpoler l'intensité de l'aberration chromatique
            chromaticAberration.intensity.value = Mathf.Lerp(initialIntensity, targetIntensity, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        // S'assurer que l'intensité cible est bien atteinte à la fin
        chromaticAberration.intensity.value = targetIntensity;
    }
}
