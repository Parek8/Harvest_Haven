using UnityEngine;

public class SmeltResultSlot : MonoBehaviour
{
    [field: SerializeField] Smelter smelter;

    SmeltItemSlot smeltItem;
    FuelItemSlot  fuelItem;
    Item item;
    int currentCapacity = 0;
    int count;
    readonly int maxCD = 5;
    float currCD = 0;

    private void Update()
    {
        if (smeltItem != null && fuelItem != null)
        {
            // smelting
            if (currCD <= 0)
            {
                if (currentCapacity <= 0)
                {
                    currentCapacity = fuelItem.Get_Item().FuelCapability;
                    fuelItem.DecreaseCount();
                }

                currCD = maxCD;
            }
        }
        currCD -= Time.deltaTime;
    }
}