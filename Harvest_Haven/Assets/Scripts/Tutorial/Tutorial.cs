using System;
using System.Collections;
using TMPro;
using UnityEngine;

public sealed class Tutorial : MonoBehaviour
{
    public static Tutorial TutorialInstance
    {
        get
        {
            if (_instance == null)
                return null;

            return _instance;
        }
    }
    private void Awake()
    {
        if (_instance == null)
            _instance = this;
    }
    private static Tutorial _instance;

    private TutorialState _tutorialState = TutorialState.Movement;

    // Movement stage
    [field: Header("MOVEMENT")]
    [field: SerializeField] UI_Behaviour MoveDialog;
    [field: SerializeField] UI_Behaviour WCheck;
    [field: SerializeField] UI_Behaviour ACheck;
    [field: SerializeField] UI_Behaviour SCheck;
    [field: SerializeField] UI_Behaviour DCheck;
    bool _forward = false;
    bool _backward = false;
    bool _left = false;
    bool _right = false;

    [field: Header("INVENTORY")]
    [field: SerializeField] UI_Behaviour InvDialog;
    [field: SerializeField] UI_Behaviour TABCheck;
    [field: SerializeField] Character_Stats PlayerStats;
    [field: SerializeField] Character_Behaviour PlayerBehaviour;
    [field: SerializeField] Player_Health PlayerHealth;
    [field: SerializeField] Inventory PlayerInventory;
    bool _tab = false;

    [field: Header("INTERACT")]
    [field: SerializeField] UI_Behaviour IntDialog;
    [field: SerializeField] UI_Behaviour ECheck;
    [field: SerializeField] UI_Behaviour ShopHighlighter;
    bool _interacted = false;

    [field: Header("SWAP SLOTS")]
    [field: SerializeField] UI_Behaviour SwapDialog;
    Item _assignedItem;
    bool _swappedSlots = false;

    [field: Header("DESTROY OBJECTS")]
    [field: SerializeField] UI_Behaviour DesDialog;
    [field: SerializeField] Item AxeItem;
    [field: SerializeField] GameObject InstantiatedObject;
    [field: SerializeField] Transform InstantiatedObjectPosition;
    bool _destroyedObject = false;

    [field: Header("SHOPPING BUY")]
    [field: SerializeField] UI_Behaviour BuyDialog;
    bool _boughtItem = false;

    [field: Header("SHOPPING SELL")]
    [field: SerializeField] UI_Behaviour SelDialog;
    [field: SerializeField] Item PotatoItem;
    bool _soldItem = false;

    private void Start()
    {
        InitMovement();
    }
    private void FixedUpdate()
    {
        switch (_tutorialState)
        {
            case TutorialState.Movement:
                MovementTutorial();
                break;

            case TutorialState.Inventory:
                InventoryTutorial();
                break;

            case TutorialState.DragItems:
                DragItemsTutorial();
                break;

            case TutorialState.Interact:
                InteractTutorial();
                break;

            case TutorialState.ShoppingBuy:
                BuyTutorial();
                break;

            case TutorialState.ShoppingSell:
                SellTutorial();
                break;

            case TutorialState.Destroy:
                DestroyTutorial();
                break;

            case TutorialState.Craft:
                CraftTutorial();
                break;

            case TutorialState.Smelt:
                SmeltTutorial();
                break;

            default:
                Debug.Log("Tak a je to v pièi! :)");
                break;
        }
    }
    
    #region Init
    private void InitMovement()
    {
        MoveDialog.Show();
    }
    private IEnumerator InitInventory()
    {
        yield return new WaitForSeconds(0.5f);

        PlayerStats.enabled = true;
        PlayerBehaviour.enabled = true;
        PlayerHealth.enabled = true;
        PlayerInventory.enabled = true;
        InvDialog.Show();
        MoveDialog.Hide();
    }
    private IEnumerator InitInteract()
    {
        yield return new WaitForSeconds(0.5f);

        SwapDialog.Hide();
        ShopHighlighter.Show();
        IntDialog.Show();
    }

