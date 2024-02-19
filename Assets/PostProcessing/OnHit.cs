using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class OnHit : MonoBehaviour
{
    [SerializeField] internal UnityEngine.Rendering.VolumeProfile volume;
    [SerializeField] internal float minimalIntensity = 0f;
    [SerializeField] internal float maximalIntensity = 1f;
    [SerializeField] internal float transitionDuration = 1f;
    private float transitionTimer = 0f;
    private bool isMaxIntensity = false;
    private Vignette vignette;

    private void Start()
    {
        volume.TryGet(out vignette);
        vignette.intensity.value = 0f;
    }

    public void StartAnimation()
    {
        transitionTimer = 0f;

        isMaxIntensity = !isMaxIntensity;

        StartCoroutine(ChangeIntensityOverTime());
    }

    private IEnumerator ChangeIntensityOverTime()
    {
        if (vignette == null)
        {
            Debug.LogError("Vignette není inicializováno.");
            yield break;
        }

        float startIntensity = vignette.intensity.value;
        float targetIntensity = isMaxIntensity ? maximalIntensity : minimalIntensity;

        while (transitionTimer < transitionDuration/2)
        {
            transitionTimer += Time.deltaTime;

            float newIntensity = Mathf.Lerp(startIntensity, targetIntensity, transitionTimer / transitionDuration);

            vignette.intensity.value = newIntensity;

            yield return null;
        }

        while (transitionTimer < transitionDuration)
        {
            transitionTimer += Time.deltaTime;

            float newIntensity = Mathf.Lerp(targetIntensity, startIntensity, transitionTimer / transitionDuration);

            vignette.intensity.value = newIntensity;

            yield return null;
        }

        vignette.intensity.value = targetIntensity;
    }
}
