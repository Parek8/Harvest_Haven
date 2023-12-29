using Cinemachine;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(Character_Stats))]
[RequireComponent(typeof(Player_Health))]
public class Character_Behaviour : MonoBehaviour
{
    [field: SerializeField] UI_Behaviour inventory_screen;
    [field: SerializeField] List<Inventory_Slot> hotbar = new();
    [field: SerializeField] Camera _normalCam;
    [field: SerializeField] Transform _aimStart;
    [field: SerializeField] TMP_Text _itemText;

    Character_Stats stats;
    Inventory inventory;
    Animator animator;
    Player_Movement movement;
    PlayerState _state = PlayerState.normal;
    Plot _lastPlot;

    bool _isAttacking = false;
    public bool IsAttacking => _isAttacking;

    private void Start()
    {
        stats = GetComponent<Character_Stats>();
        inventory = GetComponent<Inventory>();
        animator = GetComponent<Animator>();
        movement = GetComponent<Player_Movement>();

        stats.AddPlayerStateListener(delegate (PlayerState _newState) { this._state = _newState; });
    }

    void Update()
    {
        // __DEBUG__
        if (Input.GetKeyDown(KeyCode.Escape))
            GameManager.game_manager.saveManager.SaveInventory();

        if (Input.GetKeyDown(KeyCode.Space))
            Day_Cycle.Next_Day();

        // __NON_DEBUG__
        bool inv = Input_Manager.GetCustomAxisRawDown("Inventory");
        if(inv)
            inventory_screen.Change_State();

        for (int i = 0; i < hotbar.Count; i++)
        {
            if (Input_Manager.GetCustomAxisRawDown($"Slot_{i+1}"))
                hotbar[i].Equip();
        }

        if(!_isAttacking)
        {
            bool att = Input_Manager.GetCustomAxisRawDown("Attack");
            if (att && inventory.IsEquippedItemTool())
            {
                animator.SetTrigger("Attack");
                Hit_Destroyable();
            }
            else if (att && inventory.IsEquippedFood())
            {
                inventory.Equipped_Item.AssignedSlot.DecreaseCount();
                stats.Saturate(inventory.Equipped_Item);
            }
        }
     
        // First Interactable Has Bouncing Interact Key Above

        RaycastHit _highInfo;
        RaycastHit _inteInfo;
        _aimStart.rotation = _normalCam.transform.rotation;
        if (Physics.Raycast(_aimStart.position, _aimStart.forward * stats.pick_up_distance * 3, out _highInfo, stats.pick_up_distance*3, stats.highlightable_layers))
        {
            Highlightable _object;
            if (_highInfo.collider.TryGetComponent(out _object))
                _itemText.text = _object.GetMessage();
        }
        if (Physics.Raycast(_aimStart.position, _aimStart.forward * stats.pick_up_distance * 3, out _inteInfo, stats.pick_up_distance * 3, stats.interactable_layers))
        {
            Interactable _object;
            Plot _plot;
            if (_inteInfo.collider.TryGetComponent(out _object) && Input_Manager.GetCustomAxisRawDown("Interact") && _state == PlayerState.normal)
                _object.Interact();
            else if (_inteInfo.collider.TryGetComponent(out _plot))
            {
                if (_lastPlot != null)
                    _lastPlot.Lowlight();
                _plot.Highlight();
                _lastPlot = _plot;
                if (Input_Manager.GetCustomAxisRawDown("Interact"))
                    SeedOrWater(_plot);
            }
        }
        else
            _itemText.text = "";
    }
    public void StartAttacking()
    {
        _isAttacking = true;
        movement.enabled = false;
    }
    public void StopAttacking()
    {
        _isAttacking = false;
        movement.enabled = true;
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
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward * stats.attack_distance);

        if (Physics.Raycast(ray, out info, stats.attack_distance, stats.destroyable_layers))
        {
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
    private void SeedOrWater(Plot _plot)
    {
        if (_state == PlayerState.seeding)
            _plot.Plant(inventory.Equipped_Item.plantable_object);
        else if (_state == PlayerState.watering)
            _plot.Water(true);
        else
            Debug.Log("There was an Error!");
    }
}