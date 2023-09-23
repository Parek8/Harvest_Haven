using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Inventory_Slot : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    Image item_image;
    bool is_dragging = false;
    [field: SerializeField] Item item;
    Transform image_parent;
    void Start()
    {
        image_parent = transform.GetChild(0);
        item_image = image_parent.GetChild(0).GetComponent<Image>();
    }

    void Update()
    {
        if (is_dragging)
            item_image.transform.position = Input.mousePosition;
        Update_UI();
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
            is_dragging = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
            is_dragging = false;
    }

    public bool Is_Empty()
    {
        return (this.item == null);
    }
    
    public void Assign_Item(Item item)
    {
        if (this.Is_Empty())
            this.item = item;
    }

    public void Clear_Item()
    {
        this.item = null;
    }

    public void Update_UI()
    {
        if(this.item != null)
            item_image.sprite = item.item_icon;
        else
            item_image.sprite = null;
    }

}
