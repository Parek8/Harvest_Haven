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

    private List<Interactable> interactables = new();
    private void Start()
    {
        stats = GetComponent<Character_Stats>();
        inventory = GetComponent<Inventory>();
        animator = GetComponent<Animator>();
        movement = GetComponent<Player_Movement>();

        stats.AddPlayerStateListener(Change_Camera_Angle);
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

        if (_state == PlayerState.seeding || _state == PlayerState.watering)
            SeedOrWater();

     
        SortInteractablesList();

        // First Interactable Has Bouncing Interact Key Above
        if (_state == PlayerState.normal && Input_Manager.GetCustomAxisRawDown("Interact") && interactables.Count > 0)
            interactables[0].Interact();

        RaycastHit _hitInfo;
        _aimStart.rotation = _normalCam.transform.rotation;
        if (Physics.Raycast(_aimStart.position, _aimStart.forward * stats.pick_up_distance * 3, out _hitInfo, stats.pick_up_distance*3, stats.highlightable_layers))
        {
            Highlightable _object;
            if (_hitInfo.collider.TryGetComponent(out _object))
                _itemText.text = _object.GetMessage();
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
    private void SeedOrWater()
    {
        RaycastHit _hitInfo;
        Vector3 _mousePos = Input.mousePosition;
        //_mousePos.y = 100;
        Ray _ray = Camera.main.ScreenPointToRay(_mousePos);

        if (_lastPlot != null)
            _lastPlot.Lowlight();

        if (Physics.Raycast(_ray, out _hitInfo, 100, stats.plot_layers))
        {
            Plot _plot = _hitInfo.collider.GetComponent<Plot>();
            if (_plot != null)
            {
                _lastPlot = _plot;
                _plot.Highlight();

                if (Input_Manager.GetCustomAxisRawDown("Interact"))
                {
                    if (_state == PlayerState.seeding)
                        _plot.Plant(inventory.Equipped_Item.plantable_object);
                    else if (_state == PlayerState.watering)
                        _plot.Water(true);
                }
            }
        }
            
    }
    private void Change_Camera_Angle(PlayerState _state)
    {
        this._state = _state;
        if (_state == PlayerState.seeding || _state == PlayerState.watering)
        {
            _normalCam.transform.rotation = Quaternion.Euler(90, 0, 10);
            _normalCam.transform.localPosition = new Vector3(10, 70, 10);
            _normalCam.GetComponent<CinemachineBrain>().enabled = false;
            transform.GetComponent<Player_Movement>().enabled = false;
            GameManager.game_manager.Cursor_Needed(CursorLockMode.None);
        }
        else if (_state == PlayerState.normal)
        {
            _normalCam.GetComponent<CinemachineBrain>().enabled = true;
            transform.GetComponent<Player_Movement>().enabled = true;
            GameManager.game_manager.Cursor_Needed(CursorLockMode.Locked);
        }
    }

    public void AddToInteractableList(Interactable _interactable)
    {
        if (!interactables.Contains(_interactable))
            interactables.Add(_interactable);
    }
    public void RemoveFromInteractableList(Interactable _interactable)
    {
        if (interactables.Contains(_interactable))
            interactables.Remove(_interactable);
    }

    private void SortInteractablesList()
    {
        float _dis = float.MaxValue;
        Interactable _closest = null;
        for (int i = 0; i < interactables.Count; i++)
        {
            Interactable _h = interactables[i];
            if (Vector3.Distance(transform.position, _h.transform.position) < _dis)
                _closest = _h;
        }
    }
}
