using Cinemachine;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(Character_Stats))]
[RequireComponent(typeof(Player_Health))]
internal class Character_Behaviour : MonoBehaviour
{
    [field: SerializeField] UI_Behaviour inventory_screen;
    [field: SerializeField] List<Inventory_Slot> hotbar = new();
    [field: SerializeField] Camera _normalCam;
    [field: SerializeField] Transform _aimStart;
    [field: SerializeField] TMP_Text _itemText;

    PlayerStats stats;
    Inventory inventory;
    Animator animator;
    Player_Movement movement;
    PlayerState _state = PlayerState.normal;
    Highlightable _lastHighlighted;

    bool _isAttacking = false;
    internal bool IsAttacking => _isAttacking;

    int _equippedIndex = 0;

    private void Start()
    {
        stats = GetComponent<PlayerStats>();
        inventory = GetComponent<Inventory>();
        animator = GetComponent<Animator>();
        movement = GetComponent<Player_Movement>();

        stats.AddPlayerStateListener(delegate (PlayerState _newState) { this._state = _newState; });

        GameManager.game_manager.ResumeGame();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            Day_Cycle.Next_Day();

        // __NON_DEBUG__
        if (Input_Manager.GetCustomAxisRawDown("Inventory"))
            inventory_screen.Change_State();

        if (Input.GetKeyDown(KeyCode.Escape) && !GameManager.game_manager.is_game_paused)
            GameManager.game_manager.PauseGame();
        else if (Input.GetKeyDown(KeyCode.Escape) && GameManager.game_manager.is_game_paused)
            GameManager.game_manager.ResumeGame();

        for (int i = 0; i < hotbar.Count; i++)
        {
            if (Input_Manager.GetCustomAxisRawDown($"Slot_{i+1}"))
            {
                hotbar[i].Equip();
                _equippedIndex = i;
            }
        }
        inventory.ChangeEquippedItem(_equippedIndex);
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
                inventory.DecreaseItemCount(inventory.Equipped_Item);
                stats.Saturate(inventory.Equipped_Item);
            }
        }

        RaycastHit _highInfo;
        RaycastHit _inteInfo;
        _aimStart.rotation = _normalCam.transform.rotation;
        if (_lastHighlighted != null)
            _lastHighlighted.Lowlight();
        if (Physics.Raycast(_aimStart.position, _aimStart.forward * stats.pick_up_distance * 3, out _highInfo, stats.pick_up_distance*3, stats.highlightable_layers))
        {
            Highlightable _object;
            if (_highInfo.collider.TryGetComponent(out _object))
            {
                _itemText.text = _object.GetMessage();
                _lastHighlighted = _object;
                _object.Highlight();
            }
        }
        else
            _itemText.text = "";

        if (Physics.Raycast(_aimStart.position, _aimStart.forward * stats.pick_up_distance * 3, out _inteInfo, stats.pick_up_distance * 3, stats.interactable_layers))
        {
            Interactable _object;
            if (_inteInfo.collider.TryGetComponent(out _object) && Input_Manager.GetCustomAxisRawDown("Interact") && _state == PlayerState.normal)
            {
                if (Tutorial.TutorialInstance != null)
                    Tutorial.TutorialInstance.Interacted();

                _object.Interact();
            }
        }
        if (Physics.Raycast(_aimStart.position, _aimStart.forward * stats.pick_up_distance * 3, out _inteInfo, stats.pick_up_distance * 3, stats.plot_layers))
        {
            Plot _plot;

            if (_inteInfo.collider.TryGetComponent(out _plot))
                if (Input_Manager.GetCustomAxisRawDown("Interact"))
                    SeedOrWater(_plot);
        }
    }
    internal void StartAttacking()
    {
        _isAttacking = true;
        movement.enabled = false;
    }
    internal void StopAttacking()
    {
        _isAttacking = false;
        movement.enabled = true;
    }
    private void IsMoving(bool move)
    {
        movement.enabled = move;
    }
    internal void ChangeAttacking()
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
            if (_hit.Compare_Tag(_eq_item.ToolType) && _eq_item.IsTool)
            {
                _hit.Damage(_eq_item.ToolDamage);
            }
            else
                Debug.Log("Wrong type");
        }
    }
    private void SeedOrWater(Plot _plot)
    {
        if (_state == PlayerState.seeding)
            _plot.Plant(inventory.Equipped_Item.PlantableObject, inventory.Equipped_Item);
        else
            Debug.Log("There was an Error!");
    }
}