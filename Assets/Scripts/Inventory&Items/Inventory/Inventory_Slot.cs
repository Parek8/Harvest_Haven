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
    Transform image_parent;
    void Start()
    {
        //Transform childTransform = transform.GetChild(0);
        //item_image = childTransform.GetComponentInChildren<Image>();
    }

    void FixedUpdate()
    {
        if (is_dragging)
            transform.position = Input.mousePosition;
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
    public Item Get_Item()
    {
        return (this.item);
    }

}
