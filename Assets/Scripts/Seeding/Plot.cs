using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plot : MonoBehaviour
{
    public bool isWatered { get; private set; } = false;
    public Plant plantedPlant { get; private set; }

    private void Start()
    {
        Day_Cycle.On_New_Day_Subscribe(OnDayChange);
    }

    public void Plant(Plant plant)
    {
        if (plant != null)
            plantedPlant = plant;
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
        Water(false);
    }
}