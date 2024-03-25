using UnityEngine;

[System.Serializable]
public class SpecialSectionTags
{
    [field: Tooltip("Section, that HAS to be spawned."), SerializeField] public string SectionTag { get; private set; } = "";
    [field: Tooltip("How many times it has to spawn."), SerializeField] public uint MinimalSectionCount { get; private set; } = 1;
    [field: Tooltip("How many times it can spawn."), SerializeField] public uint MaximalSectionCount { get; private set; } = 1;

    public uint RemainingToSpawn { get; private set; }
    public float SpawnChance { get; private set; } = 0.0f;
    public void Init()
    {
        RemainingToSpawn = MaximalSectionCount;
    }
    public void SpawnedSpecialSection()
    {
        if (RemainingToSpawn == uint.MaxValue)
            RemainingToSpawn = MaximalSectionCount-1;

        RemainingToSpawn--;
        
        if (RemainingToSpawn < 1)
        {
            SpawnChance = 0.0f;
        }
    }
}