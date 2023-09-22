using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Item")]
public class Item : MonoBehaviour
{
    [field: SerializeField] int item_id = 0;
    [field: SerializeField] string item_name = "";
    [field: SerializeField] Sprite item_icon = null;
    [field: SerializeField] int count = 0;
    [field: SerializeField] bool is_eatable = false;
    [field: SerializeField] bool is_tool = false;
}