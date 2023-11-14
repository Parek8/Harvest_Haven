using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Character_Stats))]
[RequireComponent(typeof(Player_Health))]
public class Character_Behaviour : MonoBehaviour
{
    [field: SerializeField] UI_Behaviour inventory_screen;
    [field: SerializeField] List<Inventory_Slot> hotbar = new();

    Character_Stats stats;
    Inventory inventory;
    Animator animator;
    Player_Movement movement;

    bool _isAttacking = false;
    public bool IsAttacking => _isAttacking;
    private void Start()
    {
        stats = GetComponent<Character_Stats>();
        inventory = GetComponent<Inventory>();
        animator = GetComponent<Animator>();
        movement = GetComponent<Player_Movement>();
    }

    void Update()
    {
        bool inv = Input_Manager.GetCustomAxisRawDown("Inventory");
        if(inv)
            inventory_screen.Change_State();

        if (Input.GetKeyDown(KeyCode.Space))
            stats.Saturate(1f);

        for (int i = 0; i < hotbar.Count; i++)
        {
            if (Input_Manager.GetCustomAxisRawDown($"Slot_{i+1}"))
                hotbar[i].Equip();
        }

        if(!_isAttacking)
        {
            bool att = Input_Manager.GetCustomAxisRawDown("Attack");
            if (att)
            {
                animator.SetTrigger("Attack");
                Hit_Destroyable();
            }

        }
    }
    private void IsMoving(bool move)
    {
        movement.enabled = move;
    }
    public void ChangeAttacking()
    {
        _isAttacking = !_isAttacking;
        IsMoving(_isAttacking);
    }
    private void Hit_Destroyable()
    {
        RaycastHit info;
        Ray ray = new Ray(transform.position, transform.forward * stats.attack_distance);
        if (Physics.Raycast(ray, out info, stats.attack_distance, stats.destroyable_layers))
        {
            Debug.Log($"Hit {info.transform.name}");
            Destroyable _hit = info.collider.GetComponent<Destroyable>();
            Item _eq_item = inventory.Equipped_Item;
            if (_hit.Compare_Tag(_eq_item.tool_type) && _eq_item.is_tool)
            {
                _hit.Damage(_eq_item.tool_damage);

            }
            else
                Debug.Log("Wrong type");
        }
    }
}
