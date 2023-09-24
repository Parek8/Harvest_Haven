using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_Behaviour : MonoBehaviour
{
    [field: SerializeField] UI_Behaviour inventory_screen;

    void Update()
    {
        bool inv = InputManager.GetCustomAxisRawDown("Inventory");
        if(inv)
            inventory_screen.Change_State();
    }
}
