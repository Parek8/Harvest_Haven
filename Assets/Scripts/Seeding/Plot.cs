using System.Collections.Generic;
using UnityEngine;

public class Plot : MonoBehaviour
{
    public bool isWatered { get; private set; } = true;
    public PlantObject plantedPlant { get; private set; }

    private MeshRenderer _renderer;

    List<uint> times = new List<uint>();
    List<GameObject> stages = new List<GameObject>();

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
            times = new List<uint>((List<uint>)plantedPlant.Times);
            stages = new List<GameObject>((List<GameObject>)plantedPlant.Stages);
            SpawnNewStage();
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

    public void Highlight()
    {
        if (_renderer.materials[0].color != Color.white)
            _renderer.material.color = Color.white;
    }

    public void Lowlight()
    {
        if (_renderer.materials[0].color == Color.white)
            _renderer.material.color = Color.grey;
    }
    private void SpawnNewStage()
    {
        DestroyPlant();
        Vector3 _spawnPos = transform.position + GetGameObjectOffset();
        GameObject _stage = Instantiate(stages[0], _spawnPos, stages[0].transform.localRotation, transform);

        times.RemoveAt(0);
        stages.RemoveAt(0);

        if (stages.Count <= 0 && times.Count <= 0)
            _stage.AddComponent<Destroyable>().SetupObject((List<Item>)plantedPlant.DroppedItems, new List<Tool_Type> { Tool_Type.pickaxe }, 1);
    }
    private Vector3 GetGameObjectOffset()
    {
        float _xOffset = stages[0].GetComponent<MeshRenderer>().bounds.size.x / 2;
        float _yOffset = 0.5f;
        float _zOffset = stages[0].GetComponent<MeshRenderer>().bounds.size.z / 2;

        return new Vector3(0, _yOffset, 0);
    }
    private void DestroyPlant()
    {
        if (transform.childCount > 0)
            Destroy(transform.GetChild(0).gameObject);
    }
}