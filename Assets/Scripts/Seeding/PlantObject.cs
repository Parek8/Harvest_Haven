using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "/Plants/Plant")]
public class PlantObject : ScriptableObject
{
    [field: SerializeField] List<uint> times = new List<uint>();
    [field: SerializeField] List<GameObject> stages = new List<GameObject>();

    public IReadOnlyCollection<uint> Times => times;
    public IReadOnlyCollection<GameObject> Stages => stages;
}
