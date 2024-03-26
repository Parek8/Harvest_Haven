using DungeonGenerator.Scripts.Sections;
using DungeonGenerator.Scripts.Services;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace DungeonGenerator.Scripts
{
    public class DungeonManager : MonoBehaviour
    {
        /// <summary>
        /// Number of sections in the dungeon.
        /// </summary>
        [field: Tooltip("Set the number of generated sections."), SerializeField] public int DungeonSize { get; private set; } = 50;
        /// <summary>
        /// Dungeon seed.
        /// </summary>
        [field: Tooltip("Seed of the dungeon. Can be saved for later use. Let value be set to 0 if you want to generate new seed."), SerializeField] public int DungeonSeed { get; private set; } = 0;
        /// <summary>
        /// All spawnable sections.
        /// </summary>
        [field: Tooltip("All the sections this dungeon can generate."), SerializeField] public List<DungeonSection> Sections { get; private set; }
        /// <summary>
        /// All section tags, that can be used as a starting section.
        /// </summary>
        [field: Tooltip("Tags, that can be spawned as a first section."), SerializeField] public List<string> StartTags { get; private set; }
        /// <summary>
        /// All section tags, that can be used as an ending section.
        /// </summary>
        [field: Tooltip("Tags, that can be spawned as a first section."), SerializeField] public List<string> EndTags { get; private set; }
        [field: Tooltip("Tags, that have to be spawned."), SerializeField] public List<SpecialSectionTags> SpecialTags { get; set; }



        public List<DungeonSection> RegisteredSections { get; private set; } = new List<DungeonSection>();

        public IEnumerable<Collider> RegisteredColliders => (RegisteredSections.SelectMany(s => s.Bounds));
        public Dictionary<int, string> SpecialSectionDictionary = new Dictionary<int, string>();
        /// <summary>
        /// Initializes the seed.
        /// </summary>
        private void Awake()
        {
            if (DungeonSeed == 0)
                DungeonSeed = Services.RandomService.Seed;
            else
                Services.RandomService.SetSeed(DungeonSeed);

            GenerateDungeon();
        }

        /// <summary>
        /// Generates the dungeon.
        /// </summary>
        private void GenerateDungeon()
        {
            if (AreTagsValid())
            {
                DungeonSection _startSection = PickRandomStartSection();
                if (_startSection != null)
                {
                    Instantiate(_startSection.gameObject, Vector3.zero, Quaternion.identity).GetComponent<DungeonSection>().Initialize(this, 0);
                }
                else
                    Debug.LogError("Starting section is null!");
            }
        }

        /// <summary>
        /// Picks a random starting section.
        /// </summary>
        /// <returns>Randomly picked StartSection.</returns>
        private DungeonSection PickRandomStartSection() => RandomService.GetRandomSection(Sections, StartTags);

        /// <summary>
        /// Register generated section.
        /// </summary>
        public void RegisterSection(DungeonSection registeredSection)
        {
            RegisteredSections.Add(registeredSection);
            DungeonSize--;
        }

        public bool IsSectionIntersecting(List<Collider> newBounds) => ((RegisteredColliders.Except(newBounds)).Any(c => c.bounds.Intersects(newBounds[0].bounds)));

        public bool CanSpawn() => (DungeonSize > 0);

        private bool AreTagsValid()
        {
            if (SpecialTags.Sum(t => t.MinimalSectionCount) > DungeonSize)
            {
                Debug.LogError("Dungeon is not big enough!");
                return false;
            }
            
            foreach (string tag in SpecialTags.Select(t => t.SectionTag))
            {
                if (SpecialTags.Count(t => t.SectionTag == tag) > 1)
                {
                    Debug.LogError($"Same tags ({tag}) found multiple times ({SpecialTags.Count(t => t.SectionTag == tag)}x)!");
                    return false;
                }
            }

            foreach (SpecialSectionTags tag in SpecialTags)
            {
                if (tag.MinimalSectionCount > tag.MaximalSectionCount)
                {
                    Debug.LogError($"The tag {tag.SectionTag} has MinimalSectionCount ({tag.MinimalSectionCount}) set to a bigger value than MaximalSectionValue ({tag.MaximalSectionCount})!");
                    return false;
                }
            }

            Dictionary<int, string> _specialTags = new();


            foreach (SpecialSectionTags tag in SpecialTags)
            {
                int _randCount = Random.Range((int)tag.MinimalSectionCount, (int)tag.MaximalSectionCount);
                for (int i = 0; i < _randCount; i++)
                {
                    int _randPos = Random.Range(1, DungeonSize);
                    int _max = 50;
                    while (_specialTags.ContainsKey(_randPos) && _max > 0)
                    {
                        _randPos = Random.Range(1, DungeonSize);
                        _max--;
                    }

                    _specialTags.Add(_randPos, tag.SectionTag);
                }
            }

            Dictionary<string, int> _specialTagsCount = new Dictionary<string, int>();

            // Spoèítání poètu prvkù v dictionary
            foreach (string _specialTag in _specialTags.Values)
            {
                if (_specialTagsCount.ContainsKey(_specialTag))
                    _specialTagsCount[_specialTag]++;
                else
                    _specialTagsCount[_specialTag] = 1;
            }
            // Pro každý prvek v dictionary zavolání metody Init() s odpovídajícím poètem
            foreach (KeyValuePair<string, int> _tag in _specialTagsCount)
            {
                SpecialSectionTags x = SpecialTags.Where(t => t.SectionTag == _tag.Key).First();
                x.Init(_tag.Value);
            }


            SpecialSectionDictionary = _specialTags;



            foreach (KeyValuePair<int, string> tag in SpecialSectionDictionary)
            {
                Debug.Log($"{tag.Value}: {tag.Key}");
            }
            return true;
        }
    }
}