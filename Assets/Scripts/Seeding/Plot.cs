using System.Collections.Generic;
using UnityEngine;

internal class Plot : Interactable
{
    internal bool isWatered { get; private set; } = true;
    internal PlantObject plantedPlant { get; private set; }

    List<uint> times = new List<uint>();
    List<GameObject> stages = new List<GameObject>();

    private new void Start()
    {
        base.Start();
        Day_Cycle.On_New_Day_Subscribe(OnDayChange);
    }

    internal void Plant(PlantObject plant)
    {
        if (plant != null)
        {
            plantedPlant = plant;
            times = new List<uint>((List<uint>)plantedPlant.Times);
            stages = new List<GameObject>((List<GameObject>)plantedPlant.Stages);
            SpawnNewStage();
        }
    }

    internal void Harvest()
    {
        plantedPlant = null;
    }

    internal void Water(bool watered)
    {
        isWatered = watered;
    }

    internal void OnDayChange()
    {
        if (isWatered && plantedPlant != null)
        {
            for (int i = 0; i < times.Count; i++)
            {
                times[i]--;
            }

            if ((stages.Count == times.Count) && stages.Count > 0)
                if (times[0] <= 0)
                    SpawnNewStage();
        }
        else if (!isWatered && plantedPlant != null)
        {
            DestroyPlant();
            Debug.Log("Plant has died!");
        }
        Water(true);
    }

    private void SpawnNewStage()
    {
        DestroyPlant();
        Vector3 _spawnPos = transform.position + GetGameObjectOffset();
        GameObject _stage = Instantiate(stages[0], _spawnPos, stages[0].transform.localRotation, transform);

        times.RemoveAt(0);
        stages.RemoveAt(0);

        if (stages.Count <= 0 && times.Count <= 0)
            _stage.AddComponent<Harvestable>().Setup((List<Item>)plantedPlant.DroppedItems);
    }

    private Vector3 GetGameObjectOffset()
    {
        float _yOffset = 0.5f;

        return new Vector3(0, _yOffset, 0);
    }

    private void DestroyPlant()
    {
        if (transform.childCount > 0)
            Destroy(transform.GetChild(0).gameObject);
    }

    internal override void Interact()
    {
        
    }
}