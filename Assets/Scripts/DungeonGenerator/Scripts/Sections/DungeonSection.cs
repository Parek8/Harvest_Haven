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
        [field: Tooltip("Bounds of the section"), SerializeField] public List<Collider> Colliders { get; private set; }
        public float DeadEndPercentage => (DeadEndChance / 100);

        int _order = 0;
        DungeonManager _dungeonManager;
        public void Initialize(DungeonManager DungeonManager, int sourceOrder)
        {
            _order = sourceOrder + 1;
            gameObject.name = _order.ToString();
            _dungeonManager = DungeonManager;
            _dungeonManager.RegisterSection(this);
            GenerateNextSections();
        }
        private void GenerateNextSections()
        {
            if (NextSectionsTags.Any())
            {
                List<DungeonSection> _neighborSections = new();
                foreach (Transform _exit in Exits)
                {
                    DungeonSection _spawnedSection;
                    if (_dungeonManager.CanSpawn())
                    {
                        if (!RandomService.ShouldSpawnDeadEnd(DeadEndPercentage))
                            _spawnedSection = RandomService.GetRandomSection(_dungeonManager.Sections, NextSectionsTags);
                        else
                            _spawnedSection = RandomService.GetRandomSection(_dungeonManager.Sections, _dungeonManager.EndTags);

                    }
                    else
                        _spawnedSection = RandomService.GetRandomSection(_dungeonManager.Sections, _dungeonManager.EndTags);

                    _neighborSections.Add(Instantiate(_spawnedSection, _exit.position, _exit.rotation).GetComponent<DungeonSection>());
                }

                _neighborSections.OrderBy(_ => RandomService.GetRandomInt(0, 100));

                List<DungeonSection> _neighborSectionsCopy = new List<DungeonSection>(_neighborSections);
                int _length = _neighborSectionsCopy.Count;

                for (int i = 0; i < _length; i++)
                {
                    int _index = RandomService.GetRandomInt(0, _neighborSectionsCopy.Count - 1);
                    _neighborSectionsCopy[_index].Initialize(_dungeonManager, _order + i + 1);
                    _neighborSectionsCopy.RemoveAt(_index);
                }
            }
        }
    }
}