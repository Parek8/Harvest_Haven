using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

internal class ChestInventoryButton : MonoBehaviour, IPointerClickHandler
{
    [field: SerializeField] Item item;
    [field: SerializeField] internal int slot_index { get; private set; }

    [field: SerializeField] Image item_image;
    [field: SerializeField] TMP_Text item_count;

    int _itemCount = 0;
    internal int ItemCount => _itemCount;
    internal bool IsAvailable => (this.item == null || this.ItemCount < 10);

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            for (int i = 0; i < _itemCount; _itemCount--)
                GameManager.game_manager.player_inventory.Add(this.item);

            Destroy(gameObject);
        }
    }

    internal void AssignItem(Item item, int count)
    {
        this.item = item;
        this._itemCount = count;

        this.item_image.sprite = this.item.ItemIcon;
        this.item_count.text = $"{this.ItemCount}x";
    }
}