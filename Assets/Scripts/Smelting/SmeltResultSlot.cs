using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SmeltResultSlot : MonoBehaviour, IPointerClickHandler
{
    [field: SerializeField] Smelter smelter;
    [field: SerializeField] TMP_Text countLabel;
    [field: SerializeField] Image itemIcon;
    [field: SerializeField] SmeltItemSlot smeltItem;
    [field: SerializeField] FuelItemSlot  fuelItem;
    [field: SerializeField] GameObject FireVFX;

    Item item;
    int currentCapacity = 0;
    int count;
    readonly int maxCD = 5;
    float currCD = 0;
    private void Start() => UpdateUI();
    private void Update()
    {
        if (smeltItem.Get_Item() != null && fuelItem.Get_Item() != null)
        {
            if (!FireVFX.activeInHierarchy)
                FireVFX.SetActive(true);

            // smelting
            if (currCD <= 0)
            {
                if (currentCapacity <= 0)
                {
                    currentCapacity = fuelItem.Get_Item().FuelCapability;
                    fuelItem.DecreaseCount();
                }

                this.item = smeltItem.Get_Item().SmeltItem;
                count++;
                smeltItem.DecreaseCount();

                UpdateUI();
                currCD = maxCD;
            }
        }
        else if (FireVFX.activeInHierarchy)
            FireVFX.SetActive(false);
        currCD -= Time.deltaTime;
    }
    void UpdateUI()
    {
        if (item != null && count > 0)
        {
            this.itemIcon.sprite = this.item.ItemIcon;
            this.countLabel.text = $"{this.count}x";
        }
        else
        {
            this.itemIcon.sprite = GameManager.GameManagerInstance.NullItem.ItemIcon;
            this.countLabel.text = $"";
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            for (int i = 0; i < count; i++) 
                GameManager.GameManagerInstance.PlayerInventory.Add(this.item);
            count = 0;
            UpdateUI();
        }
    }
}