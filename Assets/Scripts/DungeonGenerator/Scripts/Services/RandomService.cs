using DungeonGenerator.Scripts.Sections;
using System;
using System.Linq;
using System.Collections.Generic;

namespace DungeonGenerator.Scripts.Services
{
    public static class RandomService
    {
        /// <summary>
        /// Random object.
        /// </summary>
        static Random _random;
        /// <summary>
        /// Used seed.
        /// </summary>
        static int _seed;

        /// <summary>
        /// Used seed.
        /// </summary>
        public static int Seed => _seed;

        /// <summary>
        /// Creates a random seeded RandomService.
        /// </summary>
        static RandomService()
        {
            _random = new Random();
            _seed = _random.Next(Int32.MinValue, Int32.MaxValue);

            _random = new Random(_seed);
        }

        /// <summary>
        /// Sets the seed for the RandomService.
        /// </summary>
        /// <param name="seed">Set seed in the random.</param>
        public static void SetSeed(int seed)
        {
            _seed = seed;
            _random = new Random(seed);
        }

        /// <summary>
        /// Return a random value in the min - max range.
        /// </summary>
        /// <param name="min">Minimal INCLUDED value.</param>
        /// <param name="max">Maximal INCLUDED value.</param>
        /// <returns>Random value between min - max.</returns>
        public static int GetRandomInt(int min, int max) => _random.Next(min, max+1);

        /// <summary>
        /// Decides, if dead end should be spawned.
        /// </summary>
        /// <param name="percentage">Percentage to be met.</param>
        /// <returns>Bool, if dead end should be spawned.</returns>
        public static bool ShouldSpawnDeadEnd(float percentage) => (_random.NextDouble() < percentage);

        /// <summary>
        /// Selects one random section.
        /// </summary>
        /// <param name="Sections">List of sections to pick from.</param>
        /// <param name="Tags">Tags of sections to pick from.</param>
        /// <returns>Randomly picked DungeonSection</returns>
        public static DungeonSection GetRandomSection(List<DungeonSection> Sections, List<string> Tags) => Sections.Where(_section => _section.SectionTags
                                                                                                                                 .Intersect(Tags)
                                                                                                                                 .Any())
                                                                                                                                 .OrderBy(_ => Seed)
                                                                                                                                 .FirstOrDefault();
    }
}