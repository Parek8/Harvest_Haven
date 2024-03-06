using DungeonGenerator.Scripts.Sections;
using DungeonGenerator.Scripts.Services;
using System.Collections.Generic;
using System.Linq;
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
            DungeonSection _startSection = PickRandomStartSection();
            if (_startSection != null)
            {
                Instantiate(_startSection.gameObject, Vector3.zero, Quaternion.identity).GetComponent<DungeonSection>().Initialize(this, 0);
            }
            else
                Debug.LogError("Starting section is null!");
        }

        /// <summary>
        /// Picks a random starting section.
        /// </summary>
        /// <returns>Randomly picked StartSection.</returns>
        private DungeonSection PickRandomStartSection() => RandomService.GetRandomSection(Sections, StartTags);

        /// <summary>
        /// Register generated section.
        /// </summary>
        public void RegisterSection()
        {
            DungeonSize--;
        }

        public bool CanSpawn() => (DungeonSize > 0);
    }
}