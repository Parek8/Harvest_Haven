using UnityEngine;

internal class AudioManager : MonoBehaviour
{
    [field: SerializeField] AudioListener AudioListener;
    internal void PlaySound(AudioSource audio)
    {
        audio.Play();
    }
}