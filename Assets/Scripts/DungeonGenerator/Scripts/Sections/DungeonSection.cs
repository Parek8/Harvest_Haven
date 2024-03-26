using DungeonGenerator.Scripts.Services;
using System;
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
        [field: Tooltip("Bounds of the section"), SerializeField] public List<Collider> Bounds { get; private set; }
        public float DeadEndPercentage => (DeadEndChance / 100);

        int _order = 0;
        DungeonManager _dungeonManager;
        public void Initialize(DungeonManager DungeonManager, int sourceOrder)
        {
            //_order = sourceOrder + 1;
            _order = DungeonManager.RegisteredSections.Count;
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
                    DungeonSection _spawnedSection = null;
                    if (_dungeonManager.CanSpawn())
                    {
                        if (!CheckSpecialRooms(ref _spawnedSection))
                        {
                            if (!RandomService.ShouldSpawnDeadEnd(DeadEndPercentage) && !_dungeonManager.IsSectionIntersecting(Bounds))
                                _spawnedSection = RandomService.GetRandomSection(_dungeonManager.Sections, NextSectionsTags);
                        }
                    }

                    if (_spawnedSection == null)
                            _spawnedSection = RandomService.GetRandomSection(_dungeonManager.Sections, _dungeonManager.EndTags);
                    _neighborSections.Add(Instantiate(_spawnedSection, _exit.position, _exit.rotation).GetComponent<DungeonSection>());
                }

                _neighborSections.OrderBy(_ => RandomService.GetRandomInt(0, 100));

                List<DungeonSection> _neighborSectionsCopy = new List<DungeonSection>(_neighborSections);
                int _length = _neighborSectionsCopy.Count;

                for (int i = 0; i < _length; i++)
                {
                    int _index = RandomService.GetRandomInt(0, _neighborSectionsCopy.Count - 1);
                    _neighborSectionsCopy[_index].Initialize(_dungeonManager, _order + i);
                    _neighborSectionsCopy.RemoveAt(_index);
                }
            }
        }

        private bool CheckSpecialRooms(ref DungeonSection _spawnedSection)
        {
            _dungeonManager.SpecialTags = _dungeonManager.SpecialTags.OrderBy(_ => RandomService.GetRandomInt(0, 100)).ToList();

            foreach (SpecialSectionTags _specialTag in _dungeonManager.SpecialTags)
            {
                //Debug.Log(RandomService.ShouldSpawnSpecialSectionVerbal(_specialTag.RemainingToSpawn, _specialTag.MinimalSectionCount, _specialTag.MaximalSectionCount));

                if (_dungeonManager.IsSectionIntersecting(Bounds))
                    return false;
                else
                {
                    if (RandomService.ShouldSpawnSpecialSection(_specialTag.RemainingToSpawn, _specialTag.MinimalSectionCount, _specialTag.MaximalSectionCount) && _dungeonManager.SpecialSectionDictionary.ContainsKey(_order))
                    {
                        _specialTag.SpawnedSpecialSection();
                        _spawnedSection = RandomService.GetRandomSection(_dungeonManager.Sections, new List<string> { _dungeonManager.SpecialSectionDictionary[_order] });
                        _dungeonManager.SpecialSectionDictionary.Remove(_order);
                        return true;
                    }
                    //else
                    //{
                    //    int _randPos = Random.Range((int)_order + 1, _dungeonManager.DungeonSize + _order - 1);
                    //    int _max = 500;
                    //    while (_dungeonManager.SpecialSectionDictionary.ContainsKey(_randPos) && _max > 0)
                    //    {
                    //        _randPos = Random.Range((int)_order + 1, _dungeonManager.DungeonSize + _order - 1);
                    //        Debug.Log(_randPos);
                    //        _max--;
                    //    }
                    //    if (_max > 0)
                    //        _dungeonManager.SpecialSectionDictionary.Add(_randPos, _specialTag.SectionTag);
                    //}
                }
            }
            _spawnedSection = RandomService.GetRandomSection(_dungeonManager.Sections, NextSectionsTags);
            return false;
        }
    }
}