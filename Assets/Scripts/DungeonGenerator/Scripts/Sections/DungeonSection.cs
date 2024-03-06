using DungeonGenerator.Scripts.Services;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DungeonGenerator.Scripts.Sections
{
    public class DungeonSection : MonoBehaviour
    {
        [field: Tooltip("Tags describing this section."), SerializeField] public List<string> SectionTags { get; private set; }
        [field: Tooltip("Tags for sections, that can be connected to this section."), SerializeField] public List<string> NextSectionsTags { get; private set; }
        [field: Tooltip("Array of all exits excluding the entrance."), SerializeField] public List<Transform> Exits { get; private set; }
        [field: Tooltip("Chance to spawn a DeadEnd section at exit."), Range(0, 100)] public float DeadEndChance;
        public float DeadEndPercentage => (DeadEndChance / 100);

        int _order = 0;
        DungeonManager _dungeonManager;
        public void Initialize(DungeonManager DungeonManager, int sourceOrder)
        {
            _order = sourceOrder + 1;

            _dungeonManager = DungeonManager;
            _dungeonManager.RegisterSection();
            GenerateNextSections();

        }
        private void GenerateNextSections()
        {
            if (NextSectionsTags.Any())
            {
                foreach (Transform _exit in Exits)
                {
                    DungeonSection _spawnedSection;
                    if (_dungeonManager.CanSpawn())
                    {
                        if (!RandomService.ShouldSpawnDeadEnd(DeadEndPercentage))
                            _spawnedSection = RandomService.GetRandomSection(_dungeonManager.Sections, NextSectionsTags);
                        else
                            _spawnedSection = RandomService.GetRandomSection(_dungeonManager.Sections, _dungeonManager.EndTags);

                        Instantiate(_spawnedSection, _exit.position, Quaternion.Euler(_exit.forward)).GetComponent<DungeonSection>().Initialize(_dungeonManager, _order);
                    }
                }
            }
        }
    }
}