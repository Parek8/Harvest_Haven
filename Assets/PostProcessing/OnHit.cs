using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class OnHit : MonoBehaviour
{
    [SerializeField] internal UnityEngine.Rendering.VolumeProfile Volume;
    [SerializeField] internal float MinimalIntensity = 0f;
    [SerializeField] internal float MaximalIntensity = 1f;
    [SerializeField] internal float TransitionDuration = 1f;
    private Vignette vignette;

    private void Start()
    {
        Volume.TryGet(out vignette);
        vignette.intensity.value = 0f;
    }

    public void StartAnimation()
    {
        StartCoroutine(ChangeIntensityOverTime());
    }

    private IEnumerator ChangeIntensityOverTime()
    {
        if (vignette == null)
            yield break;

        float _newIntensity = 0;
        float _delay = Time.deltaTime * TransitionDuration;
        while (_newIntensity <= MaximalIntensity)
        {
            _newIntensity += Time.deltaTime;
            vignette.intensity.value = _newIntensity;

            yield return new WaitForSeconds(_delay);
        }

        while (_newIntensity >= MinimalIntensity)
        {
            _newIntensity -= Time.deltaTime;
            vignette.intensity.value = _newIntensity;

            yield return new WaitForSeconds(_delay);

        }
    }
}