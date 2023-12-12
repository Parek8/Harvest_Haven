using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Plants/PlantObject")]
public class PlantObject : ScriptableObject
{
    [field: SerializeField] List<uint> times = new List<uint>();
    [field: SerializeField] List<GameObject> stages = new List<GameObject>();
    [field: SerializeField] List<Item> droppedItems = new List<Item>();
    public IReadOnlyCollection<uint> Times => times;
    public IReadOnlyCollection<GameObject> Stages => stages;
    public IReadOnlyCollection<Item> DroppedItems => droppedItems;
}
