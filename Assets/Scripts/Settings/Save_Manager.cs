using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class Save_Manager : MonoBehaviour
{
    //[field: SerializeField] string inventory_path = "./Saved/pl_inv.json";
    //List<Inventory_Slot> slots = new();
    //Dictionary<int, Inventory_Slot> slot_ids = new();
    //void OnEnable()
    //{
    //    GameManager.game_manager.Subscribe(() => { Load_Inventory(); });
    //    Main_Menu_Buttons.Subscribe_To_On_Exit(() => { Save_Inventory(); });
    //}
    
    #region Inventory
    //private void Get_Slots()
    //{
    //    slots.Clear();
    //    slot_ids.Clear();
    //    Inventory_Slot[] tmp_array = FindObjectsByType<Inventory_Slot>(FindObjectsInactive.Include, FindObjectsSortMode.None);

    //    foreach (Inventory_Slot slot in tmp_array)
    //    {
    //        slots.Add(slot);
    //        slot_ids.Add(slot.slot_index, slot);
    //    }
    //}
    //public void Save_Inventory()
    //{
    //    Get_Slots();
    //    if (GameManager.game_manager.Check_Items_Ids())
    //    {
    //        if (File.Exists(inventory_path))
    //        {
    //            string new_text = "";
    //            foreach (Inventory_Slot slot in slots)
    //            {
    //                if (slot.item != null)
    //                    new_text += $"{slot.slot_index}:{slot.item.id};";
    //            }
    //            File.WriteAllText(inventory_path, new_text);
    //        }
    //        else
    //        {
    //            using (StreamWriter writer = File.CreateText(inventory_path))
    //            {
    //                string new_text = "";
    //                foreach (Inventory_Slot slot in slots)
    //                {
    //                    if (slot.item != null)
    //                        new_text += $"{slot.slot_index}:{slot.item.id};";
    //                }
    //                writer.Write(new_text);
    //            }
    //            Debug.Log("Created a new file for Player_Inventory");
    //        }
    //        Debug.Log("Saved");
    //    }
    //}
    //public void Load_Inventory()
    //{
    //    Get_Slots();
    //    Debug.Log(File.Exists(inventory_path));
    //    if (File.Exists(inventory_path))
    //    {
    //        string[] s_slots = File.ReadAllText(inventory_path).Split(';');
    //        Debug.Log($"You've got {s_slots.Length} items saved!");
    //        for (int i = 0; i < s_slots.Length - 1; i++)
    //        {
    //            string slot = s_slots[i];
    //            int slot_id = Convert.ToInt32(slot.Split(':')[0]);
    //            Item item = GameManager.game_manager.Get_Item_By_Id(Convert.ToInt32(slot.Split(':')[1]));
    //            slot_ids[slot_id].Assign_Item_To_Slot(item);
    //        }
    //    }
    //    else
    //    {
    //        Debug.Log("No Save Found");
    //    }
    //}
    #endregion
}
