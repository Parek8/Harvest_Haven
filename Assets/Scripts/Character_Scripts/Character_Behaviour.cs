using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Character_Stats))]
[RequireComponent(typeof(Player_Health))]
public class Character_Behaviour : MonoBehaviour
{
    [field: SerializeField] UI_Behaviour inventory_screen;

    Character_Stats stats;
    private void Start()
    {
        stats = GetComponent<Character_Stats>();
    }

    void Update()
    {
        bool inv = Input_Manager.GetCustomAxisRawDown("Inventory");
        if(inv)
            inventory_screen.Change_State();

        bool att = Input_Manager.GetCustomAxisRawDown("Attack");
        if (att)
            Hit_Destroyable();

        if (Input.GetKeyDown(KeyCode.Space))
            stats.Saturate(1f);
    }
    private void Hit_Destroyable()
    {
        RaycastHit info;
        Ray ray = new Ray(transform.position, transform.forward * stats.attack_distance);
        if (Physics.Raycast(ray, out info, stats.attack_distance, stats.destroyable_layers))
        {
            Debug.Log($"Hit {info.transform.name}");
            info.collider.GetComponent<Destroyable>().Damage(stats.attack_damage);
        }
    }
}
