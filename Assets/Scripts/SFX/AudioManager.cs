using System.Collections.Generic;
using UnityEngine;

internal class AudioManager : MonoBehaviour
{
    [field: Header("CONFIG")]
    [field: SerializeField] AudioListener AudioListener;

    [field: Header("ENVIRONMENT")]
    [field: SerializeField] List<AudioSource> TreeHitSounds;
    [field: SerializeField] List<AudioSource> RockHitSounds;

    [field: Header("")]
    [field: SerializeField] internal AudioSource CraftedItemSound { get; private set; }

    internal Dictionary<ObjectType, List<AudioSource>> Sounds = new();
    private void Start()
    {
        Sounds.Add(ObjectType.Tree, TreeHitSounds);
        Sounds.Add(ObjectType.Rock, RockHitSounds);
    }
    internal void PlaySound(AudioSource audio)
    {
        audio.Play();
    }
}