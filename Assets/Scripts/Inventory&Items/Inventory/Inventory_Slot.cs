using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Inventory_Slot : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [field: SerializeField] Item item;
    [field: SerializeField] public int slot_index { get; private set; }

    [field: SerializeField] Image item_image; // I was way too lazy to fix teh thing commented in the Start() method
    bool is_dragging = false;
    Vector3 item_image_initial_position;
    Transform image_parent;
    void Start()
    {
        //Transform childTransform = transform.GetChild(0);
        //item_image = childTransform.GetComponentInChildren<Image>();
        item_image_initial_position = item_image.transform.localPosition;
    }

    void FixedUpdate()
    {
        if (is_dragging)
            item_image.transform.position = Input.mousePosition;
        Update_UI();
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
            if (!this.Is_Empty())
                is_dragging = true;
    }
    public float Return_Distance_From_Mouse()
    {
        return (Vector2.Distance(transform.position, Input.mousePosition));
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            is_dragging = false;
            item_image.transform.localPosition = item_image_initial_position;
            Swap_Slots();
        }
    }

    private void Swap_Slots()
    {
        Inventory_Slot closest_slot = GameManager.game_manager.player_inventory.Return_Closest_Slot();
        Item current_item = this.item;

        if(closest_slot.Is_Empty())
        {
            closest_slot.Assign_Item(current_item);
            this.Clear_Item();
        }
        else if(Is_Current_Slot(closest_slot))
        {
            is_dragging = false;
        }
        else
        {
            this.Assign_Item(closest_slot.item);
            closest_slot.Assign_Item(this.item);
            is_dragging = true;
        }
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
    public Item Get_Item()
    {
        return (this.item);
    }
    public bool Is_Current_Slot(Inventory_Slot compared_slot)
    {
        return (compared_slot == this);
    }
}