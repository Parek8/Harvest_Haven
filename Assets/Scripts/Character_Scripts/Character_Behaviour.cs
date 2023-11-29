using Cinemachine;
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneTemplate;
using UnityEngine;

[RequireComponent(typeof(Character_Stats))]
[RequireComponent(typeof(Player_Health))]
public class Character_Behaviour : MonoBehaviour
{
    [field: SerializeField] UI_Behaviour inventory_screen;
    [field: SerializeField] List<Inventory_Slot> hotbar = new();
    [field: SerializeField] Camera _normalCam;

    Character_Stats stats;
    Inventory inventory;
    Animator animator;
    Player_Movement movement;
    PlayerState _state = PlayerState.normal;

    bool _isAttacking = false;
    public bool IsAttacking => _isAttacking;
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
            if (att && inventory.IsEquippedItemTool())
            {
                animator.SetTrigger("Attack");
                Hit_Destroyable();
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
            GameManager.game_manager.saveManager.SaveInventory();

        if (_state == PlayerState.seeding)
            Seed();
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
        Ray ray = new Ray(transform.position, transform.forward * stats.attack_distance);
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
    private void Seed()
    {
        RaycastHit _hitInfo;
        Vector3 _mousePos = Input.mousePosition;
        //_mousePos.y = 100;
        Ray _ray = Camera.main.ScreenPointToRay(_mousePos);
        Debug.DrawRay(_mousePos, Vector3.down * 100, Color.cyan);

        if (Physics.Raycast(_ray, out _hitInfo, 100, stats.plot_layers))
        {
            _hitInfo.collider.gameObject.GetComponent<MeshRenderer>().materials[0].color = Color.red;
        }
    }
    private void Change_Camera_Angle(PlayerState _state)
    {
        this._state = _state;
        if (_state == PlayerState.seeding)
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
}
