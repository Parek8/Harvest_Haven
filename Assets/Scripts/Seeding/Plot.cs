using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class Plot : MonoBehaviour
{
    public bool isWatered { get; private set; } = false;
    public PlantObject plantedPlant { get; private set; }

    private MeshRenderer _renderer;

    [field: SerializeField] List<uint> times = new List<uint>();
    [field: SerializeField] List<GameObject> stages = new List<GameObject>();

    private void Start()
    {
        Day_Cycle.On_New_Day_Subscribe(OnDayChange);
        _renderer = GetComponent<MeshRenderer>();
    }

    public void Plant(PlantObject plant)
    {
        if (plant != null)
        {
            plantedPlant = plant;
            times = (List<uint>)plantedPlant.Times;
            stages = (List<GameObject>)plantedPlant.Stages;
            Instantiate(stages[0], transform);
        }
    }

    public void Harvest()
    {
        plantedPlant = null;
    }

    public void Water(bool watered)
    {
        isWatered = watered;
    }

    public void OnDayChange()
    {
        if (isWatered)
        {
            for (int i = 0; i < times.Count; i++)
            {
                times[i]--;
            }

            if (times[0] <= 0)
                SpawnNewStage();
        }
        Water(false);
    }

    public void Highlight()
    {
        if (_renderer.materials[0].color != Color.red)
            _renderer.material.color = Color.red;
    }

    public void Lowlight()
    {
        if (_renderer.materials[0].color == Color.red)
            _renderer.material.color = Color.grey;
    }
    private void SpawnNewStage()
    {
        Destroy(transform.GetChild(0));

        Instantiate(stages[0]);

        times.RemoveAt(0);
        stages.RemoveAt(0);
    }
}