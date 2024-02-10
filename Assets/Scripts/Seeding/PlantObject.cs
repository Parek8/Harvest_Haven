using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Plants/PlantObject")]
internal class PlantObject : ScriptableObject
{
    [field: SerializeField] internal int PlantObjectIndex { get; private set; }

    [field: SerializeField] List<uint> times = new List<uint>();
    [field: SerializeField] List<GameObject> stages = new List<GameObject>();
    [field: SerializeField] List<Item> droppedItems = new List<Item>();
    internal IReadOnlyCollection<uint> Times => times;
    internal IReadOnlyCollection<GameObject> Stages => stages;
    internal IReadOnlyCollection<Item> DroppedItems => droppedItems;
}
