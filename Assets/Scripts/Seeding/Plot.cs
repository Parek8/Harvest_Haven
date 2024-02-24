using System.Collections.Generic;
using UnityEngine;

internal class Plot : Interactable
{
    internal int PlotIndex { get; private set; }
    internal bool isWatered { get; private set; } = true;
    internal PlantObject plantedPlant { get; private set; }
    internal int Days = 0;

    List<uint> times = new List<uint>();
    List<GameObject> stages = new List<GameObject>();
    private void Awake()
    {
        while (GameManager.game_manager == null)
        {

        }

        GameManager.game_manager.all_crops.Add(this);
    }
    private new void Start()
    {
        base.Start();
        Day_Cycle.On_New_Day_Subscribe(OnDayChange);
    }
    internal void SetIndex(int index) => this.PlotIndex = index;
    internal void LoadPlot(PlantObject plantObject, int days)
    {
        //Days = days;
        if (plantObject != null)
        {
            this.Plant(plantObject);

            for (int i = 0; i < days; i++)
            {
                //for (int j = 0; j < times.Count; j++)
                //{
                //    times[j]--;
                //}

                //if ((stages.Count == times.Count) && stages.Count > 0)
                //    if (times[0] <= 0)
                //        SpawnNewStage();
                OnDayChange();
            }
            DestroyPlant();
        }
    }

    internal void Plant(PlantObject plant, Item item = null)
    {
        if (plant != null)
        {
            plantedPlant = plant;
            times = new List<uint>((List<uint>)plantedPlant.Times);
            stages = new List<GameObject>((List<GameObject>)plantedPlant.Stages);
            if (item != null)
                GameManager.game_manager.player_inventory.DecreaseItemCount(item);
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
                else if ((stages.Count != times.Count))
                    Debug.Log("Nerovna se");

            Days++;
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
        //Debug.Log(_stage.name);
        times.RemoveAt(0);
        stages.RemoveAt(0);

        if (stages.Count <= 0 && times.Count <= 0)
        {
            DestroyPlant();
            _stage.AddComponent<Harvestable>().Setup((List<Item>)plantedPlant.DroppedItems);
        }
    }

    private Vector3 GetGameObjectOffset()
    {
        float _yOffset = 0.5f;

        return new Vector3(0, _yOffset, 0);
    }

    private void DestroyPlant()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Debug.Log(transform.GetChild(i).name);
        }
        if (transform.childCount != 0)
        {
            Destroy(transform.GetChild(0).gameObject);
        }
        Debug.Log("_______________________________________________");
    }

    internal override void Interact()
    {
        
    }
}