    internal void Interacted()
    {
        if (_tutorialState == TutorialState.Interact)
            _interacted = true;
    }
    internal void SwappedSlots()
    {
        if (_tutorialState == TutorialState.DragItems)
            _swappedSlots = true;
    }
    internal void DestroyedObject()
    {
        if (_tutorialState == TutorialState.Destroy)
            _destroyedObject = true;
    }
    private IEnumerator InitDestroy()
    {
        yield return new WaitForSeconds(0.5f);

        SelDialog.Hide();
        DesDialog.Show();

        GameManager.game_manager.player_inventory.Add(AxeItem);

        Instantiate(InstantiatedObject, InstantiatedObjectPosition.position, Quaternion.identity);
    }

    private IEnumerator InitDragSlots()
    {
        yield return new WaitForSeconds(0.5f);

        _assignedItem = GameManager.game_manager.all_items[UnityEngine.Random.Range(1, GameManager.game_manager.all_items.Count - 1)];
        GameManager.game_manager.player_inventory.Add(_assignedItem);

        InvDialog.Hide();
        SwapDialog.Show();
    }

    private IEnumerator InitBuy()
    {
        yield return new WaitForSeconds(0.5f);

        IntDialog.Hide();
        BuyDialog.Show();
    }
    private IEnumerator InitSell()
    {
        yield return new WaitForSeconds(0.5f);

        BuyDialog.Hide();
        SelDialog.Show();
    }

    internal void BoughtItem()
    {
        if (_tutorialState == TutorialState.ShoppingBuy)
            _boughtItem = true;
    }

    internal void SoldItem()
    {
        if (_tutorialState == TutorialState.ShoppingSell)
            _soldItem = true;
    }
    #endregion Init
    #region Tutorials
    private void MovementTutorial()
    {
        if (!_forward || !_backward || !_left || !_right)
        {
            if (Input_Manager.GetCustomKeyDown(KeybindNames.forward))
            {
                WCheck.Show();
                _forward = true;
            }
            if (Input_Manager.GetCustomKeyDown(KeybindNames.backward))
            {
                SCheck.Show();
                _backward = true;
            }
            if (Input_Manager.GetCustomKeyDown(KeybindNames.left_strafe))
            {
                ACheck.Show();
                _left = true;
            }
            if (Input_Manager.GetCustomKeyDown(KeybindNames.right_strafe))
            {
                DCheck.Show();
                _right = true;
            }
        }
        else
        {
            _tutorialState = TutorialState.Inventory;
            StartCoroutine(InitInventory());
        }
    }
    private void InventoryTutorial()
    {
        if (!_tab)
        {
            if (Input_Manager.GetCustomKeyDown(KeybindNames.inventory))
            {
                TABCheck.Show();
                _tab = true;
            }
        }
        else
        {
            _tutorialState = TutorialState.DragItems;
            StartCoroutine(InitDragSlots());
        }
    }

    private void DragItemsTutorial()
    {
        if (_swappedSlots)
        {
            _tutorialState = TutorialState.Interact;
            StartCoroutine(InitInteract());
        }
    }
    private void InteractTutorial()
    {
        if (_interacted)
        {
            ECheck.Show();
            _tutorialState = TutorialState.ShoppingBuy;
            StartCoroutine(InitBuy());
        }
    }

    private void BuyTutorial()
    {
        if (_boughtItem)
        {
            _tutorialState = TutorialState.ShoppingSell;
            StartCoroutine(InitSell());
        }
    }
    private void SellTutorial()
    {
        if (_soldItem)
        {
            _tutorialState = TutorialState.Destroy;
            StartCoroutine(InitDestroy());
        }
    }
    private void DestroyTutorial()
    {
        if (_destroyedObject)
        {
            _tutorialState = TutorialState.Craft;
            //StartCoroutine(InitInteract());
        }
    }
    private void CraftTutorial()
    {

    }
    private void SmeltTutorial()
    {

    }
    #endregion Tutorials
    private enum TutorialState
    {
        Movement,
        Inventory,
        DragItems,
        Interact,
        ShoppingBuy,
        ShoppingSell,
        Destroy,
        Craft,
        Smelt,
    }
}